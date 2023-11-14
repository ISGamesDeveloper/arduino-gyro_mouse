using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonExtender : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
	public Action OnUp;
	public Action OnDown;
	public Action OnMove;
	public static bool DOWN, UP;
	public void OnPointerDown(PointerEventData eventData)
	{
		DOWN = true;
		UP = false;
		Debug.Log("Down");
		OnDown?.Invoke();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		DOWN = false;
		UP = true;
		Debug.Log("Up");
		OnUp?.Invoke();
	}
}
