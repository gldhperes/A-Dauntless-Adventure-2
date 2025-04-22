using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossDropUpgradePoints : StateMachineBehaviour
{
    [Header ("Game Events")]
    public GameObject game_Events;
    public Game_Events events;

    [Header ("Boss Behaviour")]
    public GameObject boss;
    public Boss_Behaviour bossBehaviour;
    public int dropUpgradePoints;
    public int upgradeDropsCount = 0;
    public float timeForDrop;
    public float auxTimeForDrop;

    [Header ("Upgrade Drop")]
    public GameObject dropGO;
    Drop_Behaviour drop;
    public List<GameObject> dropsGO;
    private float dropOriginalSpeed;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {   
        game_Events = GameObject.FindWithTag("Game_Events");
        events = game_Events.GetComponent<Game_Events>();

        boss = events.getBossGO();
        bossBehaviour = boss.GetComponent<Boss_Behaviour>();

        dropUpgradePoints = bossBehaviour.getDropUpgradePoints();
        timeForDrop = bossBehaviour.getTimeForDrop();
        auxTimeForDrop = timeForDrop;

        dropGO = events.GetDropGO();
        drop = dropGO.GetComponent<Drop_Behaviour>();
        dropOriginalSpeed = drop.getSpeed();
        drop.setSpeed(0);
        drop.setTagName("Upgrade");
        drop.setSprite( events.getUpgradeSprite() );
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        timeForDrop -= Time.deltaTime;

        if (timeForDrop <= 0){
            timeForDrop = auxTimeForDrop;
            upgradeDropsCount += 1;

            if(upgradeDropsCount >= dropUpgradePoints){
                upgradeDropsCount = 0;

                // seta dropUpgrade false para ser chamada a função de OnStateExit
                animator.SetBool("dropUpgrade", false);
                
                return;
            }

            dropUpgrade();
            
        }

    }



    private void dropUpgrade(){
        float random = Random.Range( -bossBehaviour.getDropAreaOffset(), bossBehaviour.getDropAreaOffset() );
        Vector2 pos = new Vector2(boss.transform.position.x + random, boss.transform.position.y + random);
    
        GameObject d = Instantiate(dropGO, pos, Quaternion.identity);

        dropsGO.Add(d);
    }



    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Debug.Log("Drop Upgrade Points From BossDropUpgradePoints");
        GameObject playerGO = events.getPlayer();
        Player_Behavior player = playerGO.GetComponent<Player_Behavior>();

        
        // player.receiveUpgradeList(dropsGO, dropUpgradePoints);

        foreach (GameObject d in dropsGO) {
            d.GetComponent<Drop_Behaviour>().setSpeed(dropOriginalSpeed);
            d.GetComponent<Drop_Behaviour>().setFollowPlayer(playerGO);
        }

        Destroy(boss);

    }



    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
