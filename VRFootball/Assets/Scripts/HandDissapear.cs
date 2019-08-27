using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandDissapear : MonoBehaviour {

	public Material mat;
	private GameObject handRight;
	// Use this for initialization
	void Awake () {
	}
	
	// Update is called once per frame
	void Update () {

			handRight = GameObject.Find("hand_right_renderPart_0");

			if(handRight)
			{
				handRight.GetComponent<SkinnedMeshRenderer>().enabled = false;
				//Destroy(GetComponent<HandDissapear>());
			}
		
	}
}
