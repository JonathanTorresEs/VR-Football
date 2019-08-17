using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IPManager : MonoBehaviour {

	// Quartet Variables
	private int currentIndex = 0;
	private string[] quartets;	

	[Header("IP Address")]
	public string ipAddress = "";
	public GameObject ipObject;
	
	// Controller and Line Variables
	public GameObject controller;
	private LineRenderer controllerLine;
	private Ray shootRay = new Ray();
    private RaycastHit shootHit;
	private Material defaultMaterial;
	private Material selectMaterial;
    private int shootableMask;
	private float range = 20.0f;

	// Event variables
	private float timer = 0.0f;
	private float timeBetweenEvents = 0.3f;

	// UI variables
	[Header("UI Variables")]
	public Text[] quartetText;
	public Image[] selectors;

	void Start () {
		quartets = new string[4];

		for (int i = 0; i < 4; i++) {
			quartets[i] = "";
		}

		controllerLine = controller.GetComponent<LineRenderer>();
		controllerLine.enabled = true;
		shootableMask = 5;
		defaultMaterial = Resources.Load("line_default", typeof(Material)) as Material;
		selectMaterial = Resources.Load("line_select", typeof(Material)) as Material;
		displayCurrentSelector();
	}
	
	void Update () {

		timer += Time.deltaTime;

		controllerLine.SetPosition(0, controller.transform.position);
		shootRay.origin = controller.transform.position;
		shootRay.direction = controller.transform.forward;

		//bool anyTriggerPressed = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > .1f || OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > .1f ? true : false;
		bool anyTriggerPressed = OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > .1f ? true : false;

		if(anyTriggerPressed)
			controllerLine.material = selectMaterial;
		else
			controllerLine.material = defaultMaterial;

		if(Physics.Raycast(shootRay, out shootHit, range, shootableMask) && anyTriggerPressed && timer > timeBetweenEvents) {
				timer = 0.0f;

				if (shootHit.collider.tag == "Event") {
					switch (shootHit.collider.name) {
						case "Clear":
							clearAllQuartets(); break;
						case "Next":
							advanceCurrentIndex(); break;
						case "Back": 
							decreaseCurrentIndex(); break;
						case "Backspace": 
							backspace(); break;
						case "Enter":
							saveIpAddress(); break;
						case "Play":
							loadMainGame(); break;
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
			controllerLine.SetPosition(1, shootHit.point);
		} else {
				controllerLine.SetPosition(1, shootRay.origin + shootRay.direction * range);
			}
	}

	//
	//	DATA QUARTETS METHODS
	//

	private void addNumberToQuartet(string number) {
		checkQuartetLength();
		if(quartets[currentIndex].Length < 3) {
			quartets[currentIndex] = quartets[currentIndex] + number;
		}
		updateSingleQuartet(quartets[currentIndex]);
	}

	private void backspace() {
		if(quartets[currentIndex].Length > 0) {
			quartets[currentIndex] = quartets[currentIndex].Remove(quartets[currentIndex].Length - 1);
			updateSingleQuartet(quartets[currentIndex]);
		}
	}

	private void clearAllQuartets() {
		ipAddress = "";
		for (int i = 0; i < 4; i++) {
			quartets[i] = "";
		}
		currentIndex = 0;
		clearAllQuartetText();
		displayCurrentSelector();
	}

	private void advanceCurrentIndex() {
		if (currentIndex < 3)
			currentIndex++;
		else
			currentIndex = 0;

			displayCurrentSelector();
	}

	private void decreaseCurrentIndex() {
		if (currentIndex > 0)
			currentIndex--;
		else
			currentIndex = 3;
	
			displayCurrentSelector();
	}
	private void checkQuartetLength() {
		if (quartets[currentIndex].Length >= 3)
		{
			advanceCurrentIndex();
		}
	}

	private void saveIpAddress() {

		ipAddress = "";

		for(int i = 0; i < quartets.Length; i++) {
			if (quartets[i] == "")
				quartets[i] = "0";
		}
		ipAddress = quartets[0] + '.' + quartets[1] + '.' + quartets[2] + '.' + quartets[3];
	}

	//
	//	DISPLAY DATA METHODS
	//

	private void updateSingleQuartet(string quartet) {
		quartetText[currentIndex].text = quartet;
	}

	private void displayCurrentSelector() {

		for(int i = 0; i < selectors.Length; i++) {
			selectors[i].enabled = false;
		}

		selectors[currentIndex].enabled = true;
	}

	private void clearAllQuartetText() {
		for(int i = 0; i < quartetText.Length; i++) {
			quartetText[i].text = "";
		}
	}

	private void loadMainGame() {
		Text ipObjectText = ipObject.GetComponent<Text>();
		ipObjectText.text = ipAddress;
		DontDestroyOnLoad(ipObject);
		SceneManager.LoadScene("Main");
	}

}
