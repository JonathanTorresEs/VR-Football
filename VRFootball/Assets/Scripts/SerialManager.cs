using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Threading;
using System.Net;
using System.Text;
using System.IO;

public class SerialManager : MonoBehaviour {

    [SerializeField]
    private string comPort = "COM3";
    public bool oculusQuestBuild = false;
    public bool playWithSerial = false;
    public SerialPort serial;
    private UnityWebRequest www;

    public Text debugText;
    private float timer = 0.0f;
    private string url = "";
    public string ipAddress = "192.168.0.20:8000";

    // Use this for initialization
    void Start ()
    {
        if (oculusQuestBuild)
        {
            try
            {
                url = string.Concat("http://", ipAddress, "/chest/com1/4");
                SendHTTPRequest();
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
        yield return new WaitForSeconds(3.0f);
        /*
        www.useHttpContinue = false;
        www.SendWebRequest();
        Debug.Log("First shake!");

        yield return new WaitForSeconds(3.0f);
        www.Dispose();
        www = null;

        yield return new WaitForSeconds(1.0f);
        www = UnityWebRequest.Get("http://192.168.0.20:8000/chest/com1/4");*/
    }

    void SendHTTPRequest()
    {
        Debug.Log("Shaken at " + timer);
        ThreadPool.QueueUserWorkItem(new WaitCallback(HttpShakeVest));
    }

    void HttpShakeVest(object a)
    {
        string result = "";
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Proxy = null;

        using(HttpWebResponse response = (HttpWebResponse)request.GetResponse())
        {
            var encoding = Encoding.GetEncoding(response.CharacterSet);

            using (var responseStream = response.GetResponseStream())
            using (var reader = new StreamReader(responseStream, encoding))
                result = reader.ReadToEnd();
        }

        debugText.text = result;

    }

    public void ActivateVRVest()
    {
        if (oculusQuestBuild)
        {
            StopAllCoroutines();
            try
            {
                SendHTTPRequest();
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
