#if UNITY_EDITOR
using System;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Provides access to checks that can test whether a ray is hovering over a UI element
	/// </summary>
	public interface IIsHoveringOverUI : IInjectedFunctionality<IIsHoveringOverUIProvider>
	{
	}

	public interface IIsHoveringOverUIProvider
	{
		/// <summary>
		/// Returns whether the specified ray origin is hovering over a UI element
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that is being checked</param>
		bool IsHoveringOverUI(Transform rayOrigin);
	}

	public static class IIsHoveringOverUIMethods
	{
		/// <summary>
		/// Returns whether the specified ray origin is hovering over a UI element
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that is being checked</param>
		public static bool IsHoveringOverUI(this IIsHoveringOverUI @this, Transform rayOrigin)
		{
			return @this.provider.IsHoveringOverUI(rayOrigin);
		}
	}
}
#endif
