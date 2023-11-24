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
    }

    private void Update()
    {
        compassText.text = Input.compass.magneticHeading.ToString();
    }
}