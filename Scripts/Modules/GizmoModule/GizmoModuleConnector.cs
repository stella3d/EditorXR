#if UNITY_EDITOR && UNITY_EDITORVR
namespace UnityEditor.Experimental.EditorVR.Core
{
	partial class EditorVR
	{
		class GizmoModuleConnector : Nested, ILateBindInterfaceMethods<GizmoModule>
		{
			public void LateBindInterfaceMethods(GizmoModule provider)
			{
				IUsesGizmosMethods.drawRay = provider.DrawRay;
				IUsesGizmosMethods.drawSphere = provider.DrawSphere;
				IUsesGizmosMethods.drawCube = provider.DrawCube;
			}
		}
	}
}
#endif