#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR.Core
{
	delegate void ForEachRayOriginCallback(Transform rayOrigin);

	interface IForEachRayOrigin : IInjectedFunctionality<IForEachRayOriginProvider>
	{
	}

	interface IForEachRayOriginProvider
	{
		void ForEachRayOrigin(ForEachRayOriginCallback callback);
	}

	static class IForEachRayOriginMethods
	{
		public static void ForEachRayOrigin(this IForEachRayOrigin obj, ForEachRayOriginCallback callback)
		{
			obj.provider.ForEachRayOrigin(callback);
		}
	}
}
#endif
