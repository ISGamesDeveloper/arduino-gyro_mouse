using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public SendSockets sendSockets;

    public GameObject Connected;
    public GameObject StartPanel;

    public InputField IPAddressField;
    public Button ApplyIPButton;
    public Button ExitButton;

	public Text ConnectingMessage;
	public Image ImageOutline;

	void Start()
    {
        SetStatePanels(false);

        ApplyIPButton.onClick.AddListener(delegate { ConnectingMessage.enabled = true; sendSockets.StartConnecting(IPAddressField.text);});
		ExitButton.onClick.AddListener(delegate { sendSockets.StopConnecting(); SetStatePanels(false); });

		IPAddressField.text = SendSockets.GetLocalIPAddress().address + ":1000";
	}

    public void SetStatePanels(bool connected)
    {
		Connected.SetActive(connected);
		StartPanel.SetActive(!connected);

		ConnectingMessage.enabled = connected;
		Debug.Log("SetStatePanels: " + connected);
	}

	public void SetMessage(bool error)
	{
		Debug.Log("SetMessage: " + error);

		if (error)
		{
			ConnectingMessage.text = "No connection established. \r\nPlease reconnect";
			ConnectingMessage.color = Color.red;
			ImageOutline.color = Color.red;
		}
		else
		{
			ConnectingMessage.text = "Tap the screen \r\nto control the model";
			ConnectingMessage.color = new Color(0, 0.55f, 1, 1);
			ImageOutline.color = new Color(0, 0.55f, 1, 1); ;
		}
	}
}
