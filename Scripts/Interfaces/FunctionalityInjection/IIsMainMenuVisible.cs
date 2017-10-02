#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Provides access to checks that can test whether the main menu is visible on a given ray origin
	/// </summary>
	public interface IIsMainMenuVisible : IInjectedFunctionality<IIsMainMenuVisibleProvider>
	{
	}

	public interface IIsMainMenuVisibleProvider
	{
		/// <summary>
		/// Returns whether the main menu is visible on the specified rayOrigin
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that is being checked</param>
		bool IsMainMenuVisible(Transform rayOrigin);
	}

	public static class IIsMainMenuVisibleMethods
	{
		/// <summary>
		/// Returns whether the main menu is visible on the specified rayOrigin
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that is being checked</param>
		public static bool IsMainMenuVisible(this IIsMainMenuVisible @this, Transform rayOrigin)
		{
			return @this.provider.IsMainMenuVisible(rayOrigin);
		}
	}
}
#endif
