using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop_Behaviour : MonoBehaviour
{
    public string tagName;
    public float speed;
    private bool followPlayer = false;
    private GameObject player;
    public GameObject dropSound;
    public bool catchable = true;

    void Update(){
        move();
    }
    
    public void move(){
        if(followPlayer){
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, (player.GetComponent<Player_Behavior>().getSpeed() + speed) * Time.deltaTime );
        }else{
            transform.Translate( new Vector2( 0, -speed ) * Time.deltaTime, Space.Self );
        }

    }

    public void setFollowPlayer( GameObject player ){
        this.player = player;
        followPlayer = true;
    }

    public void setTagName(string name){
        tagName = name;
    }

    public void setSpeed(float speed){
        this.speed = speed;
    }


    public string getTagName(){
        return tagName;
    }

    public float getSpeed(){
        return this.speed;
    }

    public void setSprite(Sprite _sprite){
        transform.gameObject.GetComponent<SpriteRenderer>().sprite = _sprite;
    }

    void OnTriggerEnter2D(Collider2D col){
        
        if (col.gameObject.tag == "Player" && catchable){
            if (this.tagName == "Upgrade"){
                col.gameObject.GetComponent<Player_Behavior>().gainUpgradePoints();

                if(followPlayer){
                    col.gameObject.GetComponent<Player_Behavior>().UpgradeReceived();
                }
            }

            if (this.tagName == "Life"){
                col.gameObject.GetComponent<Player_Behavior>().gainLife();
            }

            if (this.tagName == "Shield"){
                col.gameObject.GetComponent<Player_Behavior>().gainShield();
            }

            Instantiate(dropSound);
            Destroy(this.transform.gameObject);
        }
    }
}
