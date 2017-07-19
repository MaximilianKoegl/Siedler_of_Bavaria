using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkAnimation : MonoBehaviour {


    private Animator myAnimator;
    // Use this for initialization
    void Start () {
        myAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        myAnimator.SetFloat("VSpeed", 1);
    }

    /*public void AnimatePunch()
    {
        
        //play punch animation
        GetComponent<Animation>().Play("WK_heavy_infantry_05_combat_idle", PlayMode.StopAll);
        GetComponent<Animation>().wrapMode = WrapMode.Once;
        //GetComponent<Animation>()["WK_heavy_infantry_06_combat_walk"].speed = punchAnimSpeed;
    }*/
}
