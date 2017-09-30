#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Gives decorated class the ability to place objects in the scene, or a MiniWorld
	/// </summary>
	public interface IPlaceSceneObject : IInjectedFunctionality<IPlaceSceneObjectProvider>
	{
	}

	public interface IPlaceSceneObjectProvider
	{
		/// <summary>
		/// Method used to place objects in the scene/MiniWorld
		/// </summary>
		/// <param name="transform">Transform of the GameObject to place</param>
		/// <param name="scale">Target scale of placed object</param>
		void PlaceSceneObject(Transform transform, Vector3 scale);
	}

	public static class IPlaceSceneObjectMethods
	{
		/// <summary>
		/// Method used to place objects in the scene/MiniWorld
		/// </summary>
		/// <param name="transform">Transform of the GameObject to place</param>
		/// <param name="scale">Target scale of placed object</param>
		public static void PlaceSceneObject(this IPlaceSceneObject @this, Transform transform, Vector3 scale)
		{
			@this.provider.PlaceSceneObject(transform, scale);
		}
	}
}
#endif
