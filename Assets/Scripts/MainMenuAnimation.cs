// using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAnimation : MonoBehaviour
{
    [Header("Enemy Settings")]
    public GameObject enemyPlaneGO;
    public GameObject respawPlaneEnemys;
    public GameObject destroyPlaneEnemys;
    public List<GameObject> enemyClones;
    public float enemySpeed;
    public float timerToRespawn;
    private float timer;
    public int maxEnemysInScene;
    public int enemysInScene = 0;
    public Sprite[] planeEnemysLVL1;
    public Sprite[] planeEnemysLVL2;
    public Sprite[] planeEnemysLVL3;

    void Update(){
        remainingEnemys();
        moveEnemys();
        // destroyEnemys();
    }

    private void destroyEnemys(GameObject enemy)
    {
        enemysInScene -= 1;
        enemyClones.Remove(enemy);
        Destroy(enemy);
    }

    private void moveEnemys(){
        // Checks if the array is not empty
        if (enemyClones.Count != 0){
            try{
                foreach(GameObject enemy in enemyClones){
                enemy.gameObject.transform.Translate( new Vector2( 0, enemySpeed ) * Time.deltaTime, Space.Self );

                    if(enemy.transform.position.y <= destroyPlaneEnemys.transform.position.y){
                        destroyEnemys(enemy);
                    }
                }
            }catch{
               
            }
        }
    }

    private void remainingEnemys(){
        if(enemysInScene < maxEnemysInScene){
            timerToRespawnEnemys();    
        }
    }

    private void timerToRespawnEnemys(){   
        if (timer > 0){
            timer -= Time.deltaTime;
        }else{
            resetTimerToRespawn();
            setEnemys();
        }   
    }

    private void resetTimerToRespawn(){
        timer = timerToRespawn;
    }

    private void setEnemys(){
        float respawnSize = respawPlaneEnemys.transform.localScale.x / 2;

        GameObject enemyPlane = Instantiate(enemyPlaneGO, new Vector3(Random.Range( -respawnSize, respawnSize ), respawPlaneEnemys.transform.position.y, 0), new Quaternion(0, 0 , 180, 0)) as GameObject;

        int randomLlv = Random.Range(1, 3);
        switch(randomLlv){
            case 1:
                enemyPlane.GetComponent<SpriteRenderer>().sprite = planeEnemysLVL1[ Random.Range(0, planeEnemysLVL1.Length)];
                break;
            case 2:
                enemyPlane.GetComponent<SpriteRenderer>().sprite = planeEnemysLVL2[ Random.Range(0, planeEnemysLVL2.Length)];
                break;
            case 3:
                enemyPlane.GetComponent<SpriteRenderer>().sprite = planeEnemysLVL3[ Random.Range(0, planeEnemysLVL3.Length)];
                break;
        }
        
        // enemyB.moveSample(enemyPlane);

        enemyClones.Add(enemyPlane);
        // enemyPlane.gameObject.transform.Translate( new Vector2( 0, enemySpeed ) * Time.deltaTime, Space.Self );
        enemysInScene += 1;        
    }
}
