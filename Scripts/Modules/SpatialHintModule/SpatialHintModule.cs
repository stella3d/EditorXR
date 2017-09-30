#if UNITY_EDITOR
using UnityEditor.Experimental.EditorVR.Core;
using UnityEditor.Experimental.EditorVR.Menus;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR.Modules
{
	public sealed class SpatialHintModule : MonoBehaviour, IConnectInterfaces, IInstantiateUI, INodeToRay, IRayVisibilitySettings, IControlSpatialHintingProvider
	{
		public enum SpatialHintStateFlags
		{
			Hidden,
			PreDragReveal,
			Scrolling,
			CenteredScrolling,
		}

		[SerializeField]
		SpatialHintUI m_SpatialHintUI;

		SpatialHintStateFlags m_State;
		Node m_ControllingNode;

		public SpatialHintStateFlags state
		{
			get { return m_State; }
			set
			{
				m_State = value;
				switch (m_State)
				{
					case SpatialHintStateFlags.Hidden:
						m_SpatialHintUI.centeredScrolling = false;
						m_SpatialHintUI.preScrollArrowsVisible = false;
						m_SpatialHintUI.secondaryArrowsVisible = false;
						this.RemoveRayVisibilitySettings(this.RequestRayOriginFromNode(m_ControllingNode), this);
						controllingNode = Node.None;
						break;
					case SpatialHintStateFlags.PreDragReveal:
						m_SpatialHintUI.centeredScrolling = false;
						m_SpatialHintUI.preScrollArrowsVisible = true;
						m_SpatialHintUI.secondaryArrowsVisible = true;
						break;
					case SpatialHintStateFlags.Scrolling:
						m_SpatialHintUI.centeredScrolling = false;
						m_SpatialHintUI.preScrollArrowsVisible = false;
						m_SpatialHintUI.scrollVisualsVisible = true;
						break;
					case SpatialHintStateFlags.CenteredScrolling:
						m_SpatialHintUI.centeredScrolling = true;
						m_SpatialHintUI.preScrollArrowsVisible = false;
						m_SpatialHintUI.scrollVisualsVisible = true;
						break;
				}
			}
		}

		Node controllingNode
		{
			set
			{
				var controllingNode = m_SpatialHintUI.controllingNode;
				if (value == controllingNode)
					return;

				m_ControllingNode = value;
				if (m_ControllingNode != Node.None)
					state = SpatialHintStateFlags.PreDragReveal;

				m_SpatialHintUI.controllingNode = value;
			}
		}

		Vector3 spatialHintScrollVisualsRotation { set { m_SpatialHintUI.scrollVisualsRotation = value; } }

		Transform spatialHintContentContainer { get { return m_SpatialHintUI.contentContainer; } }

		IConnectInterfacesProvider IInjectedFunctionality<IConnectInterfacesProvider>.provider { get; set; }
		IInstantiateUIProvider IInjectedFunctionality<IInstantiateUIProvider>.provider { get; set; }
		INodeToRayProvider IInjectedFunctionality<INodeToRayProvider>.provider { get; set; }
		IRayVisibilitySettingsProvider IInjectedFunctionality<IRayVisibilitySettingsProvider>.provider { get; set; }

		void Awake()
		{
			m_SpatialHintUI = this.InstantiateUI(m_SpatialHintUI.gameObject).GetComponent<SpatialHintUI>();
			this.ConnectInterfaces(m_SpatialHintUI);
		}

		public void PulseSpatialHintScrollArrows()
		{
			m_SpatialHintUI.PulseScrollArrows();
		}

		public void SetSpatialHintState(SpatialHintStateFlags newState)
		{
			state = newState;
		}

		public void SetSpatialHintPosition(Vector3 newPosition)
		{
			spatialHintContentContainer.position = newPosition;
		}

		public void SetSpatialHintContainerRotation(Quaternion newRotation)
		{
			m_SpatialHintUI.transform.rotation = newRotation;
		}

		public void SetSpatialHintShowHideRotationTarget(Vector3 target)
		{
			spatialHintScrollVisualsRotation = target;
		}

		public void SetSpatialHintLookAtRotation(Vector3 position)
		{
			var orig = spatialHintContentContainer.rotation;
			spatialHintContentContainer.LookAt(position);
			spatialHintContentContainer.rotation = orig;
		}

		public void SetSpatialHintDragThresholdTriggerPosition(Vector3 position)
		{
			if (state == SpatialHintStateFlags.Hidden || position == m_SpatialHintUI.scrollVisualsDragThresholdTriggerPosition)
				return;

			m_SpatialHintUI.scrollVisualsDragThresholdTriggerPosition = position;
		}

		public void SetSpatialHintControlNode(Node controlNode)
		{
			controllingNode = controlNode;
			this.AddRayVisibilitySettings(this.RequestRayOriginFromNode(m_ControllingNode), this, false, false);
		}
	}
}
#endif
