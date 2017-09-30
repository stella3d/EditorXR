#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Gives decorated class the ability to place objects in the scene or a MiniWorld
	/// </summary>
	public interface IPlaceSceneObjects : IInjectedFunctionality<IPlaceSceneObjectsProvider>
	{
	}

	public interface IPlaceSceneObjectsProvider
	{
		/// <summary>
		/// Method used to place groups of objects in the scene/MiniWorld
		/// </summary>
		/// <param name="transforms">Array of Transforms to place</param>
		/// <param name="targetPositionOffsets">Array of per-object target positions</param>
		/// <param name="targetRotations">Array of per-object target rotations</param>
		/// <param name="targetScales">Array of per-object target scales</param>
		void PlaceSceneObjects(Transform[] transforms, Vector3[] targetPositionOffsets, Quaternion[] targetRotations, Vector3[] targetScales);
	}

	public static class IPlaceSceneObjectsMethods
	{
		/// <summary>
		/// Method used to place groups of objects in the scene/MiniWorld
		/// </summary>
		/// <param name="transforms">Array of Transforms to place</param>
		/// <param name="targetPositionOffsets">Array of per-object target positions</param>
		/// <param name="targetRotations">Array of per-object target rotations</param>
		/// <param name="targetScales">Array of per-object target scales</param>
		public static void PlaceSceneObjects(this IPlaceSceneObjects @this, Transform[] transforms, Vector3[] targetPositionOffsets, Quaternion[] targetRotations, Vector3[] targetScales)
		{
			@this.provider.PlaceSceneObjects(transforms, targetPositionOffsets, targetRotations, targetScales);
		}
	}
}
#endif
