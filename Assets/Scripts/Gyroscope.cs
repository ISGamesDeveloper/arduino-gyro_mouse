using System;
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

	public Vector3 OldAttitude;
	public Vector3 CoeffAttitude;

	void Start()
	{
		Input.compass.enabled = true;
		Input.gyro.enabled = true;
		ResetButton.onClick.AddListener(delegate { RESET = true; /*transform.position = Vector3.zero;*/ });

		Debug.Log("INTERVAL: " + Input.gyro.updateInterval);
		Input.gyro.updateInterval = 0.0001f;

		OldAttitude = Input.gyro.attitude.eulerAngles;
	}

	void CalibrateGyroscope()
	{
		int calibrateCount = 500;
		for (int i = 0; i < calibrateCount; i++)
		{
			rotation = Quaternion.Slerp(rotation, Input.gyro.attitude, 0.1f);
		}
	}

	public Vector3 pPos = Vector3.zero;

	public Quaternion accelerometer1;
	public Vector3 coeff;

	Quaternion rotation;
	private float gyroWeight = 0.98f;
	public Vector3 Compass;

	private void Update()
	{
		Compass = Input.gyro.rotationRate;
		accelerometer1 = Input.gyro.attitude;
		var euler = new Vector3(accelerometer1.x + 100, accelerometer1.y + 10, accelerometer1.z + 100);

		coeff = OldAttitude - euler;
		var m = 0.005f;


		if (Mathf.Abs(coeff.x) > 300)
		{
			coeff.x = 0;
		}

		if (Mathf.Abs(coeff.y) > 300)
		{
			coeff.y = 0;
		}

		if (Mathf.Abs(coeff.z) > 300)
		{
			coeff.z = 0;
		}

		if (Mathf.Abs(coeff.x) > m)
		{
			CoeffAttitude.x = coeff.x;
		}

		if (Mathf.Abs(coeff.y) > m)
		{
			CoeffAttitude.y = coeff.y;
		}

		if (Mathf.Abs(coeff.z) > m)
		{
			CoeffAttitude.z = coeff.z;
		}

		OldAttitude = euler;

		dir = new Vector2(CoeffAttitude.z, CoeffAttitude.x) * 100;
		ss.Send(dir);
		CoeffAttitude = Vector3.zero;


	}


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
