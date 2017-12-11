#if UNITY_EDITOR
using System;
using System.Collections;
using System.Reflection;
using UnityEditor.Experimental.EditorVR.Helpers;
using UnityEditor.Experimental.EditorVR.Utilities;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR;

#if UNITY_POST_PROCESSING_STACK_V2
using UnityEngine.Rendering.PostProcessing;
#endif

#if ENABLE_STEAMVR_INPUT
using Valve.VR;
#endif

namespace UnityEditor.Experimental.EditorVR.Core
{
    [RequiresLayer(k_EXRLayerName)]
    sealed class VRView : EditorWindow
    {
        public const float HeadHeight = 1.7f;
        const string k_ShowDeviceView = "VRView.ShowDeviceView";
        const string k_UseCustomPreviewCamera = "VRView.UseCustomPreviewCamera";
        const string k_SceneCameraName = "EXR Scene Camera";
        const string k_EXRUICameraName = "EXR UI Camera";
        const string k_EXRLayerName = "EXROnly";

        static Camera s_ExistingSceneMainCamera;
        static bool s_ExistingSceneMainCameraEnabledState;

        DrawCameraMode m_RenderMode = DrawCameraMode.Textured;

        // To allow for alternate previews (e.g. smoothing)
        public static Camera customPreviewCamera
        {
            set
            {
                if (s_ActiveView)
                    s_ActiveView.m_CustomPreviewCamera = value;
            }
            get
            {
                return s_ActiveView && s_ActiveView.m_UseCustomPreviewCamera ?
                    s_ActiveView.m_CustomPreviewCamera : null;
            }
        }

        Camera m_CustomPreviewCamera;

        [NonSerialized]
        Camera m_SceneContentsCamera;

        [NonSerialized]
        Camera m_EXRCamera;

        LayerMask? m_CullingMask;
        LayerMask m_BackupCullingMask;
        RenderTexture m_TargetTexture;
        RenderTexture m_EXRTargetTexture;
        bool m_ShowDeviceView;
        EditorWindow[] m_EditorWindows;

        static VRView s_ActiveView;

        Transform m_CameraRig;

        bool m_HMDReady;
        bool m_UseCustomPreviewCamera;

        Rect m_ToggleDeviceViewRect = new Rect(0, 0, 0, 20); // Width will be set based on window size
        Rect m_PresentationCameraRect = new Rect(0, 0, 160, 20); // Y position and width will be set based on window size

        public static Transform cameraRig
        {
            get
            {
                if (s_ActiveView)
                    return s_ActiveView.m_CameraRig;

                return null;
            }
        }

        public static Camera viewerCamera
        {
            get
            {
                if (s_ActiveView)
                    return s_ActiveView.m_SceneContentsCamera;

                return null;
            }
        }

        public static VRView activeView
        {
            get { return s_ActiveView; }
        }

        public static bool showDeviceView
        {
            get { return s_ActiveView && s_ActiveView.m_ShowDeviceView; }
        }

        public static LayerMask cullingMask
        {
            private get
            {
                LayerMask mask = new LayerMask();

                if (s_ActiveView)
                    mask = (LayerMask) s_ActiveView.m_CullingMask;

                return mask;
            }

            set
            {
                if (s_ActiveView)
                    s_ActiveView.m_CullingMask = value;
            }
        }

        public static Vector3 headCenteredOrigin
        {
            get
            {
#if UNITY_2017_2_OR_NEWER
                return XRDevice.GetTrackingSpaceType() == TrackingSpaceType.Stationary ? Vector3.up * HeadHeight : Vector3.zero;
#else
                return Vector3.zero;
#endif
            }
        }

        public static event Action viewEnabled;
        public static event Action viewDisabled;
        public static event Action<VRView> beforeOnGUI;
        public static event Action<VRView> afterOnGUI;
        public static event Action<bool> hmdStatusChange;

        public Rect guiRect { get; private set; }

        public static Coroutine StartCoroutine(IEnumerator routine)
        {
            if (s_ActiveView && s_ActiveView.m_CameraRig)
            {
                var mb = s_ActiveView.m_CameraRig.GetComponent<EditorMonoBehaviour>();
                return mb.StartCoroutine(routine);
            }

            return null;
        }

