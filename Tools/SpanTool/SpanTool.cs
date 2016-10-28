using System;
using CB.VossetSystem;
using UnityEditor;
using UnityEngine;
using UnityEditor.VR;
using UnityEngine.VR.Tools;
using UnityEngine.VR.Utilities;
using UnityEngine.InputNew;

[MainMenuItem("Span", "Create", "Create a span")]
public class SpanTool : MonoBehaviour, ITool, IStandardActionMap, IRay
{
	[SerializeField]
	SpanSettings m_SpanSettings;

	const float m_DistanceToSpawnNewSpan = 1f;

	public Standard standardInput
	{
		get; set;
	}

	public Transform rayOrigin
	{
		get; set;
	}

	void Update()
	{
		if (standardInput.action.wasJustPressed)
		{
			if (rayOrigin)
			{
				var spanGroupTrans = U.Object.Instantiate(m_SpanSettings.spanGroup).transform;
				spanGroupTrans.position = rayOrigin.position + rayOrigin.forward * m_DistanceToSpawnNewSpan 
					+ Vector3.up * m_SpanSettings.groupHandleElevation;
			}
		}
	}
}
