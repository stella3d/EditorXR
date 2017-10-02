#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	interface IGetPointerLength : IInjectedFunctionality<IGetPointerLengthProvider>
	{
	}

	interface IGetPointerLengthProvider
	{
		float GetPointerLength(Transform rayOrigin);
	}

	static class IGetPointerLengthMethods
	{
		public static float GetPointerLength(this IGetPointerLength @this, Transform rayOrigin)
		{
			return @this.provider.GetPointerLength(rayOrigin);
		}
	}
}
#endif
