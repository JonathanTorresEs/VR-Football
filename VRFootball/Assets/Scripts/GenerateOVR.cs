using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateOVR : MonoBehaviour {

	private GameObject CameraRig;

	//lo primerito que va a pasar sionoraza
	void Awake()
	{
		GameObject ovrInstance = Instantiate(Resources.Load("OVRPlayerController", typeof (GameObject))) as GameObject;
	}

	// Use this for initialization
	void Start () {
		CameraRig = GameObject.Find("OVRCameraRig");
		OVRManager manager = CameraRig.gameObject.GetComponent<OVRManager>();
	

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
