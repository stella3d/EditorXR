#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Gives decorated class access to hover/intersection detection
	/// </summary>
	public interface IUsesRaycastResults : IInjectedFunctionality<IUsesRaycastResultsProvider>
	{
	}

	public interface IUsesRaycastResultsProvider
	{
		/// <summary>
		/// Method used to test hover/intersection
		/// Returns the first GameObject being hovered over, or intersected with
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin for intersection purposes</param>
		GameObject GetFirstGameObject(Transform rayOrigin);
	}

	public static class IUsesRaycastResultsMethods
	{
		/// <summary>
		/// Method used to test hover/intersection
		/// Returns the first GameObject being hovered over, or intersected with
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin for intersection purposes</param>
		public static GameObject GetFirstGameObject(this IUsesRaycastResults @this, Transform rayOrigin)
		{
			return @this.provider.GetFirstGameObject(rayOrigin);
		}
	}
}
#endif
