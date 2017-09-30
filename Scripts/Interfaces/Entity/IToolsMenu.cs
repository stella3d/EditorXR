#if UNITY_EDITOR
using System;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Gives decorated class Tools Menu functionality
	/// </summary>
	public interface IToolsMenu : IUsesMenuOrigins, ICustomActionMap, IUsesNode, ISelectTool, IInjectedFunctionality<IToolsMenuProvider>
	{
		/// <summary>
		/// Bool denoting that the alternate menu (radial menu, etc) is currently visible
		/// Allows the ToolsMenu to adapt visibility state changes that occur  in the AlternateMenu
		/// </summary>
		bool alternateMenuVisible { set; }

		/// <summary>
		/// This menu's RayOrigin
		/// </summary>
		Transform rayOrigin { get; set; }

		/// <summary>
		/// The ToolsMenuButton that the menu uses to display tool previews
		/// </summary>
		IToolsMenuButton PreviewToolsMenuButton { get; }

		/// <summary>
		/// Function that assigns & sets up a tool button for a given tool type
		/// This method isn't hooked up in EVR, it should reside in the implementing class
		/// </summary>
		Action<Type, Sprite> setButtonForType { get; }

		/// <summary>
		/// Delete the tool button with corresponding type of the first parameter.
		/// Then, select the tool button with corresponds to the type of the second parameter.
		/// </summary>
		Action<Type, Type> deleteToolsMenuButton { get; }

		/// <summary>
		/// Set the interactable state on the main menu activator button
		/// </summary>
		bool mainMenuActivatorInteractable { set; }
	}

	public interface IToolsMenuProvider
	{
		/// <summary>
		/// Called when selecting the main menu activator
		/// </summary>
		/// <param name="rayOrigin">This menu's RayOrigin</param>
		void MainMenuActivatorSelected(Transform rayOrigin);

		/// <summary>
		/// Selects a tool, based on type, from a Tools Menu Button
		/// </summary>
		/// <param name="rayOrigin">This menu's RayOrigin</param>
		/// <param name="type">The type of the tool that is to be selected</param>
		void SelectTool(Transform rayOrigin, Type type);
	}

	public static class IToolsMenuMethods
	{
		/// <summary>
		/// Called when selecting the main menu activator
		/// </summary>
		/// <param name="rayOrigin">This menu's RayOrigin</param>
		public static void MainMenuActivatorSelected(this IInjectedFunctionality<IToolsMenuProvider> @this, Transform rayOrigin)
		{
			@this.provider.MainMenuActivatorSelected(rayOrigin);
		}

		/// <summary>
		/// Selects a tool, based on type, from a Tools Menu Button
		/// </summary>
		/// <param name="rayOrigin">This menu's RayOrigin</param>
		/// <param name="type">The type of the tool that is to be selected</param>
		public static void SelectTool(this IInjectedFunctionality<IToolsMenuProvider> @this, Transform rayOrigin, Type type)
		{
			@this.provider.SelectTool(rayOrigin, type);
		}
	}
}
#endif
