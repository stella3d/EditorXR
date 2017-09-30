#if UNITY_EDITOR
using System.Collections.Generic;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Provides access to other tools of the same type
	/// </summary>
	public interface ILinkedObject : IInjectedFunctionality<ILinkedObjectProvider>
	{
		/// <summary>
		/// List of other tools of the same type (not including this one)
		/// </summary>
		List<ILinkedObject> linkedObjects { set; }
	}

	public interface ILinkedObjectProvider
	{
		/// <summary>
		/// Returns whether the specified object is designated to perform the duties of all linked objects of this type
		/// </summary>
		/// <param name="linkedObject">Object among the linked objects to check if it is the central one</param>
		bool IsSharedUpdater(ILinkedObject linkedObject);
	}

	public static class ILinkedObjectMethods
	{
		/// <summary>
		/// Returns whether the specified object is designated to perform the duties of all linked objects of this type
		/// </summary>
		/// <param name="linkedObject">Object among the linked objects to check if it is the central one</param>
		public static bool IsSharedUpdater(this ILinkedObject @this, ILinkedObject linkedObject)
		{
			return @this.provider.IsSharedUpdater(linkedObject);
		}
	}
}
#endif
