using System;
using UnityEngine;
using UnityEngine.VR.Tools;
using UnityEngine.VR.Utilities;
using UnityEngine.InputNew;
using UnityEngine.VR;
using UnityEngine.VR.Handles;
using UnityEngine.VR.Modules;
using UnityEngine.VR.Workspaces;

[MainMenuItem("Span", "Create", "Create a span")]
public class SpanTool : MonoBehaviour, ITool, IStandardActionMap, IRay, IInstantiateMenuUI
{
	private const float k_MaximumRayRange = 10f;
	
	[SerializeField]
	private SpanSettings m_SpanSettings;

	[SerializeField]
	[Tooltip("The piece to repeat over the span. Provide a default asset here, but the user can choose another piece " +
		 "by dropping an asset from the project workspace into the drop spot in the SpanTool UI.")]
	private GameObject m_SpanPiece;

	[SerializeField]
	private Canvas m_CanvasPrefab;

	private SpanGroup m_SpanGroup;
	private float m_InitialElevation;
	private bool m_CanvasSpawned;
	private AssetGridItem m_HoveredDragObject;
	private Transform m_PreviewPiece;
	private Transform m_BaseTrans;
	private GameObject m_Icon;

	public Node selfNode { get; set; }

	public Standard standardInput { get; set; }

	public Transform rayOrigin { get; set; }

	public Func<Node, MenuOrigin, GameObject, GameObject> instantiateMenuUI { get; set; }
	
	private LayerMask raycastMask
	{
		get
		{
			if (!m_RaycastMask.HasValue)
			{
				m_RaycastMask = ~(1 << LayerMask.NameToLayer("UI"));
				return m_RaycastMask.Value;
			}
			return m_RaycastMask.Value;
		}
	}
	private LayerMask? m_RaycastMask;

	void OnEnable()
	{
		SpanGroup.SetAllSpansUIVisible(true);
	}

	void OnDisable()
	{
		SpanGroup.SetAllSpansUIVisible(false);
	}

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
				// Without inputBlock here, the action map gets reset, and .isHeld is stuck on / .wasJustReleased never 
				// fires. This is a mysterious case where this sticking behavior only happens when running enough logic
				// within the .isHeld block below. Likely related to an error that is logged while using the span tool,
				// "normalOffset >= 0", without any call stack. Further investigation is required.....
				// (For the time being, proceeding with this inputBlock workaround)
				MultipleRayInputModule.inputBlock = true;
				RaycastHit hit;
				if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, k_MaximumRayRange, raycastMask))
				{
					m_SpanGroup = U.Object.Instantiate(m_SpanSettings.spanGroup.gameObject).GetComponent<SpanGroup>();
					m_SpanGroup.transform.position = hit.point + Vector3.up * m_SpanSettings.groupHandleElevation;
					m_InitialElevation = hit.point.y;
					m_SpanGroup.SetupPieceAndCreateSpan(m_SpanPiece);
				}
			}
		}
		if (standardInput.action.isHeld)
		{
			if (m_SpanGroup == null)
				return;
			RaycastHit hit;
			if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out hit, k_MaximumRayRange, raycastMask))
			{
				var targetPosition = hit.point;
				targetPosition.y = m_InitialElevation;
				m_SpanGroup.MoveSecondVertex(targetPosition);
			}
		}
		if (standardInput.action.wasJustReleased)
		{
			if (m_SpanGroup == null)
				return;
			m_SpanGroup.MoveHandleToCenter();
			m_SpanGroup = null;
			MultipleRayInputModule.inputBlock = false;
		}
	}

	void SpawnCanvas()
	{
		var go = instantiateMenuUI(selfNode, MenuOrigin.Main, m_CanvasPrefab.gameObject);
		m_CanvasSpawned = true;
		var handle = go.GetComponentInChildren<BaseHandle>();
		m_BaseTrans = handle.transform;
		handle.dropHoverEnded += DropHoverEnded;
		handle.canDrop += CanDrop;
		handle.receiveDrop += ReceiveDrop;
		m_Icon = handle.GetComponentInChildren<WorkspaceButton>().icon.gameObject;
	}

	void DropHoverEnded(BaseHandle handle)
	{
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
		if (m_Icon.activeSelf)
		{
			m_Icon.SetActive(false);
		}
		m_SpanPiece = (GameObject)droppable.GetDropObject();
		if (m_PreviewPiece != null)
		{
			DestroyImmediate(m_PreviewPiece.gameObject);
		}
		m_PreviewPiece = Instantiate(m_SpanPiece).transform;
		m_PreviewPiece.parent = m_BaseTrans.transform;
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
		var scale = (0.5f / max) * 200f; // tmp! Magic numbers.
		m_PreviewPiece.localScale = Vector3.one * scale;
	}
}
