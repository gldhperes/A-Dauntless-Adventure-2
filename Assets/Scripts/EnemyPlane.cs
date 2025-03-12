using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPlane : Enemy_Behaviour
{
    void Start(){
        game_Events = GameObject.FindWithTag("Game_Events").GetComponent<Game_Events>();
        player = game_Events.getPlayer();
        bullet = game_Events.getEnemyBullet();        
        mainCam = game_Events.getMainCam();
        setStats();
        setCamPos();
    }

    private void setStats(){
        if(gameObject.transform.tag != "Enemy") return; 

        level = game_Events.getGameLevel();
        life *= level;
        speed = speed + (.2f * level);
        fireRate = fireRate - (.1f * level);
        damage *= level;
    }

    void Update()
    {   
        
        move();
        enterCameraView();
        respawn();
        
        if (playerIsInBound){
            timerToShot();
            // move();
        }
    }

    private void timerToShot()
    {   
        if (timer > 0){
            timer -= Time.deltaTime;
        }else{
            resetTimerToShot();
            shot();
        }   
    }

    private void shot()
    {   
        GameObject _bullet = Instantiate(bullet, this.transform.position, this.transform.rotation ) as GameObject;
        _bullet.GetComponent<Bullet_Behaviour>().setGameObj(this.gameObject as GameObject);
        _bullet.GetComponent<Bullet_Behaviour>().setDamage(this.damage);
        _bullet.transform.tag = this.transform.tag;
        // _bullet.transform.SetParent(this.transform);
    }

    private void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == "Player"){
            try{
                player.GetComponent<Player_Behavior>().takeDamage(life);
                life = 0;
                checkLife();
            }catch{}
        }
    }
}
