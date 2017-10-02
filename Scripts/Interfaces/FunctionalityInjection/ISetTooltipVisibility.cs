#if UNITY_EDITOR
namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Provides access to the ability to show or hide a Tooltip
	/// </summary>
	public interface ISetTooltipVisibility : IInjectedFunctionality<ISetTooltipVisibilityProvider>
	{
	}

	public interface ISetTooltipVisibilityProvider
	{
		/// <summary>
		/// Show the given Tooltip
		/// </summary>
		/// <param name="tooltip">The tooltip to show</param>
		/// <param name="persistent">Whether the tooltip should stay visible regardless of raycasts</param>
		/// <param name="duration">If the tooltip is shown persistently, and duration is > 0, hide after the duration, in seconds</param>
		void ShowTooltip(ITooltip tooltip, bool persistent = false, float duration = 0f);

		/// <summary>
		/// Hide the given Tooltip
		/// </summary>
		/// <param name="tooltip">The tooltip to hide</param>
		/// <param name="persistent">Whether to hide the tooltip if it was shown with the persistent argument set to true</param>
		void HideTooltip(ITooltip tooltip, bool persistent = false);
	}

	public static class ISetTooltipVisibilityMethods
	{
		/// <summary>
		/// Show the given Tooltip
		/// </summary>
		/// <param name="tooltip">The tooltip to show</param>
		/// <param name="persistent">Whether the tooltip should stay visible regardless of raycasts</param>
		/// <param name="duration">If the tooltip is shown persistently, and duration is > 0, hide after the duration, in seconds</param>
		public static void ShowTooltip(this ISetTooltipVisibility @this, ITooltip tooltip, bool persistent = false, float duration = 0f)
		{
			@this.provider.ShowTooltip(tooltip, persistent, duration);
		}

		/// <summary>
		/// Hide the given Tooltip
		/// </summary>
		/// <param name="tooltip">The tooltip to hide</param>
		/// <param name="persistent">Whether to hide the tooltip if it was shown with the persistent argument set to true</param>
		public static void HideTooltip(this ISetTooltipVisibility @this, ITooltip tooltip, bool persistent = false)
		{
			@this.provider.HideTooltip(tooltip, persistent);
		}
	}
}
#endif
