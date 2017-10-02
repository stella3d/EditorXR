#if UNITY_EDITOR
using System;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Provides access to transform roots for custom menus
	/// </summary>
	public interface IUsesCustomMenuOrigins : IInjectedFunctionality<IUsesCustomMenuOriginsProvider>
	{
	}

	public interface IUsesCustomMenuOriginsProvider
	{
		/// <summary>
		/// Get the root transform for custom menus for a given ray origin
		/// </summary>
		/// <param name="rayOrigin">The ray origin for which we want custom the menu origin</param>
		/// <returns></returns>
		Transform GetCustomMenuOrigin(Transform rayOrigin);

		/// <summary>
		/// Get the root transform for custom alternate menus for a given ray origin
		/// </summary>
		/// <param name="rayOrigin">The ray origin for which we want the alternate menu origin</param>
		/// <returns></returns>
		Transform GetCustomAlternateMenuOrigin(Transform rayOrigin);
	}

	public static class IUsesCustomMenuOriginsMethods
	{
		/// <summary>
		/// Get the root transform for custom menus for a given ray origin
		/// </summary>
		/// <param name="rayOrigin">The ray origin for which we want custom the menu origin</param>
		/// <returns></returns>
		public static Transform GetCustomMenuOrigin(this IUsesCustomMenuOrigins @this, Transform rayOrigin)
		{
			return @this.provider.GetCustomMenuOrigin(rayOrigin);
		}

		/// <summary>
		/// Get the root transform for custom alternate menus for a given ray origin
		/// </summary>
		/// <param name="rayOrigin">The ray origin for which we want the alternate menu origin</param>
		/// <returns></returns>
		public static Transform GetCustomAlternateMenuOrigin(this IUsesCustomMenuOrigins @this, Transform rayOrigin)
		{
			return @this.provider.GetCustomAlternateMenuOrigin(rayOrigin);
		}
	}
}
#endif
