using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScript : MonoBehaviour {


	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	private void OnTriggerEnter(Collider other)
	{
		if(other.name == "hand_right" || other.name == "hand_left")
		{
			SceneManager.LoadScene(0);
			DontDestroyOnLoad(this);
		}
	
	}
}
