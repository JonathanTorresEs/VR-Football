using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallManager : MonoBehaviour {

    public GameObject fakeBall;
    public MelvinManager melvin;
    public GameObject ball;
    public Transform initialBallPosition;
    public BallTarget[] ballTargets;
    public int ballScore = 0;

    // Ball variables
    public int ballsRemaining = 10;
    private BallShooter ballShooter;
    private BallTarget chosenTarget;
    public int anotados = 0;

    // Use this for initialization
    void Start () {
        ballShooter = ball.GetComponent<BallShooter>();
        StartCoroutine(LaunchCoroutine());
    }

    // Update is called once per frame
    void Update () {
		
	}

    public void corut()
    {
        StopCoroutine(LaunchCoroutine());
        StartCoroutine(LaunchCoroutine());
    }


    public IEnumerator LaunchCoroutine()
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
            yield return new WaitForSeconds(10.0f);
            chosenTarget.StopFlashingArrows();
            ResetBall();
        }
    }

    public void ResetBall()
    {
        fakeBall.SetActive(true);
        ball.transform.SetPositionAndRotation(initialBallPosition.transform.position, initialBallPosition.rotation);
        Rigidbody ballRigidbody = ball.GetComponent<Rigidbody>();
        ballRigidbody.useGravity = false;
        ballShooter.SetHoldingValue(false);
        ballShooter.missedShot = false;
        ballShooter.scoreOnce = false;
        ball.gameObject.SetActive(false);
        ballShooter.isthrowing = false;
    }

    private void ChooseRandomTarget()
    {
        chosenTarget = ballTargets[Random.Range(0, 3)];
        ballShooter.ballTarget = chosenTarget.gameObject;
    }
}
