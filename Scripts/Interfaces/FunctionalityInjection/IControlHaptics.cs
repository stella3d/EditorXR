#if UNITY_EDITOR
using UnityEditor.Experimental.EditorVR.Core;

namespace UnityEditor.Experimental.EditorVR
{
	/// <summary>
	/// Gives decorated class ability to control haptic feedback
	/// </summary>
	public interface IControlHaptics : IInjectedFunctionality<IControlHapticsProvider>
	{
	}

	public interface IControlHapticsProvider
	{
		/// <summary>
		/// Perform a haptic feedback pulse
		/// </summary>
		/// <param name="node">Node on which to control the pulse</param>
		/// <param name="hapticPulse">Haptic pulse to perform</param>
		/// <param name="durationMultiplier">(Optional) Multiplier value applied to the hapticPulse duration</param>
		/// <param name="intensityMultiplier">(Optional) Multiplier value applied to the hapticPulse intensity</param>
		void Pulse(Node node, HapticPulse hapticPulse, float durationMultiplier = 1f, float intensityMultiplier = 1f);

		/// <summary>
		/// Stop all haptic feedback on a specific device, or all devices
		/// </summary>
		/// <param name="node">Device RayOrigin/Transform on which to stop all pulses. A NULL value will stop pulses on all devices</param>
		void StopPulses(Node node);
	}

	public static class IControlHapticsMethods
	{
		/// <summary>
		/// Perform a haptic feedback pulse
		/// </summary>
		/// <param name="node">Node on which to control the pulse</param>
		/// <param name="hapticPulse">Haptic pulse to perform</param>
		/// <param name="durationMultiplier">(Optional) Multiplier value applied to the hapticPulse duration</param>
		/// <param name="intensityMultiplier">(Optional) Multiplier value applied to the hapticPulse intensity</param>
		public static void Pulse(this IControlHaptics @this, Node node, HapticPulse hapticPulse, float durationMultiplier = 1f, float intensityMultiplier = 1f)
		{
			@this.provider.Pulse(node, hapticPulse, durationMultiplier, intensityMultiplier);
		}

		/// <summary>
		/// Stop all haptic feedback on a specific device, or all devices
		/// </summary>
		/// <param name="node">Device RayOrigin/Transform on which to stop all pulses. A NULL value will stop pulses on all devices</param>
		public static void StopPulses(this IControlHaptics @this, Node node)
		{
			@this.provider.StopPulses(node);
		}
	}
}
#endif
