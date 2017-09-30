#if UNITY_EDITOR
namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Implementors can check whether the manipulator is in the dragging state
	/// </summary>
	public interface IGetManipulatorDragState : IInjectedFunctionality<IGetManiuplatorDragStateProvider>
	{
	}

	public interface IGetManiuplatorDragStateProvider
	{
		/// <summary>
		/// Returns whether the manipulator is in the dragging state
		/// </summary>
		bool GetManipulatorDragState();
	}

	public static class IGetManipulatorDragStateMethods
	{
		/// <summary>
		/// Returns whether the manipulator is in the dragging state
		/// </summary>
		public static bool GetManipulatorDragState(this IGetManipulatorDragState @this)
		{
			return @this.GetManipulatorDragState();
		}
	}
}
#endif
