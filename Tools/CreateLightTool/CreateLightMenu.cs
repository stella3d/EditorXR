#if UNITY_EDITOR
using System;
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR.Tools
{
	sealed class CreateLightMenu : MonoBehaviour, IMenu
	{
		[SerializeField]
		GameObject[] m_HighlightObjects;

		public Action<PrimitiveType, bool> selectPrimitive;
		public Action close;

		public bool visible
		{
			get { return gameObject.activeSelf; }
			set { gameObject.SetActive(value); }
		}

		public GameObject menuContent
		{
			get { return gameObject; }
		}

		public void Close()
		{
			close();
		}
	}
}
#endif
