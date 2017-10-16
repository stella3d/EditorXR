#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.EditorVR.Utilities;
using UnityEngine;
using UnityEngine.InputNew;
using UnityEngine.XR;

using TrackedControllerControl = UnityEngine.InputNew.TrackedController.TrackedControllerControl;
using OpenVRControllerControl = UnityEngine.InputNew.OpenVRController.OpenVRControllerControl;

namespace UnityEditor.Experimental.EditorVR.Proxies
{
    sealed class ViveProxy : TwoHandedProxyBase<OpenVRController>
    {
        [SerializeField]
        GameObject m_LeftHandTouchProxyPrefab;

        [SerializeField]
        GameObject m_RightHandTouchProxyPrefab;

        bool m_IsOculus;

        public override void Awake()
        {
#if UNITY_2017_2_OR_NEWER
            m_IsOculus = XRDevice.model.IndexOf("oculus", StringComparison.OrdinalIgnoreCase) >= 0;
#endif

            if (m_IsOculus)
            {
                m_LeftHandProxyPrefab = m_LeftHandTouchProxyPrefab;
                m_RightHandProxyPrefab = m_RightHandTouchProxyPrefab;
            }

            base.Awake();

            var proxyHelper = m_LeftHand.GetComponent<ViveProxyHelper>();
            if (proxyHelper)
            {
                foreach (var tooltip in proxyHelper.leftTooltips)
                {
                    ObjectUtils.Destroy(tooltip);
                }
            }

            proxyHelper = m_RightHand.GetComponent<ViveProxyHelper>();
            if (proxyHelper)
            {
                foreach (var tooltip in proxyHelper.rightTooltips)
                {
                    ObjectUtils.Destroy(tooltip);
                }
            }

#if !ENABLE_STEAMVR_INPUT
            enabled = false;
#endif
        }

        static void PostAnimate(ProxyHelper.ButtonObject[] buttons, Dictionary<Transform, ProxyAnimator.TransformInfo> transformInfos, ActionMapInput input)
        {
            var proxyInput = (ProxyAnimatorInput)input;

            foreach (var button in buttons)
            {
                switch (button.control)
                {
                    case VRControl.LeftStickButton:
                        if (!proxyInput.stickButton.isHeld)
                        {
                            var buttonTransform = button.transform;
                            var info = transformInfos[buttonTransform];
                            info.rotationOffset = Vector3.zero;
                            info.Apply(buttonTransform);
                        }
                        break;
                    case VRControl.Analog0:
                        // Trackpad touch sphere
                        if (button.translateAxes != 0)
                            button.renderer.enabled = !Mathf.Approximately(proxyInput.stickX.value, 0) || !Mathf.Approximately(proxyInput.stickY.value, 0);
                        break;
                }
            }
        }

#if ENABLE_STEAMVR_INPUT
        public override IEnumerator Start()
        {
            yield return base.Start();

            if (!m_IsOculus)
            {
                m_LeftHand.GetComponent<ProxyAnimator>().postAnimate += PostAnimate;
                m_RightHand.GetComponent<ProxyAnimator>().postAnimate += PostAnimate;
            }
        }
#endif

        protected override VRControl? VRControlFromControlIndex(int controlIndex)
        {
            switch (controlIndex)
            {
                case (int)TrackedControllerControl.Action1:
                    return VRControl.Action2;
                case (int)OpenVRControllerControl.Trigger:
                    return VRControl.Trigger1;
                case (int)OpenVRControllerControl.TriggerTouch:
                    return VRControl.Trigger1Touch;
                case (int)OpenVRControllerControl.Grip:
                    return VRControl.Trigger2;
                case (int)OpenVRControllerControl.TrackpadPress:
                    return VRControl.LeftStickButton;
                case (int)OpenVRControllerControl.TrackpadTouch:
                    return VRControl.LeftStickTouch;
                case (int)OpenVRControllerControl.TrackpadX:
                    return VRControl.LeftStickX;
                case (int)OpenVRControllerControl.TrackpadY:
                    return VRControl.LeftStickY;
                case (int)OpenVRControllerControl.Trackpad:
                    return VRControl.LeftStick;
            }
            return null;
        }
    }
}
#endif
