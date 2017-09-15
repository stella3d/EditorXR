#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR.Proxies
{
	/// <summary>
	/// Reference container for additional content origins on a device
	/// </summary>
	sealed class ProxyHelper : MonoBehaviour
	{
		/// <summary>
		/// The transform that the device's ray contents (default ray, custom ray, etc) will be parented under
		/// </summary>
		public Transform rayOrigin
		{
			get { return m_RayOrigin; }
		}

		[SerializeField]
		Transform m_RayOrigin;

		/// <summary>
		/// The transform that the menu content will be parented under
		/// </summary>
		public Transform menuOrigin
		{
			get { return m_MenuOrigin; }
		}

		[SerializeField]
		Transform m_MenuOrigin;

		/// <summary>
		/// The transform that the alternate-menu content will be parented under
		/// </summary>
		public Transform alternateMenuOrigin
		{
			get { return m_AlternateMenuOrigin; }
		}

		[SerializeField]
		Transform m_AlternateMenuOrigin;

		/// <summary>
		/// The transform that the display/preview objects will be parented under
		/// </summary>
		public Transform previewOrigin
		{
			get { return m_PreviewOrigin; }
		}

		[SerializeField]
		Transform m_PreviewOrigin;

		/// <summary>
		/// The transform that the display/preview objects will be parented under
		/// </summary>
		public Transform fieldGrabOrigin
		{
			get { return m_FieldGrabOrigin; }
		}

		[SerializeField]
		Transform m_FieldGrabOrigin;

		/// <summary>
		/// The root transform of the device/controller mesh-renderers/geometry
		/// </summary>
		public Transform meshRoot
		{
			get { return m_MeshRoot; }
		}

		[SerializeField]
		Transform m_MeshRoot;

		/// <summary>
		/// The elements that will be highlighted when corresponding input actions are performed
		/// </summary>
		public MeshRenderer[] highlightedElements
		{
			get { return m_HighlightedElements; }
		}

		[SerializeField]
		MeshRenderer[] m_HighlightedElements;

		public enum Controls
		{
			Trigger1,
			Trigger2,
			Action1,
			Action2
		}

		[SerializeField]
		Controls m_Controls;
	}
}
#endif
