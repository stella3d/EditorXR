#if UNITY_EDITOR
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEditor.Experimental.EditorVR.Core;
using UnityEngine;
using UnityEngineInternal.Input;
using XRAuthoring.Input;

namespace UnityEditor.Experimental.EditorVR.Modules
{
	sealed class HapticsModule : MonoBehaviour
	{
		public const float MaxDuration = 0.8f;

		[SerializeField]
		float m_MasterIntensity = 0.8f;

		/// <summary>
		/// Overall intensity of haptics.
		/// A value to 0 will mute haptics.
		/// A value of 1 will allow haptics to be performed at normal intensity
		/// </summary>
		public float masterIntensity { set { m_MasterIntensity = Mathf.Clamp(value, 0f, 10f); } }

#if ENABLE_OVR_INPUT
		OVRHaptics.OVRHapticsChannel m_LHapticsChannel;
		OVRHaptics.OVRHapticsChannel m_RHapticsChannel;
		OVRHapticsClip m_GeneratedHapticClip;
#endif

		/// <summary>
		/// Allow for a single warning that informs the user of an attempted pulse with a length greater than 0.8f
		/// </summary>
		bool m_SampleLengthWarningShown;

		void Start()
		{
#if ENABLE_OVR_INPUT
			m_LHapticsChannel = OVRHaptics.LeftChannel;
			m_RHapticsChannel = OVRHaptics.RightChannel;
			m_GeneratedHapticClip = new OVRHapticsClip();
#endif
		}

#if ENABLE_OVR_INPUT
		void LateUpdate()
		{
			// Perform a manual update of OVR haptics
			OVRHaptics.Process();
		}
#else
		void LateUpdate()
		{
			RumbleEvent outputEvent;
			if (m_Pulsing)
			{
				outputEvent = new RumbleEvent(1f, 0);
			}
			else
			{
				outputEvent = new RumbleEvent(0f, 0);
			}
			NativeInputSystem.SendOutput(TrackedNodeDeviceManager.DeviceIDForTrackedNode(Node.RightHand), outputEvent.type, outputEvent);
		}
#endif

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

		bool m_Pulsing;
		IEnumerator TestPulse()
		{
			m_Pulsing = true;
			yield return new WaitForSeconds(0.15f);
			m_Pulsing = false;
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
			// Clip buffer can hold up to 800 milliseconds of samples
			// At 320Hz, each sample is 3.125f milliseconds
			if (Mathf.Approximately(m_MasterIntensity, 0))
				return;

			StartCoroutine(TestPulse());
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
#endif
		}

		public void StopPulses(Node? node)
		{
#if ENABLE_OVR_INPUT
			var channel = GetTargetChannel(node);
			if (channel != null)
				channel.Clear();
			else
				Debug.LogWarning("Only null, or valid ray origins can stop pulse playback.");
#endif
		}

		public void StopPulses()
		{
#if ENABLE_OVR_INPUT
			m_RHapticsChannel.Clear();
			m_LHapticsChannel.Clear();
#endif
		}

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
