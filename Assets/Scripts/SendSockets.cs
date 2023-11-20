using System;
using System.Collections;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using WebSocketSharp;

public class SendSockets : MonoBehaviour
{
	WebSocket ws;
	SocketMessage sm = new SocketMessage();
	public Gyroscope gyroscope;

	private bool stop_connecting = true;

	public bool OPPENED, CLOSED;

	private void Start()
	{
		sm.MessageType = "GYROSCOPE";
		sm.Coordinates = new Coordinates();
		SendData += Send;
	}

	public void StartConnecting(string ip)
	{
		Debug.Log("StartConnecting");

		ws = new WebSocket("ws://" + ip + "/connect");//"ws://172.16.5.171:1000/connect"
		stop_connecting = false;

		ws.OnOpen += delegate { OPPENED = true; };
		ws.OnClose += delegate { CLOSED = true; };
		ws.OnError += delegate { Debug.Log("Error");};
		ws.OnMessage += (sender, e) => { Debug.Log("Message Received from " + ((WebSocket)sender).Url + ", Data : " + e.Data); };


		ws.Connect();

		StartCoroutine(WhileConnect());
	}

	public void StopConnecting()
	{
		StopAllCoroutines();
		stop_connecting = true;

		if(ws != null)
		{
			ws.Close();
			ws = null;
		}
	}

	private IEnumerator WhileConnect()
	{
		while (true)
		{
			yield return new WaitForSeconds(2);

			if (stop_connecting || ws == null || !ws.IsAlive)
			{
				UIManager.Instance.SetMessage(true);
			}
		}
	}

	private void Update()
	{
		if (OPPENED)
		{
			UIManager.Instance.SetStatePanels(true);
			UIManager.Instance.SetMessage(false);
			OPPENED = false;
		}

		if(CLOSED)
		{
			UIManager.Instance.SetMessage(true);
			CLOSED = false;
			StopConnecting();
		}

		Send();
	}

	//private IEnumerator UpdateData()
	//{
	//	while (true)
	//	{
	//		yield return new WaitForEndOfFrame();

	//		if (OPPENED)
	//		{
	//			UIManager.Instance.SetStatePanels(true);
	//			UIManager.Instance.SetMessage(false);
	//			OPPENED = false;
	//		}

	//		yield return new WaitForEndOfFrame();

	//		if (stop_connecting || ws == null || !ws.IsAlive)
	//			continue;

	//		Send();

	//		yield return new WaitForEndOfFrame();
	//	}
	//}

	public Action<bool> Completed;
	public static Action SendData;

	public void Send()
	{
		if (stop_connecting || ws == null || !ws.IsAlive)
			return;

		if (gyroscope.dir == Vector2.zero && !Gyroscope.RESET && !ButtonExtender.UP && !ButtonExtender.DOWN)
			return;

		sm.Coordinates.X2 = (sbyte)(gyroscope.dir.x * 20);
		sm.Coordinates.Y2 = (sbyte)(gyroscope.dir.y * 20);
		sm.Coordinates.reset = Gyroscope.RESET;
		sm.Coordinates.up = ButtonExtender.UP;
		sm.Coordinates.down = ButtonExtender.DOWN;

		Gyroscope.RESET = false;

		var s = JsonUtility.ToJson(sm);

		ws.Send(s/*, Completed*/);
	}

	public static (string address, bool finded) GetLocalIPAddress()
	{
		var host = Dns.GetHostEntry(Dns.GetHostName());

		foreach (var ip in host.AddressList)
		{
			Debug.Log(ip.ToString());

			if (ip.AddressFamily == AddressFamily.InterNetwork)
			{
				return (ip.ToString(), true);
			}
		}

		return ("No network adapters with an IPv4 address in the system!", false);
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
	public sbyte X2, Y2;
	public bool reset;
	public bool down;
	public bool up;
}