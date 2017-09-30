#if UNITY_EDITOR
namespace UnityEditor.Experimental.EditorVR
{
	public interface IUsesViewerScale : IInjectedFunctionality<IUsesViewerScaleProvider>
	{
	}

	public interface IUsesViewerScaleProvider
	{
		/// <summary>
		/// Returns whether the specified transform is over the viewer's shoulders and behind the head
		/// </summary>
		float GetViewerScale();

		/// <summary>
		/// Set the scale of the viewer object
		/// </summary>
		/// <param name="scale">Uniform scale value in world space</param>
		void SetViewerScale(float scale);
	}

	public static class IUsesViewerScaleMethods
	{
		internal static IUsesViewerScaleProvider defaultProvider;

		/// <summary>
		/// Returns the scale of the viewer object
		/// </summary>
		public static float GetViewerScale(this IUsesViewerScale @this)
		{
			return @this.provider.GetViewerScale();
		}

		/// <summary>
		/// Returns the scale of the viewer object
		/// </summary>
		public static float GetViewerScale()
		{
			return defaultProvider.GetViewerScale();
		}

		/// <summary>
		/// Set the scale of the viewer object
		/// </summary>
		/// <param name="scale">Uniform scale value in world space</param>
		public static void SetViewerScale(this IUsesViewerScale @this, float scale)
		{
			@this.provider.SetViewerScale(scale);
		}
	}
}
#endif
