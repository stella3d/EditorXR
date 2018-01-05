#if UNITY_EDITOR
using UnityEditor.Experimental.EditorVR;
using UnityEditor.Experimental.EditorVR.Data;
using UnityEditor.Experimental.EditorVR.Utilities;
using UnityEngine;

#if INCLUDE_TEXT_MESH_PRO
using TMPro;
#else
using UnityEngine.UI;
#endif

[assembly: OptionalDependency("TMPro.TextMeshProUGUI", "INCLUDE_TEXT_MESH_PRO")]

namespace UnityEditor.Experimental.EditorVR.Workspaces
{
    sealed class InspectorUnimplementedItem : InspectorPropertyItem
    {
        [SerializeField]
#if INCLUDE_TEXT_MESH_PRO
        TextMeshProUGUI m_TypeLabel;
#else
        Text m_TypeLabel;
#endif

        public override void Setup(InspectorData data)
        {
            base.Setup(data);

            m_TypeLabel.text = ObjectUtils.NicifySerializedPropertyType(m_SerializedProperty.type);
        }
    }
}
#endif
