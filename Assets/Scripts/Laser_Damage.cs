using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Damage : MonoBehaviour
{
    [Tooltip("Definido pelo EnemyBulletBehaviour Script ")]
    [SerializeField] private int damage;

    void Start(){
        damage = this.gameObject.transform.parent.GetComponent<EnemyBulletBehaviour>().getDamage();
    }

    void OnTriggerEnter2D(Collider2D col){

        if (col.gameObject.tag == "Player"){
            // Chama a função de tirar vida do inimigo colidido
            try{
                col.gameObject.GetComponent<Player_Behavior>().takeDamage(damage);
            }catch{
                // Debug.Log("Bala do player");
            }
            
        }

    }
}
