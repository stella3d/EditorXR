#if UNITY_EDITOR
using UnityEngine;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Get access to locking features
	/// </summary>
	public interface IUsesGameObjectLocking : IInjectedFunctionality<IUsesGameObjectLockingProvider>
	{
	}

	public interface IUsesGameObjectLockingProvider
	{
		/// <summary>
		/// Set a GameObject's locked status
		/// </summary>
		/// <param name="go">The GameObject to set locked or unlocked</param>
		/// <param name="locked">Locked or unlocked status</param>
		void SetLocked(GameObject go, bool locked);

		/// <summary>
		/// Check whether a GameObject is locked
		/// </summary>
		/// <param name="go">GameObject locked status to test</param>
		bool IsLocked(GameObject go);
	}

	public static class IUsesGameObjectLockingMethods
	{
		/// <summary>
		/// Set a GameObject's locked status
		/// </summary>
		/// <param name="go">The GameObject to set locked or unlocked</param>
		/// <param name="locked">Locked or unlocked status</param>
		public static void SetLocked(this IUsesGameObjectLocking @this, GameObject go, bool locked)
		{
			@this.provider.SetLocked(go, locked);
		}

		/// <summary>
		/// Check whether a GameObject is locked
		/// </summary>
		/// <param name="go">GameObject locked status to test</param>
		public static bool IsLocked(this IUsesGameObjectLocking @this, GameObject go)
		{
			return @this.provider.IsLocked(go);
		}
	}
}
#endif
