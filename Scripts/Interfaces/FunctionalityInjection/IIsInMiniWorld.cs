#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Provides access to checks that can test whether a rayOrigin is contained in a miniworld
	/// </summary>
	public interface IIsInMiniWorld : IInjectedFunctionality<IIsInMiniWorldProvider>
	{
	}

	public interface IIsInMiniWorldProvider
	{
		/// <summary>
		/// Returns whether the specified ray is contained in a miniworld
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that is being checked</param>
		bool IsInMiniWorld(Transform rayOrigin);
	}

	public static class IIsInMiniWorldMethods
	{
		/// <summary>
		/// Returns whether the specified ray is contained in a miniworld
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that is being checked</param>
		public static bool IsInMiniWorld(this IIsInMiniWorld @this, Transform rayOrigin)
		{
			return @this.provider.IsInMiniWorld(rayOrigin);
		}
	}
}
#endif
