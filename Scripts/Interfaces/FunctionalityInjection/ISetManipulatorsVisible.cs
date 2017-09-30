#if UNITY_EDITOR
namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Provide access to show or hide manipulator(s)
	/// </summary>
	public interface ISetManipulatorsVisible : IInjectedFunctionality<ISetManipulatorsVisibleProvider>
	{
	}

	public interface ISetManipulatorsVisibleProvider
	{
		/// <summary>
		/// Show or hide the manipulator(s)
		/// </summary>
		/// <param name="caller">The calling object</param>
		/// <param name="visibility">Whether the manipulators should be shown or hidden</param>
		void SetManipulatorsVisible(ISetManipulatorsVisible caller, bool visibility);
	}

	public static class ISetManipulatorsVisibleMethods
	{
		/// <summary>
		/// Show or hide the manipulator(s)
		/// </summary>
		/// <param name="visibility">Whether the manipulators should be shown or hidden</param>
		public static void SetManipulatorsVisible(this ISetManipulatorsVisible @this, bool visibility)
		{
			@this.provider.SetManipulatorsVisible(@this, visibility);
		}
	}
}
#endif
