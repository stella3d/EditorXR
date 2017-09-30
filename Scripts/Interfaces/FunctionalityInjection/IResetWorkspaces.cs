#if UNITY_EDITOR
namespace UnityEditor.Experimental.EditorVR
{
	public interface IResetWorkspaces : IInjectedFunctionality<IResetWorkspacesProvider>
	{
	}

	public interface IResetWorkspacesProvider
	{
		/// <summary>
		/// Reset all open workspaces
		/// </summary>
		void ResetWorkspaceRotations();
	}

	public static class IResetWorkspacesMethods
	{
		/// <summary>
		/// Reset all open workspaces
		/// </summary>
		public static void ResetWorkspaceRotations(this IResetWorkspaces obj)
		{
			obj.provider.ResetWorkspaceRotations();
		}
	}
}
#endif
