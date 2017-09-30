#if UNITY_EDITOR
using System;
using UnityEditor.Experimental.EditorVR.Modules;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Gives decorated class ability to control/perform spatial-scrolling
	/// 
	/// Spatially scrolling allows for directional input-device movement to drive changes/progression of UI
	/// element selection, without the need for additional input beyond the movement of an input-device.
	/// </summary>
	public interface IControlSpatialScrolling : IInjectedFunctionality<IControlSpatialScrollingProvider>
	{
		/// <summary>
		/// The data defining a spatial scroll operation
		/// </summary>
		SpatialScrollModule.SpatialScrollData spatialScrollData { get; set; }
	}

	public interface IControlSpatialScrollingProvider
	{
		/// <summary>
		/// Perform a spatial scroll action
		/// </summary>
		/// <param name="caller">The caller object</param>
		/// <param name="node">The node on which to display & perform the spatial scroll</param>
		/// <param name="startingPosition">The initial position of the spatial scroll</param>
		/// <param name="currentPosition">The current/updated position of the spatial scroll</param>
		/// <param name="repeatingScrollLengthRange">The length at which a scroll action will return a repeating/looping value</param>
		/// <param name="scrollableItemCount">The number of items being scrolled through with this action</param>
		/// <param name="maxItemCount">The maximum number of items that can be scrolled through for this action</param>
		/// <param name="centerVisuals">If true, expand the scroll line visuals outward in both directions from the scroll start position</param>
		/// <returns>The spatial scroll data for a single scroll action, but an individual caller object</returns>
		SpatialScrollModule.SpatialScrollData PerformSpatialScroll(IControlSpatialScrolling caller, Node node,
			Vector3 startingPosition, Vector3 currentPosition, float repeatingScrollLengthRange,
			int scrollableItemCount, int maxItemCount = -1, bool centerVisuals = true);

		/// <summary>
		/// End a spatial scrolling action for a given caller
		/// </summary>
		/// <param name="caller">The caller object</param>
		void EndSpatialScroll(IControlSpatialScrolling caller);
	}

	public static class IControlSpatialScrollingMethods
	{
		/// <summary>
		/// Perform a spatial scroll action
		/// </summary>
		/// <param name="node">The node on which to display & perform the spatial scroll</param>
		/// <param name="startingPosition">The initial position of the spatial scroll</param>
		/// <param name="currentPosition">The current/updated position of the spatial scroll</param>
		/// <param name="repeatingScrollLengthRange">The length at which a scroll action will return a repeating/looping value</param>
		/// <param name="scrollableItemCount">The number of items being scrolled through with this action</param>
		/// <param name="maxItemCount">The maximum number of items that can be scrolled through for this action</param>
		/// <param name="centerVisuals">If true, expand the scroll line visuals outward in both directions from the scroll start position</param>
		/// <returns>The spatial scroll data for a single scroll action, but an individual caller object</returns>
		public static SpatialScrollModule.SpatialScrollData PerformSpatialScroll(this IControlSpatialScrolling @this,
			Node node, Vector3 startingPosition, Vector3 currentPosition, float repeatingScrollLengthRange,
			int scrollableItemCount, int maxItemCount = -1, bool centerVisuals = true)
		{
			return @this.provider.PerformSpatialScroll(@this, node, startingPosition, currentPosition, repeatingScrollLengthRange, scrollableItemCount, maxItemCount, centerVisuals);
		}

		/// <summary>
		/// End a spatial scrolling action for a given caller
		/// </summary>
		public static void EndSpatialScroll(this IControlSpatialScrolling @this)
		{
			@this.provider.EndSpatialScroll(@this);
		}
	}
}
#endif
