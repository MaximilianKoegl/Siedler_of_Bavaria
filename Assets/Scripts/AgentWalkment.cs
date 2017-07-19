using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentWalkment : MonoBehaviour {

    private Animator myAnimator;
    public GameObject target;
    public NavMeshAgent agent;
	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        myAnimator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {

        //myAnimator.SetFloat("VSpeed", 1);
        //agent.SetDestination(target.transform.position);
        
    }

    
}
