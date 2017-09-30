#if UNITY_EDITOR && UNITY_EDITORVR
using System;
using System.Linq;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR.Core
{
	partial class EditorVR
	{
		class ToolsMenu : Nested, IInterfaceConnector, IToolsMenuProvider, IPreviewInToolsMenuButtonProvider
		{
			public void PreviewInToolMenuButton(Transform rayOrigin, Type toolType, string toolDescription)
			{
				// Prevents menu buttons of types other than ITool from triggering any ToolMenuButton preview actions
				if (!toolType.GetInterfaces().Contains(typeof(ITool)))
					return;

				Rays.ForEachProxyDevice((deviceData) =>
				{
					if (deviceData.rayOrigin == rayOrigin) // Enable Tools Menu preview on the opposite (handed) device
					{
						var previewToolMenuButton = deviceData.toolsMenu.PreviewToolsMenuButton;
						previewToolMenuButton.previewToolType = toolType;
						previewToolMenuButton.previewToolDescription = toolDescription;
					}
				});
			}

			public void ClearToolMenuButtonPreview()
			{
				Rays.ForEachProxyDevice(deviceData =>
				{
					deviceData.toolsMenu.PreviewToolsMenuButton.previewToolType = null;
				});
			}

			public void SelectTool(Transform rayOrigin, Type toolType)
			{
				if (toolType == typeof(IMainMenu))
					MainMenuActivatorSelected(rayOrigin);
				else
					evr.GetNestedModule<Tools>().SelectTool(rayOrigin, toolType);
			}

			public void MainMenuActivatorSelected(Transform rayOrigin)
			{
				var targetToolRayOrigin = evr.m_DeviceData.FirstOrDefault(data => data.rayOrigin != rayOrigin).rayOrigin;
				Menus.OnMainMenuActivatorSelected(rayOrigin, targetToolRayOrigin);
			}

			public void ConnectInterface(object @object, object userData = null)
			{
				var toolsMenu = @object as IInjectedFunctionality<IToolsMenuProvider>;
				if (toolsMenu != null)
					toolsMenu.provider = this;

				var previewInToolsMenuButton = @object as IPreviewInToolsMenuButton;
				if (previewInToolsMenuButton != null)
					previewInToolsMenuButton.provider = this;
			}

			public void DisconnectInterface(object @object, object userData = null)
			{
			}
		}
	}
}
#endif
