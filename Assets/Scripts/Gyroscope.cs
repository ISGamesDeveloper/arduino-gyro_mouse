using System;
using UnityEngine;
using UnityEngine.UI;

public class Gyroscope : MonoBehaviour
{
	public float speed = 10;
	public ButtonExtender TouchPanel;
	public Button ResetButton;
	public Vector3 dir;
	public static bool RESET;

	void Start()
	{
		Input.gyro.enabled = true;
		ResetButton.onClick.AddListener(delegate { RESET = true; transform.position = Vector3.zero; });
	}

	void Update()
	{
		dir = Vector3.zero;
		dir.x = -Input.gyro.rotationRateUnbiased.z;
		dir.y = Input.gyro.rotationRateUnbiased.x;

		if (Math.Abs(dir.x) < 0.1)
			dir.x = 0;

		if (Math.Abs(dir.y) < 0.1)
			dir.y = 0;

		//if(Input.GetKey(KeyCode.W))
		//{
		//	dir.y = 0.2f;
		//}
		//if (Input.GetKey(KeyCode.S))
		//{
		//	dir.y = -0.2f;
		//}
		//if (Input.GetKey(KeyCode.D))
		//{
		//	dir.x = 0.2f;
		//}
		//if (Input.GetKey(KeyCode.A))
		//{
		//	dir.x = -0.2f;
		//}

		//if (Math.Abs(dir.z) < 0.2)
		//	dir.z = 0;

		// clamp acceleration vector to unit sphere
		//if (dir.sqrMagnitude > 1)
		//	dir.Normalize();

		// Make it move 10 meters per second instead of 10 meters per frame...
		dir *= Time.deltaTime;

		transform.Translate(dir * speed);
	}
}