        public void OnEnable()
        {
            Assert.IsNull(s_ActiveView, "Only one EditorXR should be active");

            const float nearClipPlane = 0.01f;
            const float farClipPlane = 1000f;
            var defaultContext = EditingContextManager.defaultContext;
            autoRepaintOnSceneChange = true;
            s_ActiveView = this;

            s_ExistingSceneMainCamera = Camera.main;
            // TODO: Copy camera settings when changing contexts
            if (defaultContext.copyExistingCameraSettings && s_ExistingSceneMainCamera && s_ExistingSceneMainCamera.enabled)
            {
                GameObject cameraGO = EditorUtility.CreateGameObjectWithHideFlags(k_SceneCameraName, HideFlags.HideAndDontSave);
                m_SceneContentsCamera = ObjectUtils.CopyComponent(s_ExistingSceneMainCamera, cameraGO);

                if (m_SceneContentsCamera.nearClipPlane > nearClipPlane)
                {
                    Debug.LogWarning("Copying settings from scene camera that is tagged 'MainCamera'." + Environment.NewLine +
                        " Clipping issues may occur with NearClipPlane values is greater than " + nearClipPlane);

                    m_SceneContentsCamera.nearClipPlane = nearClipPlane;
                }
#if UNITY_POST_PROCESSING_STACK_V2
                if (defaultContext.supportCameraFX)
                {
                    var postprocessing = s_ExistingSceneMainCamera.GetComponent<PostProcessLayer>();
                    if (postprocessing)
                        ObjectUtils.CopyComponent(postprocessing, cameraGO);
                }
#endif

                // TODO: Support multiple cameras
                if (m_SceneContentsCamera.clearFlags == CameraClearFlags.Nothing)
                    m_SceneContentsCamera.clearFlags = CameraClearFlags.SolidColor;

                m_SceneContentsCamera.stereoTargetEye = StereoTargetEyeMask.Both;
                // Force HDR on because of a bug in the mirror view
                m_SceneContentsCamera.allowHDR = true;

                //var EXRCameraTransform = EXRCameraGO.transform;
                //EXRCameraTransform.transform.parent = sceneCameraGO.transform;
                //EXRCameraTransform.transform.localPosition = Vector3.zero;
                //EXRCameraTransform.transform.localRotation = Quaternion.identity;
            }
            else
            {
                GameObject sceneCameraGO = EditorUtility.CreateGameObjectWithHideFlags(k_SceneCameraName, HideFlags.HideAndDontSave, typeof(Camera));
                m_SceneContentsCamera = sceneCameraGO.GetComponent<Camera>();

                m_SceneContentsCamera.nearClipPlane = nearClipPlane;
                m_SceneContentsCamera.farClipPlane = farClipPlane;
            }

            if (s_ExistingSceneMainCamera)
            {
                s_ExistingSceneMainCameraEnabledState = s_ExistingSceneMainCamera.enabled;
                s_ExistingSceneMainCamera.enabled = false; // Disable existing MainCamera in the scene
            }

            var exrLayer = LayerMask.NameToLayer(k_EXRLayerName);
            SetupEXRCamera(m_SceneContentsCamera);
            var sceneCameraCullingMask = m_SceneContentsCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(k_EXRLayerName)); // Disable the EXR UI layer in the culling mask
            cullingMask = (1 << exrLayer);
            m_CullingMask = (1 << exrLayer);
            m_BackupCullingMask = sceneCameraCullingMask;
            m_SceneContentsCamera.cullingMask = sceneCameraCullingMask;

            // Setup EXR UI Camera.  This Camera draws above the main camera
            var EXRCameraGO = EditorUtility.CreateGameObjectWithHideFlags(k_EXRUICameraName, HideFlags.None, typeof(Camera));
            m_EXRCamera = EXRCameraGO.GetComponent<Camera>();
            m_EXRCamera.cullingMask = (1 << exrLayer); // Enable only the EXR UI layer in the EXR UI camera's mask
            SetupEXRCamera(m_EXRCamera);
            m_EXRCamera.clearFlags = CameraClearFlags.Depth;
            m_EXRCamera.nearClipPlane = nearClipPlane; // TODO move to setup
            m_EXRCamera.farClipPlane = farClipPlane;
            m_EXRCamera.depth = m_SceneContentsCamera.depth + 1; // Draw EXR UI content atop the scene camera contents

            var rigGO = EditorUtility.CreateGameObjectWithHideFlags("VRCameraRig", HideFlags.HideAndDontSave, typeof(EditorMonoBehaviour));
            var sceneCamTransform = m_SceneContentsCamera.transform;
            m_CameraRig = rigGO.transform;
            m_EXRCamera.transform.parent = m_CameraRig;
            sceneCamTransform.parent = m_CameraRig;
            m_CameraRig.position = headCenteredOrigin;
            m_CameraRig.rotation = Quaternion.identity;

