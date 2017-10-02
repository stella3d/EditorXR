#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Provides functionality that allows all UI interaction to be negated for a given rayOrigin
	/// </summary>
	public interface IBlockUIInteraction : IInjectedFunctionality<IBlockUIInteractionProvider>
	{
	}

	public interface IBlockUIInteractionProvider
	{
		/// <summary>
		/// Prevent UI interaction for a given rayOrigin
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that is being checked</param>
		/// <param name="blocked">If true, UI interaction will be blocked for the rayOrigin.  If false, the ray origin will be removed from the blocked collection.</param>
		void SetUIBlockedForRayOrigin(Transform rayOrigin, bool blocked);
	}

	public static class IBlockUIInteractionMethods
	{
		/// <summary>
		/// Prevent UI interaction for a given rayOrigin
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that is being checked</param>
		/// <param name="blocked">If true, UI interaction will be blocked for the rayOrigin.  If false, the ray origin will be removed from the blocked collection.</param>
		public static void SetUIBlockedForRayOrigin(this IBlockUIInteraction @this, Transform rayOrigin, bool blocked)
		{
			@this.provider.SetUIBlockedForRayOrigin(rayOrigin, blocked);
		}
	}
}
#endif
