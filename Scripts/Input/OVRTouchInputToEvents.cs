#if UNITY_EDITOR && UNITY_2017_2_OR_NEWER
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.EditorVR;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.InputNew;
using UnityEngine.VR;
using UnityEngine.XR;
using UnityEngineInternal.Input;
using GenericControlEvent = UnityEngine.InputNew.GenericControlEvent;
using InputTracking = UnityEngine.XR.InputTracking;

[assembly: OptionalDependency("OVRInput", "ENABLE_OVR_INPUT")]

namespace UnityEditor.Experimental.EditorVR.Input
{
    using Input = UnityEngine.Input;

    /// <summary>
    /// Sends events to the input system based on native Oculus SDK calls
    /// </summary>
    sealed class OVRTouchInputToEvents : BaseInputToEvents
    {
        const string k_LeftControllerName = "Oculus Touch Controller - Left";
        const string k_RightControllerName = "Oculus Touch Controller - Right";

        const uint k_ControllerCount = 2;

        int[] m_JoystickIndices = new int[k_ControllerCount];
        string[,] m_AxisNames = new string[k_ControllerCount, JoystickInputToEvents.axisCount];
        float[,] m_LastAxisValues = new float[k_ControllerCount, JoystickInputToEvents.axisCount];
        Vector3[] m_LastPositionValues = new Vector3[k_ControllerCount];
        Quaternion[] m_LastRotationValues = new Quaternion[k_ControllerCount];

        void Update()
        {
            var isActive = false;
            var names = Input.GetJoystickNames();
            for (var i = 0; i < k_ControllerCount; i++)
            {
                var joystickName = i == 0 ? k_LeftControllerName : k_RightControllerName;
                var joystickIndex = Array.IndexOf(names, joystickName);
                if (joystickIndex >= 0)
                {
                    isActive = true;
                    m_JoystickIndices[i] = joystickIndex;
                    for (var j = 0; j < JoystickInputToEvents.axisCount; j++)
                    {
                        m_AxisNames[i, j] = string.Format("Analog{0}_Joy{1}", j + 1, joystickIndex + 1);
                    }
                }
            }

            active = isActive;

            if (!active)
                return;

            //for (VRInputDevice.Handedness hand = VRInputDevice.Handedness.Left;
            //	(int)hand <= (int)VRInputDevice.Handedness.Right;
            //	hand++)
            //{
            //	int deviceIndex = hand == VRInputDevice.Handedness.Left ? 3 : 4;
            //	var controller = (int)hand;
            //	//SendAxisEvents(controller, deviceIndex);
            //	var node = hand == VRInputDevice.Handedness.Left ? XRNode.LeftHand : XRNode.RightHand;
            //	SendTrackingEvents(node, controller, deviceIndex);
            //	//SendButtonEvents(m_JoystickIndices[controller], deviceIndex);
            //}
        }

        //void OnGUI()
        //{
        //	GUILayout.BeginHorizontal();
        //	for (var i = 0; i < Input.GetJoystickNames().Length; i++)
        //	{
        //		GUILayout.BeginVertical();
        //		var max = JoystickInputToEvents.axisCount + 1;
        //		for (var j = 1; j < max; j++)
        //		{
        //			var axisName = string.Format("Analog{0}_Joy{1}", j, i + 1);
        //			GUILayout.Label(string.Format("{0}: {1:f2}", axisName, Input.GetAxis(axisName)));
        //		}
        //		GUILayout.EndVertical();

        //		GUILayout.BeginVertical();
        //		var first = (int)KeyCode.Joystick1Button0 + i * 20;
        //		var last = (int)KeyCode.Joystick1Button19 + i * 20;

        //		for (var j = 0; j <= last - first; j++)
        //		{
        //			var buttonName = string.Format("Button {0}_Joy{1}", j, i + 1);
        //			GUILayout.Toggle(Input.GetKey((KeyCode)(j + first)), string.Format("{0}", buttonName));
        //		}
        //		GUILayout.EndVertical();
        //	}
        //	GUILayout.EndHorizontal();
        //}

