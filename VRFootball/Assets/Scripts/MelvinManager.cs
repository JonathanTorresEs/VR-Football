﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelvinManager : MonoBehaviour {

    public Animator animator;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartBallThrowAnimation()
    {
        animator.SetTrigger("Throwing");
    }

}
