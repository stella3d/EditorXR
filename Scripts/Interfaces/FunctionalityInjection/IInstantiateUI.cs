#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Decorates types that need to connect interfaces for spawned objects
	/// </summary>
	public interface IInstantiateUI : IInjectedFunctionality<IInstantiateUIProvider>
	{
	}

	public interface IInstantiateUIProvider
	{
		/// <summary>
		/// Method provided by the system for instantiating UI
		/// </summary>
		/// <param name="prefab">The prefab to instantiate</param>
		/// <param name="parent">(Optional) A parent transform to instantiate under</param>
		/// <param name="worldPositionStays">(Optional) If true, the parent-relative position, scale and rotation are modified</param>
		/// <param name="rayOrigin">(Optional) RayOrigin override that will be used when connecting interfaces on this object
		/// such that the object keeps the same world space position, rotation and scale as before.</param>
		/// <returns>The instantiated gameobject</returns>
		GameObject InstantiateUI( GameObject prefab, Transform parent = null, bool worldPositionStays = true, Transform rayOrigin = null);
	}

	public static class IInstantiateUIMethods
	{
		/// <summary>
		/// Method provided by the system for instantiating UI
		/// </summary>
		/// <param name="prefab">The prefab to instantiate</param>
		/// <param name="parent">(Optional) A parent transform to instantiate under</param>
		/// <param name="worldPositionStays">(Optional) If true, the parent-relative position, scale and rotation are modified</param>
		/// <param name="rayOrigin">(Optional) RayOrigin override that will be used when connecting interfaces on this object
		/// such that the object keeps the same world space position, rotation and scale as before.</param>
		/// <returns>The instantiated gameobject</returns>
		public static GameObject InstantiateUI(this IInstantiateUI @this, GameObject prefab, Transform parent = null, bool worldPositionStays = true, Transform rayOrigin = null)
		{
			return @this.provider.InstantiateUI(prefab, parent, worldPositionStays, rayOrigin);
		}
	}
}
#endif
