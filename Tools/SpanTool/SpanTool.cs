using System;
using CB.VossetSystem;
using UnityEditor;
using UnityEngine;
using UnityEditor.VR;
using UnityEngine.VR.Tools;
using UnityEngine.VR.Utilities;
using UnityEngine.InputNew;
using UnityEngine.VR;
using UnityEngine.VR.Handles;
using UnityEngine.VR.Modules;

[MainMenuItem("Span", "Create", "Create a span")]
public class SpanTool : MonoBehaviour, ITool, IStandardActionMap, IRay, IInstantiateMenuUI
{
	[SerializeField]
	private SpanSettings m_SpanSettings;

	[SerializeField]
	[Tooltip("The piece to repeat over the span. Provide a default asset here, but the user can choose another piece " +
		 "by dropping an asset from the project workspace into the drop spot in the SpanTool UI.")]
	private GameObject m_SpanPiece;

	[SerializeField]
	private Canvas m_CanvasPrefab;

	private bool m_CanvasSpawned;
	private Renderer m_DropZoneRenderer;
	private Material m_OriginalMat; // tmp!
	private AssetGridItem m_HoveredDragObject;
	private Transform m_PreviewPiece;

	public Node selfNode { get; set; }

	public Standard standardInput
	{
		get; set;
	}

	public Transform rayOrigin
	{
		get; set;
	}

	public Func<Node, MenuOrigin, GameObject, GameObject> instantiateMenuUI { get; set; }

	void Update()
	{
		if (!m_CanvasSpawned)
		{
			SpawnCanvas();
		}

		if (standardInput.action.wasJustPressed)
		{
			if (rayOrigin)
			{
				var spanGroup = U.Object.Instantiate(m_SpanSettings.spanGroup.gameObject).GetComponent<SpanGroup>();
				spanGroup.transform.position = rayOrigin.position + rayOrigin.forward * m_SpanSettings.distanceToSpawnNewSpan 
					+ Vector3.up * m_SpanSettings.groupHandleElevation;
				spanGroup.SetupPieceAndCreateSpan(m_SpanPiece);
			}
		}
	}

	void SpawnCanvas()
	{
		var go = instantiateMenuUI(selfNode, MenuOrigin.Main, m_CanvasPrefab.gameObject);
		m_CanvasSpawned = true;
		var handle = go.GetComponentInChildren<BaseHandle>();
		m_DropZoneRenderer = handle.GetComponentInChildren<Renderer>();
		m_OriginalMat = m_DropZoneRenderer.sharedMaterial;
		handle.dropHoverStarted += DropHoverStarted;
		handle.dropHoverEnded += DropHoverEnded;
		handle.canDrop += CanDrop;
		handle.receiveDrop += ReceiveDrop;
	}

	void DropHoverStarted(BaseHandle handle)
	{
		m_DropZoneRenderer.material = m_SpanSettings.realVertexHandleMaterial;
	}

	void DropHoverEnded(BaseHandle handle)
	{
		m_DropZoneRenderer.material = m_OriginalMat;
		m_HoveredDragObject.consumed = false;
	}

	bool CanDrop(BaseHandle handle, IDroppable droppable)
	{
		if (droppable == null)
			return false;

		if (!(droppable.GetDropObject() is GameObject))
			return false;

		m_HoveredDragObject = ((MonoBehaviour)droppable).GetComponent<AssetGridItem>();
		m_HoveredDragObject.consumed = true;
		return true;
	}

	void ReceiveDrop(BaseHandle handle, IDroppable droppable)
	{
		m_SpanPiece = (GameObject)droppable.GetDropObject();
		if (m_PreviewPiece != null)
		{
			DestroyImmediate(m_PreviewPiece.gameObject);
		}
		m_PreviewPiece = Instantiate(m_SpanPiece).transform;
		m_PreviewPiece.parent = m_DropZoneRenderer.transform.parent; // tmp!
		m_PreviewPiece.localPosition = Vector3.up * 0.5f;
		m_PreviewPiece.localRotation = Quaternion.identity;
		var bounds = m_PreviewPiece.GetComponent<MeshFilter>().sharedMesh.bounds;
		var max = bounds.extents.x;
		if (max < bounds.extents.y)
		{
			max = bounds.extents.y;
		}
		if (max < bounds.extents.z)
		{
			max = bounds.extents.z;
		}
		var scale = (0.5f / max) * 200f; // tmp!
		m_PreviewPiece.localScale = Vector3.one * scale;
	}
}