            m_ShowDeviceView = EditorPrefs.GetBool(k_ShowDeviceView, false);
            m_UseCustomPreviewCamera = EditorPrefs.GetBool(k_UseCustomPreviewCamera, false);

            // Disable other views to increase rendering performance for EditorVR
            SetOtherViewsEnabled(false);

#if UNITY_2017_2_OR_NEWER
            // XRSettings.enabled latches the reference pose for the current camera
            XRSettings.enabled = true;
#endif

            //Camera.SetupCurrent(m_EXRCamera);
            //Camera.SetupCurrent(m_SceneContentsCamera);

            var currentCamera = Camera.current;
            Camera.SetupCurrent(currentCamera); // TODO: verify current cam setup

            if (viewEnabled != null)
                viewEnabled();
        }

        void SetupEXRCamera(Camera camera)
        {
            camera.enabled = false; // Camera should be disabled after being setup for proper drawing
            camera.cameraType = CameraType.VR;
            camera.useOcclusionCulling = false;
        }

        public void OnDisable()
        {
            if (viewDisabled != null)
                viewDisabled();

#if UNITY_2017_2_OR_NEWER
            XRSettings.enabled = false;
#endif

            EditorPrefs.SetBool(k_ShowDeviceView, m_ShowDeviceView);
            EditorPrefs.SetBool(k_UseCustomPreviewCamera, m_UseCustomPreviewCamera);

            SetOtherViewsEnabled(true);

            if (m_CameraRig)
                DestroyImmediate(m_CameraRig.gameObject, true);

            Assert.IsNotNull(s_ActiveView, "EditorXR should have an active view");
            s_ActiveView = null;

            if (s_ExistingSceneMainCamera)
                s_ExistingSceneMainCamera.enabled = s_ExistingSceneMainCameraEnabledState;
        }

        void UpdateCameraTransform()
        {
            var cameraTransform = m_SceneContentsCamera.transform;
#if UNITY_2017_2_OR_NEWER
            cameraTransform.localPosition = InputTracking.GetLocalPosition(XRNode.Head);
            cameraTransform.localRotation = InputTracking.GetLocalRotation(XRNode.Head);
#endif
        }

        public void CreateCameraTargetTexture(ref RenderTexture renderTexture, Rect cameraRect, bool hdr)
        {
            bool useSRGBTarget = QualitySettings.activeColorSpace == ColorSpace.Linear;

            int msaa = Mathf.Max(1, QualitySettings.antiAliasing);

            RenderTextureFormat format = hdr ? RenderTextureFormat.ARGBHalf : RenderTextureFormat.ARGB32;
            if (renderTexture != null)
            {
                bool matchingSRGB = renderTexture != null && useSRGBTarget == renderTexture.sRGB;

                if (renderTexture.format != format || renderTexture.antiAliasing != msaa || !matchingSRGB)
                {
                    DestroyImmediate(renderTexture);
                    renderTexture = null;
                }
            }

            Rect actualCameraRect = cameraRect;
            int width = (int)actualCameraRect.width;
            int height = (int)actualCameraRect.height;

            if (renderTexture == null)
            {
                renderTexture = new RenderTexture(0, 0, 24, format);
                renderTexture.name = "Scene RT";
                renderTexture.antiAliasing = msaa;
                renderTexture.hideFlags = HideFlags.HideAndDontSave;
            }
            if (renderTexture.width != width || renderTexture.height != height)
            {
                renderTexture.Release();
                renderTexture.width = width;
                renderTexture.height = height;
            }
            renderTexture.Create();
        }

        void PrepareCameraTargetTexture(Rect cameraRect)
        {
            // Always render camera into a RT
            CreateCameraTargetTexture(ref m_TargetTexture, cameraRect, false);
            CreateCameraTargetTexture(ref m_EXRTargetTexture, cameraRect, false);
            var targetTexture = m_ShowDeviceView ? m_TargetTexture : null;
            var targetEXRCameraTexture = m_ShowDeviceView ? m_EXRTargetTexture : null;
            m_SceneContentsCamera.targetTexture = targetTexture;
            m_EXRCamera.targetTexture = targetEXRCameraTexture;
#if UNITY_2017_2_OR_NEWER
            XRSettings.showDeviceView = !customPreviewCamera && m_ShowDeviceView;
#endif
        }

