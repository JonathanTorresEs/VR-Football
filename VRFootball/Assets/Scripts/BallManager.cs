using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallManager : MonoBehaviour {

    public GameObject fakeBall;
    public MelvinManager melvin;
    public GameObject bolafresa;
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
        ballShooter = bolafresa.GetComponent<BallShooter>();
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
            bolafresa.gameObject.SetActive(true);
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
        bolafresa.transform.SetPositionAndRotation(initialBallPosition.transform.position, initialBallPosition.rotation);
        Rigidbody ballRigidbody = bolafresa.GetComponent<Rigidbody>();
        ballRigidbody.useGravity = false;
        ballShooter.SetHoldingValue(false);
        ballShooter.missedShot = false;
        ballShooter.scoreOnce = false;
        bolafresa.gameObject.SetActive(false);
        ballShooter.isthrowing = false;
    }

    private void ChooseRandomTarget()
    {
        chosenTarget = ballTargets[Random.Range(0, 3)];
        ballShooter.ballTarget = chosenTarget.gameObject;
    }
}
