using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour {

    // Launch at target variables
    public OVRInput.Controller controller;
    public float initialAngle = 0.0f;
    public GameObject ballTarget;
    public bool missedShot = false;
    public bool scoreOnce = false;

    public Rigidbody ballRigidbody;

    public Image[] arrows;
    public Transform[] arrowsPositions;

    private GameObject player;
    private BallManager ballManager;

    // Holding in hand variables
    private GameObject hand;
    private bool heldInHand = false;
    public bool isthrowing = false;
    public bool grabR = false;
    public bool grabL = false;

    public SerialManager serialManager;

    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        ballManager = GameObject.FindGameObjectWithTag("BallManager").GetComponent<BallManager>();
    }

    public void LookAtTarget()
    {
        Vector3 direction = ballTarget.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion lookAt = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f);
        transform.rotation = lookAt;
    }

    // Launch ball at distant target
    public void LaunchBall()
    {
        Vector3 p = ballTarget.transform.position;

        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = transform.position.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));

        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;

        foreach (Image image in arrows)
        {
            image.enabled = true;
        }

        // Fire!
        ballRigidbody.useGravity = true;
        ballRigidbody.velocity = finalVelocity;
        ballRigidbody.AddTorque(transform.forward * 20, ForceMode.VelocityChange);
    }


    // HOLDING BALL METHODS

    // When player catches ball, stop its velocity and torque,
    // disable its gravity and attach to hand
    private void OnTriggerEnter(Collider other)
    {
        if((other.gameObject.name == "hand_right" || other.gameObject.name == "hand_right" || 
            other.gameObject.name == "hand_left") && !missedShot && !scoreOnce && (grabR || grabL))
        {
            if (serialManager.playWithSerial || serialManager.oculusQuestBuild)
            {
                serialManager.ActivateVRVest();
            }

            ballRigidbody.useGravity = false;
            ballRigidbody.velocity = Vector3.zero;
            ballRigidbody.angularVelocity = Vector3.zero;

            heldInHand = true;
            hand = other.gameObject;

            ballManager.ballScore += 10;
            scoreOnce = true;

            foreach (Image image in arrows)
            {
                image.enabled = false;
            }
        }

        if (other.gameObject.tag == "BallTarget" && !heldInHand)
        {
            missedShot = true;
            foreach (Image image in arrows)
            {
                image.enabled = false;
            }
        }
        
    }

    private void Lanzamiento()
    {
    
    if(heldInHand) {
        switch(hand.name) {
                case "hand_left": 
            if (!grabL) {
                isthrowing = true;
                ballRigidbody.isKinematic = false;
                ballRigidbody.velocity = OVRInput.GetLocalControllerVelocity(controller);
                ballRigidbody.angularVelocity = OVRInput.GetLocalControllerAngularVelocity(controller);
            }
            break;
                case "hand_right": 
            if (!grabR) {
                isthrowing = true;
                ballRigidbody.isKinematic = false;
                ballRigidbody.velocity = OVRInput.GetLocalControllerVelocity(controller);
                ballRigidbody.angularVelocity = OVRInput.GetLocalControllerAngularVelocity(controller);
            }
            break;
        }
    }
}



    // TOSSING BALL FROM PLAYER HAND METHODS
    //
    //
    //
    //

    private void Update()
    {
        if(heldInHand && !isthrowing)
        {
            transform.position = hand.transform.position;
            transform.rotation = hand.transform.rotation;
            foreach (Image image in arrows)
            {
                image.enabled = false;
            }
        }

        if (OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > .1f)
        {
            grabR = true;
        }
        else
        {
            grabR = false;
        }

        if (OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > .1f)
        {
            grabL = true;
        }
        else
        {
            grabL = false;
        }
    }

    public void SetHoldingValue (bool value)
    {
        heldInHand = value;
    }

}