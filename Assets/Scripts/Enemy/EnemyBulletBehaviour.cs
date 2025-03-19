using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletBehaviour : MonoBehaviour
{
    [Tooltip("Definifo somente pela bala do inimigo")]
    [SerializeField] private float bulletSpeed;

    [SerializeField] private int damage;

    public float lifeTime;


    // Update is called once per frame
    void Update()
    {
        if( !this.gameObject.GetComponent<Laser_Behaviour>() ) return;

        transform.Translate(Vector3.up * bulletSpeed, Space.Self);
        Destroy(this.gameObject, lifeTime);
    }

    public int getDamage(){
        return this.damage;
    }

    void OnTriggerEnter2D(Collider2D col){

        if (col.gameObject.tag == "Player"){
            // Chama a função de tirar vida do inimigo colidido
            Destroy(this.gameObject);
        }

    }
}
