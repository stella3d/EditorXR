#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Gives decorated class access to IntersectionModule.Raycast
	/// </summary>
	public interface IRaycast : IInjectedFunctionality<IRaycastProvider>
	{
	}

	public interface IRaycastProvider
	{
		/// <summary>
		/// Do a raycast against all Renderers
		/// </summary>
		/// <param name="ray">The ray to use for the raycast</param>
		/// <param name="hit">Hit information</param>
		/// <param name="go">The gameobject which was hit, if any</param>
		/// <param name="maxDistance">The maximum distance of the raycast</param>
		/// <param name="ignoreList">(optional) A list of Renderers to ignore</param>
		/// <returns></returns>
		bool Raycast(Ray ray, out RaycastHit hit, out GameObject go, float maxDistance = Mathf.Infinity, List<Renderer> ignoreList = null);
	}

	public static class IRaycastMethods
	{
		public delegate bool RaycastDelegate(Ray ray, out RaycastHit hit, out GameObject go, float maxDistance = Mathf.Infinity, List<Renderer> ignoreList = null);

		public static RaycastDelegate raycast { get; set; }

		/// <summary>
		/// Do a raycast against all Renderers
		/// </summary>
		/// <param name="ray">The ray to use for the raycast</param>
		/// <param name="hit">Hit information</param>
		/// <param name="go">The gameobject which was hit, if any</param>
		/// <param name="maxDistance">The maximum distance of the raycast</param>
		/// <param name="ignoreList">(optional) A list of Renderers to ignore</param>
		/// <returns></returns>
		public static bool Raycast(this IRaycast @this, Ray ray, out RaycastHit hit, out GameObject go, float maxDistance = Mathf.Infinity, List<Renderer> ignoreList = null)
		{
			return @this.provider.Raycast(ray, out hit, out go, maxDistance, ignoreList);
		}
	}
}
#endif
