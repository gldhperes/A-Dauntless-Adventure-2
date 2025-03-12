using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletBehaviour : MonoBehaviour
{
    public Bullet_Behaviour playerBullet;
    public int damage;

    void Start(){
        playerBullet = transform.gameObject.GetComponent<Bullet_Behaviour>();
        this.damage = playerBullet.getDamage();
    }

    private void bulletHit(){
        playerBullet.bulletHit();
    }

    void OnTriggerEnter2D(Collider2D col){
        if ( (col.gameObject.tag == "BossCannon") || (col.gameObject.tag == "BossTurret") ){
            col.gameObject.GetComponent<Enemy_Behaviour>().takeDamage(this.damage);
            bulletHit();
            Destroy(this.transform.gameObject);
        }
        if ( (col.gameObject.tag == "B3Cannon") ){
            col.gameObject.GetComponent<Boss3Cannon>().takeDamage(this.damage);
            bulletHit();
            Destroy(this.transform.gameObject);
        }
    }
}
