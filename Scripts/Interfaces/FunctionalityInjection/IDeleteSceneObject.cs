#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Decorates objects which can delete objects from the scene
	/// </summary>
	public interface IDeleteSceneObject : IInjectedFunctionality<IDeleteSceneObjectProvider>
	{
	}

	public interface IDeleteSceneObjectProvider
	{
		/// <summary>
		/// Remove the game object from the scene
		/// </summary>
		/// <param name="go">The game object to delete from the scene</param>
		void DeleteSceneObject(GameObject go);
	}

	public static class IDeleteSceneObjectMethods
	{
		/// <summary>
		/// Remove the game object from the scene
		/// </summary>
		/// <param name="go">The game object to delete from the scene</param>
		public static void DeleteSceneObject(this IDeleteSceneObject @this, GameObject go)
		{
			@this.provider.DeleteSceneObject(go);
		}
	}
}
#endif
