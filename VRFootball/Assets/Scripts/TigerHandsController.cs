using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TigerHandsController : MonoBehaviour {

    // Animator variables
    private Animator handAnimator;

    // Controller variables
    private OVRControllerInputs controllers;

    public HandType thisHandType;

    public enum HandType
    {
        RIGHT,
        LEFT
    }

    public bool test = false;
	// Use this for initialization
	void Start () {
        controllers = GameObject.FindGameObjectWithTag("Controllers").GetComponent<OVRControllerInputs>();
        handAnimator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (thisHandType == HandType.RIGHT && test)
        {
            handAnimator.SetBool("isOpen", !controllers.holdingRightTrigger);
        }

        if (thisHandType == HandType.LEFT && test)
        {
            handAnimator.SetBool("isOpen", !controllers.holdingLeftTrigger);
        }
    }
}
