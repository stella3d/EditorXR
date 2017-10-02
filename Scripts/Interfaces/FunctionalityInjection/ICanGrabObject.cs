#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Provides CanGrabObject method used to check whether direct selection is possible on an object
	/// </summary>
	public interface ICanGrabObject : IInjectedFunctionality<ICanGrabObjectProvider>
	{
	}

	public interface ICanGrabObjectProvider
	{
		/// <summary>
		/// Returns true if the object can be grabbed
		/// </summary>
		/// <param name="go">The selection</param>
		/// <param name="rayOrigin">The rayOrigin of the proxy that is looking to grab</param>
		bool CanGrabObject(GameObject go, Transform rayOrigin);
	}

	public static class ICanGrabObjectMethods
	{
		/// <summary>
		/// Returns true if the object can be grabbed
		/// </summary>
		/// <param name="go">The selection</param>
		/// <param name="rayOrigin">The rayOrigin of the proxy that is looking to grab</param>
		public static bool CanGrabObject(this ICanGrabObject @this, GameObject go, Transform rayOrigin)
		{
			return @this.provider.CanGrabObject(go, rayOrigin);
		}
	}
}
#endif
