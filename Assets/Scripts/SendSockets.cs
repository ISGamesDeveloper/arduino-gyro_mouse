using System;
using System.Collections;
using UnityEngine;
using WebSocketSharp;

public class SendSockets : MonoBehaviour
{
	WebSocket ws = new WebSocket("ws://172.16.5.171:1000/connect");
	SocketMessage sm = new SocketMessage();
	public Gyroscope gyroscope;

	private void Start()
	{
		sm.MessageType = "GYROSCOPE";
		sm.Coordinates = new Coordinates();
		ws.Connect();

		Debug.Log("URL: " + ws.Url);
		ws.OnOpen += delegate { Debug.Log("IsOpen"); };

		ws.OnMessage += (sender, e) =>
		{
			Debug.Log("Message Received from " + ((WebSocket)sender).Url + ", Data : " + e.Data);
		};

		StartCoroutine(upd());
	}

	private IEnumerator upd()
	{
		while (true)
		{
			Send();

			yield return new WaitForEndOfFrame();
		}
	}

	public void Send()
	{
		sm.Coordinates.X = gyroscope.dir.x * 100;
		sm.Coordinates.Y = gyroscope.dir.y * 100;
		sm.Coordinates.reset = Gyroscope.RESET;
		sm.Coordinates.up = ButtonExtender.UP;
		sm.Coordinates.down = ButtonExtender.DOWN;

		Gyroscope.RESET = false;

		var s = JsonUtility.ToJson(sm);
		ws.Send(s);
	}
}

[Serializable]
public class SocketMessage
{
	public string MessageType;
	public Coordinates Coordinates;
}

[Serializable]
public class Coordinates
{
	public float X, Y;
	public bool reset;
	public bool down;
	public bool up;
}