        void OnGUI()
        {
            if (beforeOnGUI != null)
                beforeOnGUI(this);

            var height = position.height;
            var width = position.width;
            var rect = guiRect;
            rect.x = 0;
            rect.y = 0;
            rect.width = width;
            rect.height = height;
            guiRect = rect;
            var cameraRect = EditorGUIUtility.PointsToPixels(guiRect);
            PrepareCameraTargetTexture(cameraRect);

            m_SceneContentsCamera.cullingMask = m_CullingMask.HasValue ? (int)m_BackupCullingMask : UnityEditor.Tools.visibleLayers;

            DoDrawCamera(guiRect);

            Event e = Event.current;
            if (m_ShowDeviceView)
            {
                if (e.type == EventType.Repaint)
                {
                    GL.sRGBWrite = (QualitySettings.activeColorSpace == ColorSpace.Linear);
                    var renderTexture = customPreviewCamera && customPreviewCamera.targetTexture ? customPreviewCamera.targetTexture : m_TargetTexture;
                    GUI.DrawTexture(guiRect, renderTexture, ScaleMode.StretchToFill, false);
                    GL.sRGBWrite = false;
                }
            }

            m_ToggleDeviceViewRect.width = width;
            m_PresentationCameraRect.y = height - m_PresentationCameraRect.height;
            m_PresentationCameraRect.width = width;

            if (GUI.Button(m_ToggleDeviceViewRect, "Toggle Device View", EditorStyles.toolbarButton))
                m_ShowDeviceView = !m_ShowDeviceView;

            if (m_CustomPreviewCamera)
                m_UseCustomPreviewCamera = GUI.Toggle(m_PresentationCameraRect, m_UseCustomPreviewCamera, "Use Presentation Camera");

            if (afterOnGUI != null)
                afterOnGUI(this);
        }

        void DoDrawCamera(Rect rect)
        {
            if (!m_SceneContentsCamera.gameObject.activeInHierarchy)
                return;

#if UNITY_2017_2_OR_NEWER
            if (!XRDevice.isPresent)
                return;
#endif

            UnityEditor.Handles.DrawCamera(rect, m_SceneContentsCamera, m_RenderMode);
            UnityEditor.Handles.DrawCamera(rect, m_EXRCamera, m_RenderMode);
            if (Event.current.type == EventType.Repaint)
            {
                GUI.matrix = Matrix4x4.identity; // Need to push GUI matrix back to GPU after camera rendering
                RenderTexture.active = null; // Clean up after DrawCamera
            }
        }

        private void Update()
        {
            // If code is compiling, then we need to clean up the window resources before classes get re-initialized
            if (EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Close();
                return;
            }

            // Our camera is disabled, so it doesn't get automatically updated to HMD values until it renders
            UpdateCameraTransform();

            UpdateHMDStatus();

            SetSceneViewsAutoRepaint(false);
        }

        void UpdateHMDStatus()
        {
            if (hmdStatusChange != null)
            {
                var ready = GetIsUserPresent();
                if (m_HMDReady != ready)
                {
                    m_HMDReady = ready;
                    hmdStatusChange(ready);
                }
            }
        }

        static bool GetIsUserPresent()
        {
#if UNITY_2017_2_OR_NEWER
#if ENABLE_OVR_INPUT
            if (XRSettings.loadedDeviceName == "Oculus")
                return OVRPlugin.userPresent;
#endif
#if ENABLE_STEAMVR_INPUT
            if (XRSettings.loadedDeviceName == "OpenVR")
                return OpenVR.System.GetTrackedDeviceActivityLevel(0) == EDeviceActivityLevel.k_EDeviceActivityLevel_UserInteraction;
#endif
#endif
            return true;
        }

        void SetGameViewsAutoRepaint(bool enabled)
        {
            var asm = Assembly.GetAssembly(typeof(UnityEditor.EditorWindow));
            var type = asm.GetType("UnityEditor.GameView");
            SetAutoRepaintOnSceneChanged(type, enabled);
        }

        void SetSceneViewsAutoRepaint(bool enabled)
        {
            SetAutoRepaintOnSceneChanged(typeof(SceneView), enabled);
        }

        void SetOtherViewsEnabled(bool enabled)
        {
            SetGameViewsAutoRepaint(enabled);
            SetSceneViewsAutoRepaint(enabled);
        }

        void SetAutoRepaintOnSceneChanged(Type viewType, bool enabled)
        {
            if (m_EditorWindows == null)
                m_EditorWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();

            var windowCount = m_EditorWindows.Length;
            var mouseOverWindow = EditorWindow.mouseOverWindow;
            for (int i = 0; i < windowCount; i++)
            {
                var window = m_EditorWindows[i];
                if (window.GetType() == viewType)
                    window.autoRepaintOnSceneChange = enabled || (window == mouseOverWindow);
            }
        }
    }
}
#endif
