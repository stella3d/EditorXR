#if UNITY_EDITOR && UNITY_EDITORVR
using System;
using System.Collections.Generic;

namespace UnityEditor.Experimental.EditorVR.Core
{
	partial class EditorVR
	{
		class Interfaces : Nested, IConnectInterfacesProvider
		{
			readonly HashSet<object> m_ConnectedInterfaces = new HashSet<object>();

			event Action<object, object> connectInterfaces;
			event Action<object, object> disconnectInterfaces;

			internal void AttachInterfaceConnectors(object @object)
			{
				var connector = @object as IInterfaceConnector;
				if (connector != null)
				{
					connectInterfaces += connector.ConnectInterface;
					disconnectInterfaces += connector.DisconnectInterface;
				}
			}

			public void ConnectInterfaces(object @object, object userData = null)
			{
				if (!m_ConnectedInterfaces.Add(@object))
					return;

				if (connectInterfaces != null)
					connectInterfaces(@object, userData);
			}

			public void DisconnectInterfaces(object @object, object userData = null)
			{
				m_ConnectedInterfaces.Remove(@object);

				if (disconnectInterfaces != null)
					disconnectInterfaces(@object, userData);
			}
		}
	}
}
#endif
