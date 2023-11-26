using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class Gyroscope : MonoBehaviour
{
    public SendSockets ss;
    public float speed = 10;
    public ButtonExtender TouchPanel;
    public Button ResetButton;
    public Vector2 dir;
    public static bool RESET;
    public Text compassText;
    bool initialized;

    public float startXPos, startYPos;

    void Start()
    {
        ResetButton.onClick.AddListener(delegate { RESET = true; ResetPosition(); });
        Init();
    }

    private async void Init()
    {
        initialized = false;
        await Task.Delay(5000);

        Input.compass.enabled = true;
        Input.location.Start();

        Input.gyro.enabled = true;
        Input.gyro.updateInterval = 1f;

        transform.position = Vector3.zero;
        await Task.Delay(1000);

        ResetPosition();

        initialized = true;
    }

    private void ResetPosition()
    {
        startXPos = Input.compass.magneticHeading;
        startYPos = -Input.acceleration.y * 100;

        oldCompass = startXPos;
        oldYpos = startYPos;
    }

    public float Compass, oldCompass, YPos, oldYpos;

    private void Update()
    {
        if (!initialized)
            return;

        Compass = Input.compass.magneticHeading - startXPos;
        YPos = (-Input.acceleration.y * 100) - startYPos;
        compassText.text = Input.compass.magneticHeading.ToString();

        if (Compass - oldCompass > 350)
            Compass = Compass - 360;

        if (Compass - oldCompass < -350)
            Compass = Compass + 360;

        if (YPos - oldYpos > 350)
            YPos = YPos - 360;

        if (YPos - oldYpos < -350)
            YPos = YPos + 360;

        oldCompass = Compass;
        oldYpos = YPos;

        var pos = new Vector3(Compass / 5, YPos / 5, 0);
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * 10);

        //compassText.text = (XPos - startXPos).ToString();
        //  ss.Send(new Vector2(PP.z, PP.y));

    }
    public Vector3 PP;

    //void Update()
    //{

    //	dir = Vector2.zero;

    //	/////////////
    //	Quaternion gyroRotation = Input.gyro.attitude;

    //	// ?????? ?????? ?????????????
    //	accelerometer = Input.gyro.attitude.eulerAngles;
    //	//accelerometer = Vector3.Lerp(accelerometer, accel, Time.deltaTime * 5.0f);

    //	// ?????? ??????? ??? ??????? ?????? ????????? ? ?????????????
    //	rotation = Quaternion.Slerp(rotation, gyroRotation, gyroWeight);

    //	Vector3 gyroRateUnbiased = rotation * Input.gyro.rotationRateUnbiased;

    //	dir.x = -gyroRateUnbiased.z;
    //	dir.y = -gyroRateUnbiased.x;
    //	/////////////

    //	//dir.x = -Input.gyro.rotationRateUnbiased.z;
    //	//dir.y = -Input.gyro.rotationRateUnbiased.x;

    //	var absX = Math.Abs(dir.x);
    //	var absY = Math.Abs(dir.y);

    //	if (absX < 0.02)
    //		dir.x = 0;

    //	if (absY < 0.02)
    //		dir.y = 0;

    //	dir /= 2;

    //	if (dir.x > 1.5)
    //		dir.x *= 0.5f;

    //	if (dir.x < -1.5)
    //		dir.x *= 0.5f;

    //	if (dir.y > 1.5)
    //		dir.y *= 0.5f;

    //	if (dir.y < -1.5)
    //		dir.y *= 0.5f;

    //	//if (Mathf.Abs(dir.x) > 1.5f || Mathf.Abs(dir.y) > 1.5f)
    //	//	Debug.Log("X: " + dir.x + "  Y: " + dir.y);
    //}
}
