#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Implementors can get the color of the default ray
	/// </summary>
	public interface IGetDefaultRayColor : IInjectedFunctionality<IGetDefaultRayColorProvider>
	{
	}

	public interface IGetDefaultRayColorProvider
	{
		/// <summary>
		/// Get the color of the default ray
		/// <param name="rayOrigin">The ray on which to get the color</param>
		/// </summary>
		Color GetDefaultRayColor(Transform rayOrigin);
	}

	public static class IGetDefaultRayColorMethods
	{
		/// <summary>
		/// Get the color of the default ray
		/// <param name="rayOrigin">The ray on which to get the color</param>
		/// </summary>
		/// <param name="rayOrigin">The rayorigin of which to get the color</param>
		public static Color GetDefaultRayColor(this IGetDefaultRayColor @this, Transform rayOrigin)
		{
			return @this.provider.GetDefaultRayColor(rayOrigin);
		}
	}
}
#endif
