using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossArriving : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public GameObject boss;
    public GameObject player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = GameObject.FindWithTag("Boss");
        player = GameObject.FindWithTag("Player");
        player.GetComponent<Player_Behavior>().setCanShot(false);
    }

  
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss.GetComponent<Boss_Behaviour>().setBossArrived(true);
        player.GetComponent<Player_Behavior>().setCanShot(true);
        
    }

  
}
