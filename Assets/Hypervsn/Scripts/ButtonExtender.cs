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

	private bool _canSendAction = true;

	public void OnPointerDown(PointerEventData eventData)
	{
		if(_canSendAction)
		{
			DOWN = true;
			UP = false;
			Debug.Log("Down");
			OnDown?.Invoke();

			_canSendAction = false;

			SendSockets.SendData.Invoke();

			DOWN = false;
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_canSendAction = true;

		DOWN = false;
		UP = true;
		Debug.Log("Up");
		OnUp?.Invoke();

		SendSockets.SendData.Invoke();

		UP = false;
	}
}
