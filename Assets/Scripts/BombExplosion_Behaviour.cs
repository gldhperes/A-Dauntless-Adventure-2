using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion_Behaviour : MonoBehaviour
{  

    [Tooltip("Definido pelo Player o dano")]
    [SerializeField] private int player_Damage;
    public float explosionTimeLife;
    public float explosionOffset;

    public void setDamage(int damage){
        this.player_Damage = damage;
    }

    void Update(){
        Destroy(this.gameObject, explosionTimeLife);
    }

    public void setScale(float explosionArea){
        
        this.transform.localScale = new Vector3(1, 1, 0) * (explosionOffset + explosionArea);
        
    }

    private void OnTriggerEnter2D(Collider2D col){
        
        if ( (col.gameObject.tag == "Enemy") ) {
            Debug.Log("inimigo");
            col.gameObject.GetComponent<Enemy_Behaviour>().takeDamage(player_Damage);
        }

    }

    
}