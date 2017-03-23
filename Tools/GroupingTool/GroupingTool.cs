#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputNew;

namespace UnityEditor.Experimental.EditorVR.Tools
{
	sealed class GroupingTool : MonoBehaviour, ITool, IStandardActionMap, IUsesRayOrigin, IActions, ISetHighlight, 
		IUsesRaycastResults, ILinkedObject
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

		enum GroupingMode
		{
			Disabled,
			Enabled,
			Activated
		}

		[SerializeField]
		Sprite m_GroupingDisabledIcon;

		[SerializeField]
		Sprite m_GroupingEnabledIcon;

		[SerializeField]
		Sprite m_GroupingActivatedIcon;

		[SerializeField]
		Material m_GroupingHighlightMaterial;

		List<IAction> m_Actions;
		readonly GroupingAction m_GroupingAction = new GroupingAction();

		GroupingMode m_GroupingMode;

		readonly List<GameObject> m_Group = new List<GameObject>();

		ButtonInputControl m_SelectAction;

		GroupingMode groupingMode
		{
			get { return m_GroupingMode; }
			set
			{
				if (m_GroupingMode == value)
					return;

				SetGroupingMode(value);
			}
		}

		public List<IAction> actions
		{
			get
			{
				if (!isSharedUpdater(this))
					return null;

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
		public SetHighlightDelegate setHighlight { get; set; }
		public Func<Transform, GameObject> getFirstGameObject { get; set; }
		public List<ILinkedObject> linkedObjects { get; set; }
		public Func<ILinkedObject, bool> isSharedUpdater { get; set; }

		void Awake()
		{
			SetGroupingMode(GroupingMode.Disabled);
		}

		public void ProcessInput(ActionMapInput input, ConsumeControlDelegate consumeControl)
		{
			var standardInput = (Standard)input;
			m_SelectAction = standardInput.action;

			if (isSharedUpdater(this))
			{
				if (groupingMode == GroupingMode.Activated)
				{
					for (int i = 0; i < linkedObjects.Count; i++)
					{
						var groupingTool = ((GroupingTool)linkedObjects[i]);
						var selectAction = groupingTool.m_SelectAction;
						if (selectAction != null && selectAction.wasJustPressed)
						{
							var hoveredObject = getFirstGameObject(groupingTool.rayOrigin);

							if (hoveredObject)
							{
								if (m_Group.Remove(hoveredObject))
									setHighlight(hoveredObject, false, material: m_GroupingHighlightMaterial);
								else
									m_Group.Add(hoveredObject);

								consumeControl(selectAction);
							}
						}
					}

					for (int i = 0; i < m_Group.Count; i++)
						setHighlight(m_Group[i], true, material: m_GroupingHighlightMaterial);
				}
				else
				{
					groupingMode = Selection.gameObjects.Length > 1 ? GroupingMode.Enabled : GroupingMode.Disabled;
				}
			}
		}

		void SetGroupingMode(GroupingMode mode)
		{
			m_GroupingMode = mode;
			switch (m_GroupingMode)
			{
				case GroupingMode.Enabled:
					m_GroupingAction.icon = m_GroupingEnabledIcon;
					m_GroupingAction.tooltipText = "Enter grouping mode";
					m_GroupingAction.execute = EnterGroupingMode;
					break;

				case GroupingMode.Disabled:
					m_GroupingAction.icon = m_GroupingDisabledIcon;
					m_GroupingAction.tooltipText = "Select two objects to enable grouping";
					m_GroupingAction.execute = null;
					break;

				case GroupingMode.Activated:
					m_GroupingAction.icon = m_GroupingActivatedIcon;
					m_GroupingAction.tooltipText = "Exit grouping mode";
					m_GroupingAction.execute = ExitGroupingMode;
					break;
			}
		}

		void EnterGroupingMode()
		{
			groupingMode = GroupingMode.Activated;
			m_Group.Clear();
			m_Group.AddRange(Selection.gameObjects);
		}

		void ExitGroupingMode()
		{
			groupingMode = GroupingMode.Disabled;
		}
	}
}
#endif
