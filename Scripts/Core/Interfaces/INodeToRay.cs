#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR.Core
{
	/// <summary>
	/// Provide the ability to request a corresponding ray origin for a node
	/// </summary>
	public interface INodeToRay : IInjectedFunctionality<INodeToRayProvider>
	{
	}

	public interface INodeToRayProvider
	{
		/// <summary>
		/// Get the corresponding ray origin for a given node
		/// </summary>
		/// <param name="node">The node to request a ray origin for</param>
		Transform RequestRayOriginFromNode(Node node);
	}

	static class INodeToRayMethods
	{
		/// <summary>
		/// Get the corresponding ray origin for a given node
		/// </summary>
		/// <param name="node">The node to request a ray origin for</param>
		internal static Transform RequestRayOriginFromNode(this INodeToRay @this, Node node)
		{
			return @this.provider.RequestRayOriginFromNode(node);
		}
	}
}
#endif
