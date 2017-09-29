#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.Experimental.EditorVR.Core;
using UnityEngine;
using UnityEngineInternal.Input;
using XRAuthoring.Input;

namespace UnityEditor.Experimental.EditorVR.Modules
{
	sealed class HapticsModule : MonoBehaviour
	{
		class HapticsChannel
		{
			public float intensity;
			public Coroutine doPulseCoroutine;
		}

		public const float MaxDuration = 0.8f;

		[SerializeField]
		float m_MasterIntensity = 0.8f;

		/// <summary>
		/// Overall intensity of haptics.
		/// A value to 0 will mute haptics.
		/// A value of 1 will allow haptics to be performed at normal intensity
		/// </summary>
		public float masterIntensity { set { m_MasterIntensity = Mathf.Clamp01(value); } }

#if ENABLE_OVR_INPUT
		OVRHaptics.OVRHapticsChannel m_LHapticsChannel;
		OVRHaptics.OVRHapticsChannel m_RHapticsChannel;
		OVRHapticsClip m_GeneratedHapticClip;
#else
		Dictionary<Node, HapticsChannel> m_HapticsChannels = new Dictionary<Node, HapticsChannel>();
#endif

		/// <summary>
		/// Allow for a single warning that informs the user of an attempted pulse with a duration greater than 0.8f
		/// </summary>
		bool m_DurationWarningShown;

		void Start()
		{
#if ENABLE_OVR_INPUT
			m_LHapticsChannel = OVRHaptics.LeftChannel;
			m_RHapticsChannel = OVRHaptics.RightChannel;
			m_GeneratedHapticClip = new OVRHapticsClip();
#else
			foreach (Node node in Enum.GetValues(typeof(Node)))
			{
				m_HapticsChannels[node] = new HapticsChannel();
			}
#endif
		}

		void LateUpdate()
		{
#if ENABLE_OVR_INPUT
			// Perform a manual update of OVR haptics
			OVRHaptics.Process();
#else
			foreach (var kvp in m_HapticsChannels)
			{
				var rumbleEvent = new RumbleEvent(kvp.Value.intensity * m_MasterIntensity, 0);
				NativeInputSystem.SendOutput(TrackedNodeDeviceManager.DeviceIDForTrackedNode(kvp.Key), rumbleEvent.type, rumbleEvent);
			}
#endif
		}

		public struct FourCC
		{
			int m_Code;

			public FourCC(char a, char b, char c, char d)
			{
				m_Code = (a << 24) | (b << 16) | (c << 8) | d;
			}

			public FourCC(string str)
				: this()
			{
				Debug.Assert(str.Length == 4, "FourCC string must be exactly four characters long!");
				m_Code = (str[0] << 24) | (str[1] << 16) | (str[2] << 8) | str[3];
			}

			public static implicit operator int(FourCC fourCC)
			{
				return fourCC.m_Code;
			}

			public override string ToString()
			{
				return string.Format("'{0}{1}{2}{3}'",
					(char)(m_Code >> 24), (char)((m_Code & 0xff0000) >> 16), (char)((m_Code & 0xff00) >> 8), (char)(m_Code & 0xff));
			}
		}

		public interface IOutputEvent
		{
			FourCC type { get; }
		}

		enum HapticsEventType
		{
			None = 0,
			EnqueueRumble = 1,
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct RumbleEvent : IOutputEvent
		{
			public FourCC type { get { return new FourCC("0VCR"); } }

			public int hapticsEventType;
			public float intensity;
			public int channel;

			public RumbleEvent(float intensity, int channel)
			{
				hapticsEventType = (int)HapticsEventType.EnqueueRumble;
				this.intensity = intensity;
				this.channel = channel;
			}
		}

