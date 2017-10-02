#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Implementors receive a preview origin transform
	/// </summary>
	public interface IGetPreviewOrigin : IInjectedFunctionality<IGetPreviewOriginProvider>
	{
	}

	public interface IGetPreviewOriginProvider
	{
		/// <summary>
		/// Get the preview transform attached to the given rayOrigin
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin where the preview will occur</param>
		Transform GetPreviewOriginForRayOrigin(Transform rayOrigin);
	}

	public static class IGetPreviewOriginMethods
	{
		/// <summary>
		/// Get the preview transform attached to the given rayOrigin
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin where the preview will occur</param>
		public static Transform GetPreviewOriginForRayOrigin(this IGetPreviewOrigin @this, Transform rayOrigin)
		{
			return @this.provider.GetPreviewOriginForRayOrigin(rayOrigin);
		}
	}
}
#endif
