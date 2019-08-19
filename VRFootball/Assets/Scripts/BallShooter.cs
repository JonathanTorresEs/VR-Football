using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour 
{

// Launch at target variables
    public float initialAngle = 0.0f;
    public GameObject ballTarget;
    public bool missedShot = false;
    public bool scoreOnce = false;

    public Rigidbody ballRigidbody;
    private Rigidbody handRigidbody;

    public Image[] arrows;
    public Transform[] arrowsPositions;

    private GameObject player;
    private BallManager ballManager;
    private OVRControllerInputs controllers;

// Holding in hand variables
    private GameObject hand;
    private bool heldInHand = false;
    public bool isthrowing = false;
    public bool grabR = false;
    public bool grabL = false;
    public bool prueba = true;
    float TimeBetweenInput = 2.0f;
    float timer = 0.0f;
    Vector3 PrevPos; 
    Vector3 NewPos; 
    Vector3 ObjVelocity;

    public SerialManager serialManager;

void Start () {
    player = GameObject.FindGameObjectWithTag("Player");
    ballManager = GameObject.FindGameObjectWithTag("BallManager").GetComponent<BallManager>();
    controllers = GameObject.FindGameObjectWithTag("BallManager").GetComponent<OVRControllerInputs>();
    PrevPos = transform.position;
    NewPos = transform.position;
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
    prueba = true;
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
        Debug.Log("jala");
        hand = other.gameObject;
        handRigidbody = hand.GetComponent<Rigidbody>();

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

private bool DetermineHandTrigger()
{
    switch(hand.name) {
            case "hand_left": 
        return grabL;
            case "hand_right": 
        return grabR;
            default: 
        return false;
    }
}

// TOSSING BALL FROM PLAYER HAND METHODS
//
//
//
//

private void Update()
{
    NewPos = transform.position;  // each frame track the new position
    ObjVelocity = (NewPos - PrevPos) / Time.fixedDeltaTime;  // velocity = dist/time
    PrevPos = NewPos;  // update position for next frame calculation
    timer += Time.deltaTime;
    grabR = controllers.holdingRightTrigger;
    grabL = controllers.holdingLeftTrigger;

    if(heldInHand && !isthrowing)
    {
        bool triggerIsPressed = DetermineHandTrigger();

    if (triggerIsPressed && prueba) 
    {
        transform.position = hand.transform.position;
        transform.rotation = hand.transform.rotation;
        foreach (Image image in arrows)
        {
            image.enabled = false;
        }
    } else if (timer > TimeBetweenInput && !isthrowing)
    {
        isthrowing = true;
        prueba = false;
        ballRigidbody.isKinematic = false;
        ballRigidbody.useGravity = true;
        ballRigidbody.velocity = ObjVelocity * 8;
        Debug.Log(ObjVelocity.magnitude * 8);
       
        timer = 0.0f;
    }
//new Vector3(-15, 3, 0);
    
}
  


    
}

    public void SetHoldingValue (bool value)
    {
        heldInHand = value;
    }
}