#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Gives access to the selection module
	/// </summary>
	public interface ISelectObject : IInjectedFunctionality<ISelectObjectProvider>
	{
	}

	public interface ISelectObjectProvider
	{
		/// <summary>
		/// Given a hovered object, find what object would actually be selected
		/// </summary>
		/// <param name="hoveredObject">The hovered object that is being tested for selection</param>
		/// <param name="useGrouping">Use group selection</param>
		/// <returns>Returns what object would be selected by selectObject</returns>
		GameObject GetSelectionCandidate(GameObject hoveredObject, bool useGrouping = false);

		/// <summary>
		/// Select the given object using the given rayOrigin
		/// </summary>
		/// <param name="hoveredObject">The hovered object</param>
		/// <param name="rayOrigin">The rayOrigin used for selection</param>
		/// <param name="multiSelect">Whether to add the hovered object to the selection, or override the current selection</param>
		/// <param name="useGrouping">Use group selection</param>
		void SelectObject(GameObject hoveredObject, Transform rayOrigin, bool multiSelect, bool useGrouping = false);
	}

	public static class ISelectObjectMethods
	{
		/// <summary>
		/// Given a hovered object, find what object would actually be selected
		/// </summary>
		/// <param name="hoveredObject">The hovered object that is being tested for selection</param>
		/// <param name="useGrouping">Use group selection</param>
		/// <returns>Returns what object would be selected by selectObject</returns>
		public static GameObject GetSelectionCandidate(this ISelectObject @this, GameObject hoveredObject, bool useGrouping = false)
		{
			return @this.provider.GetSelectionCandidate(hoveredObject, useGrouping);
		}

		/// <summary>
		/// Select the given object using the given rayOrigin
		/// </summary>
		/// <param name="hoveredObject">The hovered object</param>
		/// <param name="rayOrigin">The rayOrigin used for selection</param>
		/// <param name="multiSelect">Whether to add the hovered object to the selection, or override the current selection</param>
		/// <param name="useGrouping">Use group selection</param>
		public static void SelectObject(this ISelectObject @this, GameObject hoveredObject, Transform rayOrigin, bool multiSelect, bool useGrouping = false)
		{
			@this.provider.SelectObject(hoveredObject, rayOrigin, multiSelect, useGrouping);
		}
	}
}
#endif
