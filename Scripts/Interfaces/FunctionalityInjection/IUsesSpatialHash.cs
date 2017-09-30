#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Make use of the spatial hash
	/// </summary>
	public interface IUsesSpatialHash : IInjectedFunctionality<IUsesSpatialHashProvider>
	{
	}

	public interface IUsesSpatialHashProvider
	{
		/// <summary>
		/// Add all renderers of a GameObject (and its children) to the spatial hash for queries, direct selection, etc.
		/// </summary>
		/// <param name="go">The GameObject to add</param>
		void AddToSpatialHash(GameObject go);

		/// <summary>
		/// Remove all renderers of a GameObject (and its children) from the spatial hash
		/// </summary>
		/// <param name="go">The GameObject to remove</param>
		void RemoveFromSpatialHash(GameObject go);
	}

	public static class IUsesSpatialHashMethods
	{
		/// <summary>
		/// Add all renderers of a GameObject (and its children) to the spatial hash for queries, direct selection, etc.
		/// </summary>
		/// <param name="go">The GameObject to add</param>
		public static void AddToSpatialHash(this IUsesSpatialHash @this, GameObject go)
		{
			@this.provider.AddToSpatialHash(go);
		}

		/// <summary>
		/// Remove all renderers of a GameObject (and its children) from the spatial hash
		/// </summary>
		/// <param name="go">The GameObject to remove</param>
		public static void RemoveFromSpatialHash(this IUsesSpatialHash @this, GameObject go)
		{
			@this.provider.RemoveFromSpatialHash(go);
		}
	}
}
#endif
