using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossDead : StateMachineBehaviour
{
    [Header ("Components")]
    public GameObject game_Events;
    public Game_Events events;

    public GameObject boss;
    public Boss_Behaviour bossBehaviour;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        game_Events = GameObject.FindWithTag("Game_Events");
        events = game_Events.GetComponent<Game_Events>();

        boss = events.getBossGO();
        bossBehaviour = boss.GetComponent<Boss_Behaviour>();

    }

    
  

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if ( !boss.transform.gameObject.name.Contains("Boss4") ){
            boss.transform.localScale = new Vector3(0.7f, 0.7f, 0.7f);
        }

        events.AnimMissionComplete(  boss.transform.position, bossBehaviour.getDropUpgradePoints());

        boss.SetActive(false);     
    }

  
}
