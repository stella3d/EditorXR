#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Implementors can request changes to visibility on parts of the default ray
	/// </summary>
	public interface IRayVisibilitySettings : IInjectedFunctionality<IRayVisibilitySettingsProvider>
	{
	}

	public interface IRayVisibilitySettingsProvider
	{
		/// <summary>
		/// Add visibility settings to try and show/hide the ray/cone
		/// </summary>
		/// <param name="rayOrigin">The ray to hide or show</param>
		/// <param name="caller">The object which is adding settings</param>
		/// <param name="rayVisible">Show or hide the ray</param>
		/// <param name="coneVisible">Show or hide the cone</param>
		/// <param name="priority">(Optional) The priority level of this request</param>
		void AddRayVisibilitySettings(Transform rayOrigin, object caller, bool rayVisible, bool coneVisible, int priority = 0);

		/// <summary>
		/// Remove visibility settings
		/// </summary>
		/// <param name="rayOrigin">The ray from which to remove settings</param>
		/// <param name="caller">The object whose settings to remove</param>
		void RemoveRayVisibilitySettings(Transform rayOrigin, object caller);
	}

	public static class IRayVisibilitySettingsMethods
	{
		/// <summary>
		/// Add visibility settings to try and show/hide the ray/cone
		/// </summary>
		/// <param name="rayOrigin">The ray to hide or show</param>
		/// <param name="caller">The object which is adding settings</param>
		/// <param name="rayVisible">Show or hide the ray</param>
		/// <param name="coneVisible">Show or hide the cone</param>
		/// <param name="priority">(Optional) The priority level of this request</param>
		public static void AddRayVisibilitySettings(this IRayVisibilitySettings @this, Transform rayOrigin,
			object caller, bool rayVisible, bool coneVisible, int priority = 0)
		{
			@this.provider.AddRayVisibilitySettings(rayOrigin, caller, rayVisible, coneVisible, priority);
		}

		/// <summary>
		/// Remove visibility settings
		/// </summary>
		/// <param name="rayOrigin">The ray from which to remove settings</param>
		/// <param name="caller">The object whose settings to remove</param>
		public static void RemoveRayVisibilitySettings(this IRayVisibilitySettings @this, Transform rayOrigin,
			object caller)
		{
			@this.provider.RemoveRayVisibilitySettings(rayOrigin, caller);
		}
	}
}
#endif
