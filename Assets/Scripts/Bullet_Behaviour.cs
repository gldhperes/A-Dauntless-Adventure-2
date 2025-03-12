using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Behaviour : MonoBehaviour
{   
    public Game_Events game_Events;
    public GameObject gameObj;
    private Camera mainCam;
    public GameObject bulletHitGO;
    public float bulletSpeed;
    public float lifeTime;
    public int damage = 1;


    public void setGameObj(GameObject obj){
        gameObj = obj;
    }
   
    void Start(){
        game_Events = GameObject.FindWithTag("Game_Events").GetComponent<Game_Events>();
        bulletHitGO = game_Events.getBulletHitGO();
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update() {
        transform.Translate(Vector3.up * bulletSpeed * Time.deltaTime, Space.Self);
        Destroy(this.gameObject, lifeTime);

        float upBound = (mainCam.transform.position.y + mainCam.orthographicSize) ;
        if (this.gameObject.transform.position.y >= upBound){
            Destroy(this.gameObject);
        }
    }

    public void setDamage(int dmg){
        this.damage = dmg;
    }

    public int getDamage(){
        return this.damage;
    }

    public void bulletHit(){
        GameObject bulletHit = Instantiate(bulletHitGO, gameObject.transform.position, Quaternion.identity);
        Destroy(bulletHit, game_Events.getBulletHitTime());
    }

    void OnTriggerEnter2D(Collider2D col){
        // Debug.Log("my obj: " + gameObj);
        // Debug.Log("col.gameObject: " + col.gameObject);
        // Debug.Log("tag: " + gameObj.tag);

        if ( (col.gameObject.tag == "Enemy") && ( col.gameObject.tag != this.gameObject.transform.tag) && ( !this.gameObject.transform.tag.Contains("Boss") ) ) {
            // Chama a função de tirar vida do inimigo colidido
            col.gameObject.GetComponent<Enemy_Behaviour>().takeDamage(damage);
            // Debug.Log("Bateu Enemy:"+ col.transform.gameObject.name);
            bulletHit();
            Destroy(this.transform.gameObject);
        }

        if ( (col.gameObject.tag == "Player")  && ( col.gameObject.tag != this.gameObject.transform.tag)  ){
            // Chama a função de tirar vida do inimigo colidido
            col.gameObject.GetComponent<Player_Behavior>().takeDamage(damage);
            // Debug.Log("Bateu Player");
            bulletHit();
            Destroy(this.transform.gameObject);
        }

 
    }
}
