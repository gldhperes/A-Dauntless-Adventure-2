using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser_Behaviour : EnemyBulletBehaviour
{
    [SerializeField] private Boss3Cannon boss3Cannon;
    [SerializeField] private float lifeTimeAux;

    void Start(){
        lifeTimeAux = lifeTime;
    }
    void Update()
    {
        checkLifeTime();
    }

    private void checkLifeTime(){
        lifeTimeAux -= Time.deltaTime;

        if(lifeTimeAux <= 0){
            boss3Cannon.resetShot();
            Destroy(this.gameObject);
        }
    }

    public void setBossCannon(GameObject bossCannon){
        this.boss3Cannon = bossCannon.GetComponent<Boss3Cannon>();
    }

}
