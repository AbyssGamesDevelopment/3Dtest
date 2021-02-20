using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public UnityEvent action;
	public UnityEvent onUp;
	public bool isOneShotButton;
	public bool isPressed = false;

	public void OnPointerUp (PointerEventData eventData)
	{
		isPressed = false;
		onUp.Invoke ();
	}

	public void OnPointerDown (PointerEventData eventData)
	{
		isPressed = true;
	}

	public void Update () {
		if (isPressed) {
			action.Invoke ();
			isPressed = !isOneShotButton;
		}
	}

	public void OnMouseOver(){
	}
}