		/// <summary>
		/// Pulse haptic feedback
		/// </summary>
		/// <param name="node">Node on which to perform the pulse.</param>
		/// <param name="hapticPulse">Haptic pulse</param>
		/// <param name="durationMultiplier">(Optional) Multiplier value applied to the hapticPulse duration</param>
		/// <param name="intensityMultiplier">(Optional) Multiplier value applied to the hapticPulse intensity</param>
		public void Pulse(Node? node, HapticPulse hapticPulse, float durationMultiplier = 1f, float intensityMultiplier = 1f)
		{
#if ENABLE_OVR_INPUT
			m_GeneratedHapticClip.Reset();

			var duration = hapticPulse.duration * durationMultiplier;
			var intensity = hapticPulse.intensity * intensityMultiplier;
			var fadeIn = hapticPulse.fadeIn;
			var fadeOut = hapticPulse.fadeOut;
			if (duration > MaxDuration)
			{
				duration = Mathf.Clamp(duration, 0f, MaxDuration); // Clamp at maximum 800ms for sample buffer

				if (!m_SampleLengthWarningShown)
					Debug.LogWarning("Pulse durations greater than 0.8f are not currently supported");

				m_SampleLengthWarningShown = true;
			}

			const int kSampleRateConversion = 490; // Samplerate conversion : 44100/90fps = 490
			const int kIntensityIncreaseMultiplier = 255; // Maximum value of 255 for intensity
			const float kFadeInProportion = 0.25f;
			var fadeInSampleCount = duration * kSampleRateConversion * kFadeInProportion;
			var fadeOutSampleCount = fadeInSampleCount * 2; // FadeOut is less apparent than FadeIn unless FadeOut duration is increased
			duration *= kSampleRateConversion;
			var durationFadeOutPosition = duration - fadeOutSampleCount;
			intensity = Mathf.Clamp(Mathf.Clamp01(intensity) * kIntensityIncreaseMultiplier * m_MasterIntensity, 0, kIntensityIncreaseMultiplier);
			var hapticClipSample = Convert.ToByte(intensity);
			for (int i = 1; i < duration; ++i)
			{
				float sampleShaped = hapticClipSample;
				if (fadeIn && i < fadeInSampleCount)
					sampleShaped = Mathf.Lerp(0, intensity, i / fadeInSampleCount);
				else if (fadeOut && i > durationFadeOutPosition)
					sampleShaped = Mathf.Lerp(0, intensity, (duration - i) / fadeOutSampleCount);

				m_GeneratedHapticClip.WriteSample(Convert.ToByte(sampleShaped));
			}

			const float kMaxSimultaneousClipDuration = 0.25f;
			var channel = GetTargetChannel(node);
			if (duration > kMaxSimultaneousClipDuration)
			{
				// Prevent multiple long clips from playing back simultaneously
				// If the new clip has a long duration, stop playback of any existing clips in order to prevent haptic feedback noise
				if (channel != null)
				{
					channel.Preempt(m_GeneratedHapticClip);
				}
				else
				{
					m_RHapticsChannel.Preempt(m_GeneratedHapticClip);
					m_LHapticsChannel.Preempt(m_GeneratedHapticClip);
				}
				
			}
			else
			{
				// Allow multiple short clips to play simultaneously
				if (channel != null)
				{
					channel.Mix(m_GeneratedHapticClip);
				}
				else
				{
					m_RHapticsChannel.Mix(m_GeneratedHapticClip);
					m_LHapticsChannel.Mix(m_GeneratedHapticClip);
				}
			}
#else
			// TODO: Ability mix intensities rather than preempt each time
			StopPulses(node);

			if (node != null)
			{
				m_HapticsChannels[node.Value].doPulseCoroutine = StartCoroutine(DoPulse(node.Value, hapticPulse, durationMultiplier, intensityMultiplier));
			}
			else
			{
				foreach (var kvp in m_HapticsChannels)
				{
					kvp.Value.doPulseCoroutine = StartCoroutine(DoPulse(kvp.Key, hapticPulse, durationMultiplier, intensityMultiplier));
				}
			}
#endif
		}

		public void StopPulses(Node? node)
		{
#if ENABLE_OVR_INPUT
			var channel = GetTargetChannel(node);
			if (channel != null)
				channel.Clear();
			else
			{
				m_RHapticsChannel.Clear();
				m_LHapticsChannel.Clear();
			}
#else
			if (node != null)
			{
				var channel = m_HapticsChannels[node.Value];
				channel.intensity = 0f;
				if (channel.doPulseCoroutine != null)
				{
					StopCoroutine(channel.doPulseCoroutine);
					channel.doPulseCoroutine = null;
				}
			}
			else
			{
				foreach (var channel in m_HapticsChannels.Values)
				{
					channel.intensity = 0f;
					if (channel.doPulseCoroutine != null)
					{
						StopCoroutine(channel.doPulseCoroutine);
						channel.doPulseCoroutine = null;
					}
				}
			}
#endif
		}

#if !ENABLE_OVR_INPUT
		IEnumerator DoPulse(Node node, HapticPulse hapticPulse, float durationMultiplier, float intensityMultiplier)
		{
			var totalDuration = hapticPulse.duration * durationMultiplier;
			if (totalDuration > MaxDuration)
			{
				totalDuration = Mathf.Clamp(totalDuration, 0f, MaxDuration);
				if (!m_DurationWarningShown)
					Debug.LogWarning("Pulse durations greater than 0.8f are not currently supported");
				m_DurationWarningShown = true;
			}

			const float kFadeInProportion = 0.25f;

			// FadeOut is less apparent than FadeIn unless FadeOut duration is increased
			const float kFadeOutProportion = kFadeInProportion * 2f;

			var fadeInDuration = hapticPulse.fadeIn ? totalDuration * kFadeInProportion : 0f;
			var fadeOutDuration = hapticPulse.fadeOut ? totalDuration * kFadeOutProportion : 0f;
			var intensity = hapticPulse.intensity * intensityMultiplier;
			var peakIntensityDuration = totalDuration - fadeInDuration - fadeOutDuration;

			var channel = m_HapticsChannels[node];
			var timePassed = 0f;
			while (timePassed < fadeInDuration)
			{
				channel.intensity = Mathf.Lerp(0f, intensity, timePassed / fadeInDuration);
				timePassed += Time.deltaTime;
				yield return null;
			}

			channel.intensity = intensity;
			yield return new WaitForSeconds(peakIntensityDuration);

			timePassed = 0f;
			while (timePassed < fadeOutDuration)
			{
				channel.intensity = Mathf.Lerp(intensity, 0f, timePassed / fadeOutDuration);
				timePassed += Time.deltaTime;
				yield return null;
			}

			channel.intensity = 0f;
		}
#endif

#if ENABLE_OVR_INPUT
		OVRHaptics.OVRHapticsChannel GetTargetChannel(Node? node)
		{
			OVRHaptics.OVRHapticsChannel channel = null;
			if (node == null)
				return channel;

			switch (node)
			{
				case Node.LeftHand:
					channel = m_LHapticsChannel;
					break;
				case Node.RightHand:
					channel = m_RHapticsChannel;
					break;
				default:
					Debug.LogWarning("Invalid node. Could not fetch haptics channel.");
					break;
			}

			return channel;
		}
#endif
	}
}
#endif
