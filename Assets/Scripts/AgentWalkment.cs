using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentWalkment : MonoBehaviour
{

    private Animator myAnimator;
    public GameObject target;
    public NavMeshAgent agent;
    private bool onDestination = false;
    


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        myAnimator = GetComponent<Animator>();
        target = GameObject.FindGameObjectWithTag(getCity(PhotonNetwork.playerName));

    }

    
    void Update()
    {
        
        if (Vector3.Distance(agent.destination, agent.transform.position) <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                
                myAnimator.SetFloat("VSpeed", 0);
                
            }


        }
    }


    //gibt true zurück wenn der Soldat sich nicht mehr bewegt
    //gibt false zurück wenn er sich noch bewegt
    public bool getOnDestination()
    {
        if(myAnimator.GetFloat("VSpeed") == 0)
        {
            
            return true;
        }
        else
        {
            return false;
        }
    }


    //lässt den Soldat zum Ziel losgehen
    public void startWalk()
    {

        target = GameObject.FindGameObjectWithTag(getCity(PhotonNetwork.playerName));
        myAnimator.SetFloat("VSpeed", 1);
        agent.SetDestination(target.transform.position);
    }

    //gibt anahdn des Bezirkes die Stadt zurück, zu der er laufen muss
    private string getCity(string tag)
    {
        switch (tag)
        {
            case ("Unterfranken"): return "Aschaffenburg";
            case ("Oberfranken"): return "Bamberg";
            case ("Mittelfranken"): return "Nürnberg";
            case ("Oberpfalz"): return "Weiden";
            case ("Oberbayern"): return "Ingolstadt";
            case ("Niederbayern"): return "Passau";
            case ("Schwaben"): return "Kempten";
            default: return "Kempten";
        }
    }
}
