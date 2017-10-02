#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Implementors can set the color of the default ray
	/// </summary>
	public interface ISetDefaultRayColor : IInjectedFunctionality<ISetDefaultRayColorProvider>
	{
	}

	public interface ISetDefaultRayColorProvider
	{
		/// <summary>
		/// Set the color of the default ray
		/// </summary>
		/// <param name="rayOrigin">The ray on which to set the color</param>
		/// <param name="color">The color to set on the default ray</param>
		void SetDefaultRayColor(Transform rayOrigin, Color color);
	}

	public static class ISetDefaultRayColorMethods
	{
		/// <summary>
		/// Set the color of the default ray
		/// </summary>
		/// <param name="rayOrigin">The ray on which to set the color</param>
		/// <param name="color">The color to set on the default ray</param>
		public static void SetDefaultRayColor(this ISetDefaultRayColor @this, Transform rayOrigin, Color color)
		{
			@this.provider.SetDefaultRayColor(rayOrigin, color);
		}
	}
}
#endif