        //void SendAxisEvents(int controller, int deviceIndex)
        //{
        //	for (var axis = 0; axis < JoystickInputToEvents.axisCount; ++axis)
        //	{
        //		var VRIndex = AxisIndexToVRIndex(axis);
        //		if (VRIndex == -1)
        //			continue;

        //		var value = Input.GetAxis(m_AxisNames[controller, axis]);
        //		if (Mathf.Approximately(m_LastAxisValues[controller, axis], value))
        //			continue;

        //		var inputEvent = InputSystem.CreateEvent<GenericControlEvent>();
        //		inputEvent.deviceType = typeof(VRInputDevice);
        //		inputEvent.deviceIndex = deviceIndex;
        //		inputEvent.controlIndex = VRIndex;
        //		inputEvent.value = value;

        //		m_LastAxisValues[controller, axis] = inputEvent.value;

        //		InputSystem.QueueEvent(inputEvent);
        //	}
        //}

        //static int AxisIndexToVRIndex(int axisIndex)
        //{
        //	switch (axisIndex)
        //	{
        //		case 10:
        //			return (int)VRInputDevice.VRControl.Trigger2;
        //		case 11:
        //			return (int)VRInputDevice.VRControl.Trigger2;
        //		case 12:
        //			return (int)VRInputDevice.VRControl.Trigger1;
        //		case 13:
        //			return (int)VRInputDevice.VRControl.Trigger1;
        //		default:
        //			return -1;
        //	}
        //}

        //void SendTrackingEvents(XRNode node, int controller, int deviceIndex)
        //{
        //    var localPosition = InputTracking.GetLocalPosition(node);
        //    var localRotation = InputTracking.GetLocalRotation(node);

        //    if (localPosition == m_LastPositionValues[controller] && localRotation == m_LastRotationValues[controller])
        //        return;

        //    var inputEvent = InputSystem.CreateEvent<TrackingEvent>();
        //    inputEvent.deviceType = typeof(VRInputDevice);
        //    inputEvent.deviceIndex = deviceIndex;
        //    inputEvent.localPosition = localPosition;
        //    inputEvent.localRotation = localRotation;

        //    m_LastPositionValues[controller] = inputEvent.localPosition;
        //    m_LastRotationValues[controller] = inputEvent.localRotation;

        //    InputSystem.QueueEvent(inputEvent);
        //}

        //static void SendButtonEvents(int joystickIndex, int deviceIndex)
        //{
        //	var first = KeyCode.Joystick1Button0 + joystickIndex * 20;
        //	var last = KeyCode.Joystick1Button19 + joystickIndex * 20;

        //	for (var i = 0; i <= last - first; i++)
        //	{
        //		var VRIndex = ButtonIndexToVRIndex(i);
        //		if (VRIndex == -1)
        //			continue;

        //		if (Input.GetKeyDown(i + first))
        //			SendButtonEvent(deviceIndex, VRIndex, 1.0f);
        //		if (Input.GetKeyUp(i + first))
        //			SendButtonEvent(deviceIndex, VRIndex, 0.0f);
        //	}
        //}

        //static int ButtonIndexToVRIndex(int buttonIndex)
        //{
        //    switch (buttonIndex)
        //    {
        //        case 0:
        //            return (int)VRInputDevice.VRControl.Action1;
        //        case 1:
        //            return (int)VRInputDevice.VRControl.Action2;
        //        default:
        //            return -1;
        //    }
        //}

        //static void SendButtonEvent(int deviceIndex, int controlIndex, float value)
        //{
        //	var inputEvent = InputSystem.CreateEvent<GenericControlEvent>();
        //	inputEvent.deviceType = typeof(VRInputDevice);
        //	inputEvent.deviceIndex = deviceIndex;
        //	inputEvent.controlIndex = controlIndex;
        //	inputEvent.value = value;

        //	Dictionary<int, float> vals;
        //	if (!NativeInputEventManager.values.TryGetValue(deviceIndex, out vals))
        //	{
        //		vals = new Dictionary<int, float>();
        //		NativeInputEventManager.values[deviceIndex] = vals;
        //	}

        //	vals[inputEvent.controlIndex] = inputEvent.value;

        //	InputSystem.QueueEvent(inputEvent);
        //}
    }
}
#endif
