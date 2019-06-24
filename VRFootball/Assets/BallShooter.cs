using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallShooter : MonoBehaviour {

    // Launch variables
    public float initialAngle = 0.0f;
    public GameObject target;
    private Rigidbody rigid;

    public Image[] arrows;
    private GameObject player;

    // Hand variables
    private GameObject hand;
    private bool heldInHand = false;

    // Controller variables
    private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;


    // Use this for initialization
    void Start () {

        rigid = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");

        LookAtTarget();
        LaunchBall();
    }

    // Make ball face to target
    private void LookAtTarget()
    {
        Vector3 direction = target.transform.position - transform.position;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        Quaternion lookAt = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f);
        transform.rotation = lookAt;
    }

    private void LaunchBall()
    {
        Vector3 p = target.transform.position;

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

        // Fire!
        rigid.velocity = finalVelocity;
        rigid.AddTorque(transform.forward * 20, ForceMode.VelocityChange);
    }


    // When player catches ball, stop its velocity and torque,
    // disable its gravity and attach to hand
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "hand_right")
        {
            rigid.useGravity = false;
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            gameObject.transform.SetParent(other.transform);

            heldInHand = true;
            hand = other.gameObject;
        }
    }

    private void Update()
    {
        if(heldInHand)
        {
            transform.position = hand.transform.position;
            transform.rotation = hand.transform.rotation;
        } else
        {
            Vector3 lookVector = player.transform.position - transform.position;
            lookVector.x = lookVector.z = 0.0f;
            foreach(Image image in arrows)
            {
                image.transform.LookAt(player.transform.position - lookVector);
                image.transform.Rotate(0, 180, 0);
            }
        }
    }

}