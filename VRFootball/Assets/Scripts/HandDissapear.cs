using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDissapear : MonoBehaviour {

	public Material mat;
	private GameObject handRight;
	// Use this for initialization
	void Start () {
		handRight = GameObject.Find ("hand_right_renderPart_0");
	}
	
	// Update is called once per frame
	void Update () {

		if(handRight != null)
		{
			Debug.Log("cambio de material");
			handRight.GetComponent<Renderer>().material = mat;
			Destroy(GetComponent<HandDissapear>());
		}
		
	}
}
