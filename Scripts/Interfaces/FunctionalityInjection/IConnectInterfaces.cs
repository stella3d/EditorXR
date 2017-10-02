using System;
using UnityEngine;

#if UNITY_EDITOR
namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Decorates types that need to connect interfaces for spawned objects
	/// </summary>
	interface IConnectInterfaces : IInjectedFunctionality<IConnectInterfacesProvider>
	{
	}

	interface IConnectInterfacesProvider
	{
		/// <summary>
		/// Method provided by the system for connecting interfaces
		/// </summary>
		/// <param name="object">Object to connect interfaces on</param>
		/// <param name="userData">(Optional) extra data needed to connect interfaces on this object</param>
		void ConnectInterfaces(object @object, object userData = null);

		/// <summary>
		/// Method provided by the system for disconnecting interfaces
		/// </summary>
		/// <param name="object">Object to disconnect interfaces on</param>
		/// <param name="userData">(Optional) extra data needed to connect interfaces on this object</param>
		void DisconnectInterfaces(object @object, object userData = null);
	}

	static class IConnectInterfacesMethods
	{
		/// <summary>
		/// Method provided by the system for connecting interfaces
		/// </summary>
		/// <param name="object">Object to connect interfaces on</param>
		/// <param name="userData">(Optional) extra data needed to connect interfaces on this object</param>
		public static void ConnectInterfaces(this IConnectInterfaces @this, object @object, object userData = null)
		{
			@this.provider.ConnectInterfaces(@object, userData);
		}

		/// <summary>
		/// Method provided by the system for disconnecting interfaces
		/// </summary>
		/// <param name="object">Object to disconnect interfaces on</param>
		/// <param name="userData">(Optional) extra data needed to connect interfaces on this object</param>
		public static void DisconnectInterfaces(this IConnectInterfaces @this, object @object, object userData = null)
		{
			@this.provider.DisconnectInterfaces(@object, userData);
		}
	}

}
#endif
