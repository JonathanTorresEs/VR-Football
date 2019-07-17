using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Threading;

public class SerialManager : MonoBehaviour {

    [SerializeField]
    private string comPort = "COM3";
    public bool oculusQuestBuild = false;
    public bool playWithSerial = false;
    public SerialPort serial;
    private UnityWebRequest www;

    public Text debugText;
    private float timer = 0.0f;

    // Use this for initialization
    void Start ()
    {
        // StartCoroutine(LatencyTest());

        if (oculusQuestBuild)
        {
            try
            {
                www = UnityWebRequest.Get("http://192.168.0.20:8000/chest/com1/4");
                debugText.text = "Success start!";
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

    IEnumerator ShakeVest()
    {
        www.useHttpContinue = false;
        www.SendWebRequest();
        Debug.Log("First shake!");

        yield return new WaitForSeconds(3.0f);
        www.Dispose();
        www = null;

        yield return new WaitForSeconds(1.0f);
        www = UnityWebRequest.Get("http://192.168.0.20:8000/chest/com1/4");
    }

    IEnumerator LatencyTest()
    {
        www = UnityWebRequest.Get("http://192.168.0.23:8000/chest/com1/4");
        www.useHttpContinue = false;
        Debug.Log("Starting test at " + timer);
        yield return new WaitForSeconds(3.0f);
        //Thread vestThread = new Thread(SendHTTPRequest);
        //vestThread.Start();
        www.SendWebRequest();
        Debug.Log("Shaken at " + timer);
    }

    void SendHTTPRequest()
    {
        Debug.Log("Shaken at " + timer);   
    }

    public void ActivateVRVest()
    {
        if (oculusQuestBuild)
        {
            StopAllCoroutines();
            try
            {
                StartCoroutine(ShakeVest());
                //Thread vestThread = new Thread(SendHTTPRequest);
                //vestThread.Start();
            } catch (System.Exception e)
            {
                Debug.Log(e);
                debugText.text = e.ToString();
            }
        } else if (playWithSerial && serial.IsOpen)
        {
            serial.Write("4");
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
    }
}
