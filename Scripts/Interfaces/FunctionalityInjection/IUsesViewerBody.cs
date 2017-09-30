#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Provides access to checks that can test against the viewer's body
	/// </summary>
	public interface IUsesViewerBody : IInjectedFunctionality<IUsesViewerBodyProvider>
	{
	}

	public interface IUsesViewerBodyProvider
	{
		/// <summary>
		/// Returns whether the specified transform is over the viewer's shoulders and behind the head
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin to test</param>
		bool IsOverShoulder(Transform rayOrigin);

		/// <summary>
		/// Returns whether the specified transform is over the viewer's head
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin to test</param>
		bool IsAboveHead(Transform rayOrigin);
	}

	public static class IUsesViewerBodyMethods
	{
		/// <summary>
		/// Returns whether the specified transform is over the viewer's shoulders and behind the head
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin to test</param>
		public static bool IsOverShoulder(this IUsesViewerBody @this, Transform rayOrigin)
		{
			return @this.provider.IsOverShoulder(rayOrigin);
		}

		/// <summary>
		/// Returns whether the specified transform is over the viewer's head
		/// </summary>
		/// <param name="rayOrigin">The rayOrigin to test</param>
		public static bool IsAboveHead(this IUsesViewerBody @this, Transform rayOrigin)
		{
			return @this.provider.IsAboveHead(rayOrigin);
		}
	}
}
#endif
