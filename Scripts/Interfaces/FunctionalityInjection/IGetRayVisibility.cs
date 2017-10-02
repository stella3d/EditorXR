#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Provides access to checks that can test whether parts of the default ray are visible
	/// </summary>
	public interface IGetRayVisibility : IInjectedFunctionality<IGetRayVisibilityProvider>
	{
	}

	public interface IGetRayVisibilityProvider
	{
		/// <summary>
		/// Returns whether the specified ray is visible
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that is being checked</param>
		bool IsRayVisible(Transform rayOrigin);

		/// <summary>
		/// Returns whether the specified cone is visible
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that is being checked</param>
		bool IsConeVisible(Transform rayOrigin);
	}

	public static class IGetRayVisibilityMethods
	{
		/// <summary>
		/// Returns whether the specified ray is visible
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that is being checked</param>
		public static bool IsRayVisible(this IGetRayVisibility @this, Transform rayOrigin)
		{
			return @this.provider.IsRayVisible(rayOrigin);
		}

		/// <summary>
		/// Returns whether the specified cone is visible
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that is being checked</param>
		public static bool IsConeVisible(this IGetRayVisibility @this, Transform rayOrigin)
		{
			return @this.provider.IsConeVisible(rayOrigin);
		}
	}
}
#endif
