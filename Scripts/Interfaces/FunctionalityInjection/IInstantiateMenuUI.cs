#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Provides custom menu instantiation
	/// </summary>
	public interface IInstantiateMenuUI : IInjectedFunctionality<IInstantiateMenuUIProvider>
	{
	}

	public interface IInstantiateMenuUIProvider
	{
		/// <summary>
		/// Instantiate custom menu UI on a proxy
		/// </summary>
		/// <param name="rayOrigin">The ray origin of the proxy that this menu is being instantiated from</param>
		/// <param name="menuPrefab">The prefab (with an IMenu component) to instantiate</param>
		GameObject InstantiateMenuUI(Transform rayOrigin, IMenu menuPrefab);
	}

	public static class IInstantiateMenuUIMethods
	{
		/// <summary>
		/// Instantiate custom menu UI on a proxy
		/// </summary>
		/// <param name="rayOrigin">The ray origin of the proxy that this menu is being instantiated from</param>
		/// <param name="menuPrefab">The prefab (with an IMenu component) to instantiate</param>
		public static GameObject InstantiateMenuUI(this IInstantiateMenuUI @this, Transform rayOrigin, IMenu menuPrefab)
		{
			return @this.provider.InstantiateMenuUI(rayOrigin, menuPrefab);
		}
	}
}
#endif
