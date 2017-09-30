#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR.Core
{
	/// <summary>
	/// Provide the ability to request a corresponding node for a ray origin
	/// </summary>
	interface IRayToNode : IInjectedFunctionality<IRayToNodeProvider>
	{
	}

	interface IRayToNodeProvider
	{
		/// <summary>
		/// Get the corresponding node for a given ray origin
		/// </summary>
		/// <param name="rayOrigin">The ray origin to request a node for</param>
		Node RequestNodeFromRayOrigin(Transform rayOrigin);
	}

	static class IRayToNodeMethods
	{
		/// <summary>
		/// Get the corresponding node for a given ray origin
		/// </summary>
		/// <param name="rayOrigin">The ray origin to request a node for</param>
		internal static Node RequestNodeFromRayOrigin(this IRayToNode @this, Transform rayOrigin)
		{
			return @this.provider.RequestNodeFromRayOrigin(rayOrigin);
		}
	}
}
#endif
