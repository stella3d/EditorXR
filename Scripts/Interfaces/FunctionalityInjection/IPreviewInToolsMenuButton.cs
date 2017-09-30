#if UNITY_EDITOR
using System;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Gives decorated class the ability to preview tools in a ToolButton
	/// </summary>
	public interface IPreviewInToolsMenuButton : IInjectedFunctionality<IPreviewInToolsMenuButtonProvider>
	{
	}

	public interface IPreviewInToolsMenuButtonProvider
	{
		/// <summary>
		/// Highlights a ToolMenuButton when a menu button is highlighted
		/// <param name="rayOrigin">Transform: Ray origin to check</param>
		/// <param name="toolType">Type: MenuButton's tool type to preview</param>
		/// <param name="toolDescription">String: The tool description to display as a Tooltip</param>
		/// </summary>
		void PreviewInToolMenuButton(Transform rayOrigin, Type toolType, string toolDescription);

		/// <summary>
		/// Clears any ToolMenuButton previews that are set
		/// </summary>
		void ClearToolMenuButtonPreview();
	}

	public static class IPreviewInToolMenuButtonMethods
	{
		/// <summary>
		/// Highlights a ToolMenuButton when a menu button is highlighted
		/// <param name="rayOrigin">Transform: Ray origin to check</param>
		/// <param name="toolType">Type: MenuButton's tool type to preview</param>
		/// <param name="toolDescription">String: The tool description to display as a Tooltip</param>
		/// </summary>
		public static void PreviewInToolMenuButton (this IPreviewInToolsMenuButton @this, Transform rayOrigin, Type toolType, string toolDescription)
		{
			@this.provider.PreviewInToolMenuButton(rayOrigin, toolType, toolDescription);
		}

		/// <summary>
		/// Clears any ToolMenuButton previews that are set
		/// </summary>
		public static void ClearToolMenuButtonPreview (this IPreviewInToolsMenuButton obj)
		{
			obj.provider.ClearToolMenuButtonPreview();
		}
	}
}
#endif
