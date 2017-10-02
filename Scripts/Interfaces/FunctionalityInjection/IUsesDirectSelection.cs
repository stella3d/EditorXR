#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Gives decorated class access to direct selections
	/// </summary>
	public interface IUsesDirectSelection : IInjectedFunctionality<IUsesDirectSelectionProvider>
	{
		/// <summary>
		/// Called by the system whenever any implementor calls ResetDirectSelectionState
		/// </summary>
		void OnResetDirectSelectionState();
	}

	public interface IUsesDirectSelectionProvider
	{
		/// <summary>
		/// Returns a dictionary of direct selections
		/// </summary>
		/// <returns>Dictionary (K,V) where K = rayOrigin used to select the object and V = info about the direct selection</returns>
		Dictionary<Transform, GameObject> GetDirectSelection();

		/// <summary>
		/// Calls OnResetDirectSelectionState on all implementors of IUsesDirectSelection
		/// </summary>
		void ResetDirectSelectionState();
	}

	public static class IUsesDirectSelectionMethods
	{
		/// <summary>
		/// Returns a dictionary of direct selections
		/// </summary>
		/// <returns>Dictionary (K,V) where K = rayOrigin used to select the object and V = info about the direct selection</returns>
		public static Dictionary<Transform, GameObject> GetDirectSelection(this IUsesDirectSelection @this)
		{
			return @this.provider.GetDirectSelection();
		}

		/// <summary>
		/// Calls OnResetDirectSelectionState on all implementors of IUsesDirectSelection
		/// </summary>
		public static void ResetDirectSelectionState(this IUsesDirectSelection @this)
		{
			@this.provider.ResetDirectSelectionState();
		}
	}
}
#endif
