using System;
using UnityEngine;
using UnityEngine.UI;

public class Gyroscope : MonoBehaviour
{
	public float speed = 10;
	public ButtonExtender TouchPanel;
	public Button ResetButton;
	public Vector2 dir;
	public static bool RESET;

	void Start()
	{
		Input.gyro.enabled = true;
		ResetButton.onClick.AddListener(delegate { RESET = true; /*transform.position = Vector3.zero;*/ });

		Debug.Log("INTERVAL: " + Input.gyro.updateInterval);
		Input.gyro.updateInterval = 0.01f;

		Debug.Log("INTERVAL: " + Input.gyro.updateInterval);
	}

	void Update()
	{
		dir = Vector2.zero;
		//dir.x = -Input.gyro.rotationRate.z;
		//dir.y = -Input.gyro.rotationRate.x;

		dir.x = -Input.gyro.rotationRateUnbiased.z;
		dir.y = -Input.gyro.rotationRateUnbiased.x;

		//dir.x = -Input.gyro.attitude.z;
		//dir.y = -Input.gyro.attitude.x;

		var absX = Math.Abs(dir.x);
		var absY = Math.Abs(dir.y);

		if (absX < 0.02/* || absX > 2*/)
			dir.x = 0;

		if (absY < 0.02 /*|| absY > 2*/)
			dir.y = 0;

		Debug.Log("X: " + dir.x + "  Y: " + dir.y);

		// Make it move 10 meters per second instead of 10 meters per frame...
		//dir *= Time.deltaTime;

		//transform.Translate(dir * speed);
	}
}
