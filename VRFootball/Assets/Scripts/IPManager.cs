using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPManager : MonoBehaviour {

	// Quartet Variables
	private int currentIndex = 0;
	private string[] quartets;	

	[Header("IP Address")]
	public string ipAddress = "";
	
	// Controller Variables
	public GameObject controller;
	private LineRenderer controllerLine;
	private Ray shootRay = new Ray();
    private RaycastHit shootHit;
    private int shootableMask;


	// Use this for initialization
	void Start () {
		quartets = new string[4];
		controllerLine = controller.GetComponent<LineRenderer>();
		controllerLine.enabled = true;
		shootableMask = 0;
	}
	
	// Update is called once per frame
	void Update () {
		controllerLine.SetPosition(0, transform.position);
		shootRay.origin = transform.position;
		shootRay.direction = transform.forward;

		if(Physics.Raycast(shootRay, out shootHit, 400.0f, shootableMask)) {
			if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > .1f || 
			OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > .1f) {

				checkQuartetLength();

				if (shootHit.collider.tag == "Event") {
					switch (shootHit.collider.name) {
						case "Clear":
							clearQuartet(); break;
						case "Next":
							advanceCurrentIndex(); break;
						case "Back": 
							decreaseCurrentIndex(); break;
						case "Enter":
							saveIpAddress(); break;
					}
				} else {
					switch (shootHit.collider.name) {
						case "1":
							addNumberToQuartet("1"); break;
						case "2":
							addNumberToQuartet("2"); break;
						case "3":
							addNumberToQuartet("3"); break;
						case "4":
							addNumberToQuartet("4"); break;
						case "5":
							addNumberToQuartet("5"); break;
						case "6":
							addNumberToQuartet("6"); break;
						case "7":
							addNumberToQuartet("7"); break;
						case "8":
							addNumberToQuartet("8"); break;
						case "9":
							addNumberToQuartet("9"); break;
						case "0":
							addNumberToQuartet("0"); break;
						}
				}
			}
		}

	}

	private void addNumberToQuartet(string number) {
		quartets[currentIndex] = quartets[currentIndex] + number;
	}

	private void clearQuartet() {
		quartets[currentIndex] = "";
	}

	private void advanceCurrentIndex() {
		if (currentIndex < 3)
			currentIndex++;
		else
			currentIndex = 0;
	}

	private void decreaseCurrentIndex() {
		if (currentIndex > 0)
			currentIndex--;
		else
			currentIndex = 3;
	}
	private void checkQuartetLength() {
		if (quartets[currentIndex].Length >= 3 || currentIndex < 3)
		{
			advanceCurrentIndex();
		}
	}

	private void saveIpAddress() {
		for(int i = 0; i < ipAddress.Length; i++) {
			if (quartets[i] == "")
				quartets[i] = "0";
		}
		ipAddress = quartets[0] + '.' + quartets[1] + '.' + quartets[2] + '.' + quartets[3];
	}

}
