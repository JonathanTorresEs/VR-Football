using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallTarget : MonoBehaviour {

    public Image[] arrows;
    private float timer = 0.0f;
    private bool mustFlash = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += (Time.deltaTime * .8f);
       
        if(mustFlash)
        {
            foreach (Image arrow in arrows)
            {
                arrow.fillAmount = timer;
            }
        }

        if(timer > 1)
        {
            timer = 0;
        }
	}

    public void StartFlashingArrows()
    {
        mustFlash = true;
        foreach (Image arrow in arrows)
        {
            arrow.gameObject.SetActive(true);
        }
    }

    public void StopFlashingArrows()
    {
        mustFlash = false;
        foreach (Image arrow in arrows)
        {
            arrow.gameObject.SetActive(false);
        }
    }

}
