using UnityEngine;
using System.Collections;

public class SpanGroupHandle : MonoBehaviour {

	void Awake()
	{
		GetComponent<DirectManipulator>().translate = Move;
	}

	void Move(Vector3 deltaPosition)
	{
		transform.position += deltaPosition;
	}
}
