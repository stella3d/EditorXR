using System;
using System.Collections.Generic;
using UnityEditor.Experimental.EditorVR;
using UnityEngine;
using UnityEngineInternal.Input;
using XRAuthoring.Utilities;

namespace XRAuthoring.Input
{
	/// <summary>
	/// Listens for device discoveries reported by the native runtime and keeps a persistent list of 
	/// native device information for tracked nodes.
	/// </summary>
	public class TrackedNodeDeviceManager : ScriptableSettings<TrackedNodeDeviceManager>
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

		[Serializable]
		struct TrackedNodeDeviceRecord
		{
			public NativeInputDeviceInfo deviceInfo;
			public Node node;
		}

		[SerializeField]
		List<TrackedNodeDeviceRecord> m_TrackedNodeDevices = new List<TrackedNodeDeviceRecord>();

		Dictionary<Node, int> m_NodeDeviceIDs = new Dictionary<Node, int>();

		void OnEnable()
		{
			Debug.Log("TrackedNodeDeviceManager OnEnable");
			foreach (var deviceRecord in m_TrackedNodeDevices)
			{
				Debug.Log("Existing device descriptor: " + deviceRecord.deviceInfo.deviceDescriptor);
				Debug.Log("Existing device node: " + deviceRecord.node);
				m_NodeDeviceIDs[deviceRecord.node] = deviceRecord.deviceInfo.deviceId;
			}
			NativeInputSystem.onDeviceDiscovered += OnDeviceDiscovered;
		}

		void OnDisable()
		{
			Debug.Log("TrackedNodeDeviceManager OnDisable");
			NativeInputSystem.onDeviceDiscovered -= OnDeviceDiscovered;
		}

		/// <summary>
		/// Returns the native ID for the device associated with the given node.
		/// </summary>
		public static int DeviceIDForTrackedNode(Node node)
		{
			if (instance.m_NodeDeviceIDs.ContainsKey(node))
			{
				return instance.m_NodeDeviceIDs[node];
			}
			return -1;
		}

		[RuntimeInitializeOnLoadMethod]
		static void InitializeInstanceOnRuntimeLoad()
		{
			Debug.Log("TrackedNodeDeviceManager InitializeInstanceOnRuntimeLoad");
			// Ensure that we start listening for devices as soon as possible.
			var initializedInstance = instance;
		}

		void OnDeviceDiscovered(NativeInputDeviceInfo deviceInfo)
		{
			var descriptor = JsonUtility.FromJson<NativeDeviceDescriptor>(deviceInfo.deviceDescriptor);
			if (descriptor.type == "Controller")
			{
				Node? node = null;
				if (descriptor.product.Contains("Left"))
				{
					node = Node.LeftHand;
				}
				else if (descriptor.product.Contains("Right"))
				{
					node = Node.RightHand;
				}
				if (node != null)
				{
					Debug.Log("TrackedNodeDeviceManager node device discovered: " + deviceInfo.deviceDescriptor);
					m_TrackedNodeDevices.Add(new TrackedNodeDeviceRecord { deviceInfo = deviceInfo, node = node.Value });
					m_NodeDeviceIDs[node.Value] = deviceInfo.deviceId;
				}
			}
		}
	}
}
