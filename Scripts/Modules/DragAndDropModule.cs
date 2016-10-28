using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR.Modules;

public class DragAndDropModule : MonoBehaviour
{
	readonly Dictionary<Transform, IDroppable> m_Droppables = new Dictionary<Transform, IDroppable>();
	readonly Dictionary<Transform, IDropReceiver> m_DropReceivers = new Dictionary<Transform, IDropReceiver>();

	readonly Dictionary<Transform, GameObject> m_HoverObjects = new Dictionary<Transform, GameObject>();

	void SetCurrentDroppable(Transform rayOrigin, IDroppable obj)
	{
		m_Droppables[rayOrigin] = obj;
	}

	IDroppable GetCurrentDroppable(Transform rayOrigin)
	{
		IDroppable obj;
		return m_Droppables.TryGetValue(rayOrigin, out obj) ? obj : null;
	}

	void SetCurrentDropReceiver(Transform rayOrigin, IDropReceiver dropReceiver)
	{
		if (dropReceiver == null)
			m_DropReceivers.Remove(rayOrigin);
		else
			m_DropReceivers[rayOrigin] = dropReceiver;
	}

	public IDropReceiver GetCurrentDropReceiver(Transform rayOrigin)
	{
		IDropReceiver dropReceiver;
		if (m_DropReceivers.TryGetValue(rayOrigin, out dropReceiver))
			return dropReceiver;

		return null;
	}

	public void OnRayEntered(GameObject gameObject, RayEventData eventData)
	{
		var dropReceiver = gameObject.GetComponent<IDropReceiver>();
		if (dropReceiver != null)
		{
			if (dropReceiver.CanDrop(GetCurrentDroppable(eventData.rayOrigin)))
			{
				dropReceiver.OnDropHoverStarted();
				m_HoverObjects[eventData.rayOrigin] = gameObject;
				SetCurrentDropReceiver(eventData.rayOrigin, dropReceiver);
			}
		}
	}

	public void OnRayExited(GameObject gameObject, RayEventData eventData)
	{
		var dropReceiver = gameObject.GetComponent<IDropReceiver>();
		if (dropReceiver != null)
		{
			if (m_HoverObjects.Remove(eventData.rayOrigin))
			{
				dropReceiver.OnDropHoverEnded();
				SetCurrentDropReceiver(eventData.rayOrigin, null);
			}
		}
	}

	public void OnDragStarted(GameObject gameObject, RayEventData eventData)
	{
		var droppable = gameObject.GetComponent<IDroppable>();
		if (droppable != null)
			SetCurrentDroppable(eventData.rayOrigin, droppable);
	}

	public void OnDragEnded(GameObject gameObject, RayEventData eventData)
	{
		var droppable = gameObject.GetComponent<IDroppable>();
		if (droppable != null)
		{
			var rayOrigin = eventData.rayOrigin;
			SetCurrentDroppable(rayOrigin, null);

			var dropReceiver = GetCurrentDropReceiver(rayOrigin);
			if (dropReceiver != null && dropReceiver.CanDrop(droppable))
				dropReceiver.ReceiveDrop(droppable);
		}
	}
}