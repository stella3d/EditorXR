#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Implementors receive a field grab origin transform
	/// </summary>
	public interface IGetFieldGrabOrigin : IInjectedFunctionality<IGetFieldGrabOriginProvider>
	{
	}

	public interface IGetFieldGrabOriginProvider
	{
		/// <summary>
		/// Get the field grab transform attached to the given rayOrigin
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that is grabbing the field</param>
		Transform GetFieldGrabOriginForRayOrigin(Transform rayOrigin);
	}

	public static class IGetFieldGrabOriginMethods
	{
		/// <summary>
		/// Get the field grab transform attached to the given rayOrigin
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that is grabbing the field</param>
		public static Transform GetFieldGrabOriginForRayOrigin(this IGetFieldGrabOrigin obj, Transform rayOrigin)
		{
			return obj.provider.GetFieldGrabOriginForRayOrigin(rayOrigin);
		}
	}
}
#endif
