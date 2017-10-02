#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Gives decorated class ability to highlight a given GameObject
	/// </summary>
	public interface ISetHighlight : IInjectedFunctionality<ISetHighlightProvider>
	{
	}

	public interface ISetHighlightProvider
	{
		/// <summary>
		/// Method for highlighting objects
		/// </summary>
		/// <param name="go">The object to highlight</param>
		/// <param name="active">Whether to add or remove the highlight</param>
		/// <param name="rayOrigin">RayOrigin that hovered over the object (optional)</param>
		/// <param name="material">Custom material to use for this object</param>
		/// <param name="force">Force the setting or unsetting of the highlight</param>
		/// <param name="duration">The duration for which to show this highlight. Keep default value of 0 to show until explicitly hidden</param>
		void SetHighlight(GameObject go, bool active, Transform rayOrigin = null, Material material = null, bool force = false, float duration = 0f);
	}

	public static class ISetHighlightMethods
	{
		/// <summary>
		/// Method for highlighting objects
		/// </summary>
		/// <param name="go">The object to highlight</param>
		/// <param name="active">Whether to add or remove the highlight</param>
		/// <param name="rayOrigin">RayOrigin that hovered over the object (optional)</param>
		/// <param name="material">Custom material to use for this object</param>
		/// <param name="force">Force the setting or unsetting of the highlight</param>
		/// <param name="duration">The duration for which to show this highlight. Keep default value of 0 to show until explicitly hidden</param>
		public static void SetHighlight(this ISetHighlight @this, GameObject go, bool active, Transform rayOrigin = null, Material material = null, bool force = false, float duration = 0f)
		{
			@this.provider.SetHighlight(go, active, rayOrigin, material, force, duration);
		}
	}
}
#endif
