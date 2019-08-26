using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {

    public float amplitude = 0.5f;
    public float frequency = 1f;

    // Position Storage Variables
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    public BallManager ballManager;
    public Text scoreText;
    public Text remainingShotsText;
    //public Text tirosAnotadosText;

    // Use this for initialization
    void Start () {
        posOffset = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;

        scoreText.text = ballManager.ballScore.ToString();
        remainingShotsText.text = ballManager.ballsRemaining.ToString();
        //tirosAnotadosText.text = ballManager.anotados.ToString();

    }
}
