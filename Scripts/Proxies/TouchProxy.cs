#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using UnityEngine.InputNew;
using TrackedControllerControl = UnityEngine.InputNew.TrackedController.TrackedControllerControl;
using OculusTouchControllerControl = UnityEngine.InputNew.OculusTouchController.OculusTouchControllerControl;

namespace UnityEditor.Experimental.EditorVR.Proxies
{
    sealed class TouchProxy : TwoHandedProxyBase<OculusTouchController>
    {
        public override IEnumerator Start()
        {
            // Touch controllers should be spawned under a root that corresponds to the head with no offsets, since the
            // local positions of the controllers will be provided that way.
#if UNITY_EDITOR
            EditorApplication.delayCall += () =>
            {
                if (this != null)
                    transform.localPosition = Vector3.zero;
            };
#else
            transform.localPosition = Vector3.zero;
#endif

            yield return base.Start();
        }

        protected override VRControl? VRControlFromControlIndex(int controlIndex)
        {
            switch (controlIndex)
            {
                case (int)TrackedControllerControl.Action1:
                    return VRControl.Action1;
                case (int)OculusTouchControllerControl.Action1Touch:
                    return VRControl.Action1Touch;
                case (int)OculusTouchControllerControl.Action2:
                    return VRControl.Action2;
                case (int)OculusTouchControllerControl.Action2Touch:
                    return VRControl.Action2Touch;
                case (int)OculusTouchControllerControl.Trigger:
                    return VRControl.Trigger1;
                case (int)OculusTouchControllerControl.TriggerTouch:
                    return VRControl.Trigger1Touch;
                case (int)OculusTouchControllerControl.TriggerNearTouch:
                    return VRControl.Trigger1NearTouch;
                case (int)OculusTouchControllerControl.HandTrigger:
                    return VRControl.Trigger2;
                case (int)OculusTouchControllerControl.ThumbRestTouch:
                    return VRControl.ThumbTouch;
                case (int)OculusTouchControllerControl.ThumbNearTouch:
                    return VRControl.ThumbNearTouch;
                case (int)OculusTouchControllerControl.Start:
                    return VRControl.Start;
                case (int)OculusTouchControllerControl.StickPress:
                    return VRControl.LeftStickButton;
                case (int)OculusTouchControllerControl.StickTouch:
                    return VRControl.LeftStickTouch;
                case (int)OculusTouchControllerControl.StickX:
                    return VRControl.LeftStickX;
                case (int)OculusTouchControllerControl.StickY:
                    return VRControl.LeftStickY;
                case (int)OculusTouchControllerControl.Stick:
                    return VRControl.LeftStick;
            }
            return null;
        }
    }
}
#endif
