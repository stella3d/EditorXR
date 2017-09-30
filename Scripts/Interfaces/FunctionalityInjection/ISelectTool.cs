#if UNITY_EDITOR
using System;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Gives decorated class the ability to select tools from a menu
	/// </summary>
	public interface ISelectTool : IInjectedFunctionality<ISelectToolProvider>
	{
	}

	public interface ISelectToolProvider
	{
		/// <summary>
		/// Method used to select tools from the menu
		/// Returns whether the tool was successfully selected
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that the tool should spawn under</param>
		/// <param name="toolType">Type of tool to spawn/select</param>
		/// <param name="despawnOnReselect">Despawn the tool, if re-selected while already the current tool</param>
		bool SelectTool(Transform rayOrigin, Type toolType, bool despawnOnReselect = true);

		/// <summary>
		/// Returns true if the active tool on the given ray origin is of the given type
		/// </summary>
		/// <param name="rayOrigin">The ray origin to check</param>
		/// <param name="type">The tool type to compare</param>
		bool IsToolActive(Transform rayOrigin, Type type);
	}

	public static class ISelectToolMethods
	{
		/// <summary>
		/// Method used to select tools from the menu
		/// Returns whether the tool was successfully selected
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin that the tool should spawn under</param>
		/// <param name="toolType">Type of tool to spawn/select</param>
		/// <param name="despawnOnReselect">Despawn the tool, if re-selected while already the current tool</param>
		public static bool SelectTool(this ISelectTool @this, Transform rayOrigin, Type toolType, bool despawnOnReselect = true)
		{
			return @this.provider.SelectTool(rayOrigin, toolType, despawnOnReselect);
		}

		/// <summary>
		/// Returns true if the active tool on the given ray origin is of the given type
		/// </summary>
		/// <param name="rayOrigin">The ray origin to check</param>
		/// <param name="type">The tool type to compare</param>
		public static bool IsToolActive(this ISelectTool @this, Transform rayOrigin, Type type)
		{
			return @this.provider.IsToolActive(rayOrigin, type);
		}
	}
}
#endif
