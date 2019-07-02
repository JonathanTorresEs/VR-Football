using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class SerialManager : MonoBehaviour {

    [SerializeField]
    private string comPort = "COM3";
    public bool playWithSerial = false;
    public SerialPort serial;

    // Use this for initialization
    void Start () {
        if (playWithSerial)
        {
            serial = new SerialPort(comPort, 9600);

            if (!serial.IsOpen)
            {
                try { serial.Open(); Debug.Log("Connected to serial!"); }
                catch (System.Exception e)
                {
                    Debug.Log(e);
                }
            }

            serial.Write("4");
        }
    }

    public void ActivateVRVest()
    {
        if (playWithSerial && serial.IsOpen)
        {
            serial.Write("4");
        }
    }

}
