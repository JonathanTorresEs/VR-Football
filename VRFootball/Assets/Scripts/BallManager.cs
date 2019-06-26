﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour {

    public GameObject fakeBall;
    public MelvinManager melvin;
    public GameObject ball;
    public Transform initialBallPosition;
    public BallTarget[] ballTargets;
    public int ballScore = 0;

    // Ball variables
    private int ballsRemaining = 10;
    private BallShooter ballShooter;
    private BallTarget chosenTarget;

    // Use this for initialization
    void Start () {
        ballShooter = ball.GetComponent<BallShooter>();
        StartCoroutine(LaunchCoroutine());
    }

    // Update is called once per frame
    void Update () {
		
	}

    IEnumerator LaunchCoroutine()
    {
        while (ballsRemaining > 0)
        {
            ChooseRandomTarget();
            melvin.StartBallThrowAnimation();
            yield return new WaitForSeconds(5.4f);
            fakeBall.SetActive(false);
            ball.gameObject.SetActive(true);
            ballShooter.LookAtTarget();
            ballShooter.LaunchBall();
            chosenTarget.StartFlashingArrows();
            ballsRemaining--;
            yield return new WaitForSeconds(8f);
            chosenTarget.StopFlashingArrows();
            ResetBall();
        }
    }

    private void ResetBall()
    {
        fakeBall.SetActive(true);
        ball.transform.SetPositionAndRotation(initialBallPosition.transform.position, initialBallPosition.rotation);
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ballRigidbody.useGravity = false;
        ballShooter.SetHoldingValue(false);
        ballShooter.missedShot = false;
        ballShooter.scoreOnce = false;
        ball.gameObject.SetActive(false);
    }

    private void ChooseRandomTarget()
    {
        chosenTarget = ballTargets[Random.Range(0, 4)];
        ballShooter.ballTarget = chosenTarget.gameObject;
    }
}