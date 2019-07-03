using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.UI;

public class SerialManager : MonoBehaviour {

    [SerializeField]
    private string comPort = "COM3";
    public bool oculusQuestBuild = false;
    public bool playWithSerial = false;
    public SerialPort serial;

    public Text debugText;

    // Use this for initialization
    void Start () {

        if (oculusQuestBuild)
        {
            try
            {
              
            } catch (System.Exception e)
            {
                debugText.text = e.ToString();
            }
        } else if (playWithSerial)
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

    private void Update()
    {

    }
}
