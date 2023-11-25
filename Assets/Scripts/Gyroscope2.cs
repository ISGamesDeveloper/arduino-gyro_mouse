using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI; //required for Input.compass



public class Gyroscope2 : MonoBehaviour
{
    public Text compassText;

    private void Start()
    {
        Input.compass.enabled = true;
        Input.location.Start();
        Input.gyro.enabled = true;
    }

    private void Update()
    {
        compassText.text = Input.compass.magneticHeading.ToString();
        transform.position = new Vector3(Input.compass.magneticHeading, -Input.acceleration.y * 100);
    }
}