#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor.Experimental.EditorVR.Data;
using UnityEditor.Experimental.EditorVR.Utilities;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

namespace UnityEditor.Experimental.EditorVR.Workspaces
{
	[RequiresTag(k_MiniWorldCameraTag)]
	[RequiresTag(ShowInMiniWorldTag)]
	sealed class MiniWorldRenderer : MonoBehaviour
	{
		public const string ShowInMiniWorldTag = "ShowInMiniWorld";
		const string k_MiniWorldCameraTag = "MiniWorldCamera";
		const float k_MinScale = 0.001f;

		static int s_DefaultLayer;

		public List<Renderer> ignoreList
		{
			set
			{
				m_IgnoreList = value;
				var count = m_IgnoreList == null ? 0 : m_IgnoreList.Count;
				if (m_IgnoreObjectRendererEnabled == null || count > m_IgnoreObjectRendererEnabled.Length)
				{
					m_IgnoredObjectLayer = new int[count];
					m_IgnoreObjectRendererEnabled = new bool[count];
				}
			}
		}

	    public SpatialHash<Renderer> spatialHash
	    {
	        set { m_SpatialHash = value; }
	    }
        
        List<Renderer> m_DrawList = new List<Renderer>();
        List<Renderer> m_IgnoreList = new List<Renderer>();
	    SpatialHash<Renderer> m_SpatialHash;
        List<Camera> m_CommandBufferCameras = new List<Camera>();

	    CommandBuffer m_MiniWorldDrawCommands;
        CameraEvent m_MiniWorldStep = CameraEvent.AfterForwardAlpha;

		int[] m_IgnoredObjectLayer;
		bool[] m_IgnoreObjectRendererEnabled;

		public MiniWorld miniWorld { private get; set; }
		public LayerMask cullingMask { private get; set; }

		public Matrix4x4 GetWorldToCameraMatrix(Camera camera)
		{
			return camera.worldToCameraMatrix * miniWorld.miniToReferenceMatrix;
		}

		void Awake()
		{
            m_MiniWorldDrawCommands = new CommandBuffer();
			s_DefaultLayer = LayerMask.NameToLayer("Default");
		}

		void OnEnable()
		{
		    foreach (var currentCamera in Resources.FindObjectsOfTypeAll<Camera>())
		    {
		        if (currentCamera.gameObject.CompareTag(k_MiniWorldCameraTag))
		        {
		            continue;
		        }
                currentCamera.AddCommandBuffer(m_MiniWorldStep, m_MiniWorldDrawCommands);
                m_CommandBufferCameras.Add(currentCamera);
		    }
		}

		void OnDisable()
		{
		    foreach (var currentCamera in m_CommandBufferCameras)
		    {
		        if (currentCamera != null)
		        {
		            currentCamera.RemoveCommandBuffer(m_MiniWorldStep, m_MiniWorldDrawCommands);
		        }
                
		    }
            m_CommandBufferCameras.Clear();
		}

	    void Update()
	    {
            //@Todo make this happen every x frames
            m_DrawList.Clear();
            m_MiniWorldDrawCommands.Clear();

	        if (m_SpatialHash == null)
	        {
	            return;
	        }

	        if (!miniWorld || (miniWorld && miniWorld.transform.lossyScale.magnitude < k_MinScale))
	        {
	            return;
	        }

            m_MiniWorldDrawCommands.SetViewMatrix(miniWorld.miniToReferenceMatrix*Matrix4x4.Scale(new Vector3(1,1,-1)));
	        // See what is in our spatial hash
	        m_SpatialHash.GetIntersections(m_DrawList, miniWorld.referenceBounds);

            // Build a command buffer from this list while removing ignored elements
            foreach(var toDraw in m_DrawList)
	        {
	            if (!toDraw.gameObject.activeInHierarchy)
	            {
	                continue;
	            }
	            if (m_IgnoreList.Contains(toDraw) && !toDraw.CompareTag(ShowInMiniWorldTag))
	            {
	                continue;
	            }
                m_MiniWorldDrawCommands.DrawRenderer(toDraw, toDraw.sharedMaterial);
	        }
	    }

/*
		void RenderMiniWorld(Camera camera)
		{
			// Do not render if miniWorld scale is too low to avoid errors in the console
			if (!camera.gameObject.CompareTag(k_MiniWorldCameraTag) && miniWorld && miniWorld.transform.lossyScale.magnitude > k_MinScale)
			{
				m_MiniCamera.CopyFrom(camera);

				m_MiniCamera.cullingMask = cullingMask;
				m_MiniCamera.cameraType = CameraType.Game;
				m_MiniCamera.clearFlags = CameraClearFlags.Nothing;
				m_MiniCamera.worldToCameraMatrix = GetWorldToCameraMatrix(camera);

				var referenceBounds = miniWorld.referenceBounds;
				var inverseRotation = Quaternion.Inverse(miniWorld.referenceTransform.rotation);

				Shader.SetGlobalVector("_GlobalClipCenter", inverseRotation * referenceBounds.center);
				Shader.SetGlobalVector("_GlobalClipExtents", referenceBounds.extents);
				Shader.SetGlobalMatrix("_InverseRotation", Matrix4x4.TRS(Vector3.zero, inverseRotation, Vector3.one));

				for (var i = 0; i < m_IgnoreList.Count; i++)
				{
					var hiddenRenderer = m_IgnoreList[i];
					if (!hiddenRenderer || !hiddenRenderer.gameObject.activeInHierarchy)
						continue;

					if (hiddenRenderer.CompareTag(ShowInMiniWorldTag))
					{
						m_IgnoredObjectLayer[i] = hiddenRenderer.gameObject.layer;
						hiddenRenderer.gameObject.layer = s_DefaultLayer;
					}
					else
					{
						m_IgnoreObjectRendererEnabled[i] = hiddenRenderer.enabled;
						hiddenRenderer.enabled = false;
					}
				}

				// Because this is a manual render it is necessary to set the target texture to whatever the active RT 
				// is, which would either be the left/right eye in the case of VR rendering, or the custom SceneView RT 
				// in the case of the SceneView rendering, etc.
				m_MiniCamera.targetTexture = RenderTexture.active;

				m_MiniCamera.SetReplacementShader(m_ClipShader, "RenderType");
				m_MiniCamera.Render();

				for (var i = 0; i < m_IgnoreList.Count; i++)
				{
					var hiddenRenderer = m_IgnoreList[i];
					if (!hiddenRenderer || !hiddenRenderer.gameObject.activeInHierarchy)
						continue;

					if (hiddenRenderer.CompareTag(ShowInMiniWorldTag))
						hiddenRenderer.gameObject.layer = m_IgnoredObjectLayer[i];
					else
						hiddenRenderer.enabled = m_IgnoreObjectRendererEnabled[i];
				}
			}
		}*/
	}
}
#endif
