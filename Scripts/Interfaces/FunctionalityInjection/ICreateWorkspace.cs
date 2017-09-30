#if UNITY_EDITOR
using System;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Create workspaces
	/// </summary>
	public interface ICreateWorkspace : IInjectedFunctionality<ICreateWorkspaceProvider>
	{
	}

	public interface ICreateWorkspaceProvider
	{
		/// <summary>
		/// Method for creating workspaces
		/// </summary>
		/// <param name="type">Type of the workspace (must inherit from Workspace)</param>
		/// <param name="createdCallback">Called once the workspace is created</param>
		void CreateWorkspace(Type type, Action<IWorkspace> createdCallback = null);
	}

	static class ICreateWorkspaceMethods
	{
		/// <summary>
		/// Method for creating workspaces
		/// </summary>
		/// <param name="type">Type of the workspace (must inherit from Workspace)</param>
		/// <param name="createdCallback">Called once the workspace is created</param>
		public static void CreateWorkspace(this ICreateWorkspace @this, Type type, Action<IWorkspace> createdCallback = null)
		{
			@this.provider.CreateWorkspace(type, createdCallback);
		}
	}
}
#endif
