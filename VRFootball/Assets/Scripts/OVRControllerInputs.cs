using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OVRControllerInputs : MonoBehaviour {

    public bool holdingRightTrigger = false;
    public bool holdingLeftTrigger = false;
   
	// Use this for initialization
	void Start () {
        /* Transform pathToHandR = gameObject.transform.Find("hand_right_renderPart_0");
        SkinnedMeshRenderer RHModel = pathToHandR.GetComponent<SkinnedMeshRenderer>();
		RHModel.enabled = false;*/
	}
	
	// Update is called once per frame
	void Update () {

        if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > .1f)
        {
            holdingRightTrigger = true;
        }
        else
        {
            holdingRightTrigger = false;
        }

        if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > .1f)
        {
            holdingLeftTrigger = true;
        }
        else
        {
            holdingLeftTrigger = false;
        }

    }

}
