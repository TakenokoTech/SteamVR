using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveObj : MonoBehaviour {
	private Vector3 screenPoint;
	private Vector3 offset;

	void Start () {
		EventTrigger currentTrigger = gameObject.AddComponent<EventTrigger> ();
		currentTrigger.triggers = new List<EventTrigger.Entry> ();

		EventTrigger.Entry onMouseDown = new EventTrigger.Entry ();
		onMouseDown.eventID = EventTriggerType.BeginDrag;
		onMouseDown.callback.AddListener ((x) => OnMouseDown ());
		currentTrigger.triggers.Add (onMouseDown);

		EventTrigger.Entry onMouseDrag = new EventTrigger.Entry ();
		onMouseDrag.eventID = EventTriggerType.Drag;
		onMouseDrag.callback.AddListener ((x) => OnMouseDrag ());
		currentTrigger.triggers.Add (onMouseDrag);
	}

	public void OnMouseDown () {
		screenPoint = Camera.main.WorldToScreenPoint (transform.position);
		float x = Input.mousePosition.x, y = Input.mousePosition.y;
		offset = transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (x, y, screenPoint.z));
	}

	public void OnMouseDrag () {
		float x = Input.mousePosition.x, y = Input.mousePosition.y;
		Vector3 currentScreenPoint = new Vector3 (x, y, screenPoint.z);
		Vector3 currentPosition = Camera.main.ScreenToWorldPoint (currentScreenPoint) + offset;
		transform.position = currentPosition;
	}
}