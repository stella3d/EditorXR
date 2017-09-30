#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Decorates types that need to move the camera rig
	/// </summary>
	public interface IMoveCameraRig : IInjectedFunctionality<IMoveCameraRigProvider>
	{
	}

	public interface IMoveCameraRigProvider
	{
		/// <summary>
		/// Method for moving the camera rig
		/// </summary>
		/// <param name="position">Target position</param>
		/// <param name="viewDirection">Target view direction in the XZ plane. Y component will be ignored</param>
		void MoveCameraRig(Vector3 position, Vector3? viewDirection = null);
	}

	public static class IMoveCameraRigMethods
	{
		/// <summary>
		/// Method for moving the camera rig
		/// </summary>
		/// <param name="position">Target position</param>
		/// <param name="viewDirection">Target view direction in the XZ plane. Y component will be ignored</param>
		public static void MoveCameraRig(this IMoveCameraRig @this, Vector3 position, Vector3? viewDirection = null)
		{
			@this.provider.MoveCameraRig(position, viewDirection);
		}
	}
}
#endif
