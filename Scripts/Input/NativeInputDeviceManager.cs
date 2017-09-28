using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal.Input;
using XRAuthoring.Utilities;

namespace XRAuthoring.Input
{
	/// <summary>
	/// Listens for device discoveries reported by the native runtime and keeps a persistent list of 
	/// native device information.
	/// </summary>
	public class NativeInputDeviceManager : ScriptableSettings<NativeInputDeviceManager>
	{
		struct NativeDeviceDescriptor
		{
			public string @interface;
			public string type;
			public string product;
			public string manufacturer;
			public string version;
			public string serial;
		}

		List<NativeInputDeviceInfo> m_NativeDevices = new List<NativeInputDeviceInfo>();

		void OnEnable()
		{
			Debug.Log("NativeInputDevceManager OnEnable");
			foreach (var deviceInfo in m_NativeDevices)
			{
				Debug.Log("Existing device: " + deviceInfo.deviceDescriptor);
			}
			NativeInputSystem.onDeviceDiscovered += OnDeviceDiscovered;
		}

		void OnDisable()
		{
			Debug.Log("NativeInputDeviceManager OnDisable");
			NativeInputSystem.onDeviceDiscovered -= OnDeviceDiscovered;
		}

		[RuntimeInitializeOnLoadMethod]
		static void InitializeInstanceOnRuntimeLoad()
		{
			Debug.Log("NativeInputDeviceManager InitializeInstanceOnRuntimeLoad");
			// Ensure that we start listening for devices as soon as possible.
			var initializedInstance = instance;
		}

		void OnDeviceDiscovered(NativeInputDeviceInfo deviceInfo)
		{
			var descriptor = JsonUtility.FromJson<NativeDeviceDescriptor>(deviceInfo.deviceDescriptor);
			Debug.Log("NativeInputDeviceManager device discovered: " + deviceInfo.deviceDescriptor);
			m_NativeDevices.Add(deviceInfo);
		}
	}
}
