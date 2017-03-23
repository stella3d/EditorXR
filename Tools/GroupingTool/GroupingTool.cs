#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputNew;

namespace UnityEditor.Experimental.EditorVR.Tools
{
	sealed class GroupingTool : MonoBehaviour, ITool, IStandardActionMap, IUsesRayOrigin, IActions
	{
		class GroupingAction : IAction, ITooltip
		{
			internal Action execute;
			public string tooltipText { get; internal set; }
			public Sprite icon { get; internal set; }

			public void ExecuteAction()
			{
				if (execute != null)
					execute();
			}
		}

		[SerializeField]
		Sprite m_GroupingEnabledIcon;

		[SerializeField]
		Sprite m_GroupingDisabledIcon;

		List<IAction> m_Actions;

		readonly GroupingAction m_GroupingAction = new GroupingAction();

		bool m_GroupingEnabled;

		bool groupingEnabled
		{
			get { return m_GroupingEnabled; }
			set
			{
				if (m_GroupingEnabled == value)
					return;

				m_GroupingEnabled = value;
				if (m_GroupingEnabled)
				{
					m_GroupingAction.icon = m_GroupingEnabledIcon;
					m_GroupingAction.tooltipText = "Select to enter grouping mode";
					m_GroupingAction.execute = EnterGroupingMode;
				}
				else
				{
					m_GroupingAction.icon = m_GroupingDisabledIcon;
					m_GroupingAction.tooltipText = "Select two objects to enable grouping";
					m_GroupingAction.execute = null;
				}

			}
		}

		public List<IAction> actions
		{
			get
			{
				if (m_Actions == null)
				{
					m_Actions = new List<IAction>()
					{
						m_GroupingAction
					};
				}
				return m_Actions;
			}
		}

		public Transform rayOrigin { get; set; }

		void Awake()
		{
			groupingEnabled = false;
		}

		public void ProcessInput(ActionMapInput input, ConsumeControlDelegate consumeControl)
		{
			var standardInput = (Standard)input;
			if (standardInput.action.wasJustPressed)
			{
			}

			groupingEnabled = Selection.gameObjects.Length > 1;
		}

		void EnterGroupingMode()
		{
			
		}
	}
}
#endif
