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

    public float XPos, YPos;

    public float startXPos, startYPos;

    void Start()
    {
        Input.compass.enabled = true;
        Input.location.Start();

        Input.gyro.enabled = true;
        Input.gyro.updateInterval = 0.1f;

        ResetButton.onClick.AddListener(delegate { RESET = true; ResetPosition(); });



        ResetPosition();
    }

    private async void ResetPosition()
    {
        initialized = false;
        await Task.Delay(1000);

        transform.position = Vector3.zero;


        startXPos = Input.compass.magneticHeading;
        startYPos = -Input.acceleration.y;

        if (startYPos > 200)
            startYPos = startYPos - 360;

        //   if (startXPos > 250)
        //     startXPos = startXPos - 360;

        await Task.Delay(1000);

        initialized = true;
    }

    public Quaternion accelerometer1;
    public Vector3 Compass, StartC;
    public Vector3 RotationR;

    private void Update()
    {
        if (!initialized)
            return;
        RotationR = Input.gyro.rotationRateUnbiased;
        XPos = Input.compass.magneticHeading;
        YPos = -Input.acceleration.y;


        if (YPos > 200)
            YPos = YPos - 360;

        //  if (XPos > 250)
        //    XPos = XPos - 360;
  

        var newYP = (YPos - startYPos) * 100;

        if (MathF.Abs(transform.position.y - newYP) < 1)
            newYP = transform.position.y;

        transform.position = Vector3.Lerp(transform.position, new Vector3(XPos - startXPos, newYP, 0) / 3, Time.deltaTime * 5);

        compassText.text = (XPos - startXPos).ToString();
        //  ss.Send(new Vector2(PP.z, PP.y));

    }
    public Vector3 PP;

    //void Update()
    //{

    //	dir = Vector2.zero;

    //	/////////////
    //	Quaternion gyroRotation = Input.gyro.attitude;

    //	// Чтение данных акселерометра
    //	accelerometer = Input.gyro.attitude.eulerAngles;
    //	//accelerometer = Vector3.Lerp(accelerometer, accel, Time.deltaTime * 5.0f);

    //	// Фильтр Калмана для слияния данных гироскопа и акселерометра
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
