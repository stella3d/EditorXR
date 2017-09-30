#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Provides access to the gameobjects that represent the VR player
	/// </summary>
	public interface IGetVRPlayerObjects : IInjectedFunctionality<IGetVRPlayerObjectsProvider>
	{
	}

	public interface IGetVRPlayerObjectsProvider
	{
		/// <summary>
		/// Returns objects that are used to represent the VR player
		/// </summary>
		List<Renderer> GetVRPlayerObjects();
	}

	public static class IGetVRPlayerObjectsMethods
	{
		/// <summary>
		/// Returns objects that are used to represent the VR player
		/// </summary>
		public static List<Renderer> GetVRPlayerObjects(this IGetVRPlayerObjects @this)
		{
			return @this.provider.GetVRPlayerObjects();
		}
	}
}
#endif
