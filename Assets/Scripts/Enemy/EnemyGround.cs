using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : Enemy_Behaviour
{
    private float angle;
    public bool isBossTurret;
    public bool isBossCannon;
    public bool bossArrived = false;

    void Start(){
        this.thisFireRate = this.fireRate;
        game_Events = GameObject.FindWithTag("Game_Events").GetComponent<Game_Events>();
        player = game_Events.getPlayer();
        bullet = game_Events.getEnemyBullet();        
        mainCam = game_Events.getMainCam();
        checkBossTurret();
        setStats();
        setCamPos();
    }

    private void setStats(){
        if(gameObject.transform.tag != "Enemy") return; 
        
        level = game_Events.getGameLevel();
        life *= level;
        fireRate = fireRate - (.05f * level);
    }

    private void checkBossTurret(){
        if (gameObject.transform.tag == "BossTurret"){
            isBossTurret = true;
        }else if (gameObject.transform.tag == "BossCannon"){
            isBossCannon = true;
        }
    }

    public void setBossArriveAnimation(bool b){
        this.bossArrived = b;
    }

    void Update()
    {
        enterCameraView();

        if (playerIsInBound ){
            if (isBossTurret && bossArrived) {
                timerToShot();
            } else if (isBossCannon && bossArrived) {
                thisRotation();
                timerToShot();
            } else if(gameObject.tag == "Enemy"){
                thisRotation();
                timerToShot();
            }         
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
        GameObject _bullet = Instantiate(bullet, this.transform.position, this.transform.rotation) as GameObject;
        _bullet.GetComponent<Bullet_Behaviour>().setGameObj(this.gameObject as GameObject);
        _bullet.transform.tag = this.transform.tag;
        _bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - offset - 180f));
    }

    private void thisRotation(){
        // Se player nao estiver vivo, entao retorna
        if ( !game_Events.playerIsAlive() ) return;

        Vector3 targetPos = player.transform.position;
        Vector3 thisPos = transform.position;
        targetPos.x = thisPos.x - targetPos.x;
        targetPos.y = thisPos.y - targetPos.y;
        
        // Pega o valor da Tangente e trasnforma em angulo
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        // Debug.Log("player: " + targetPos);
        // Debug.Log("enemy: " + thisPos);
        // Debug.Log("x: " + targetPos.x);
        // Debug.Log("y: " + targetPos.y);
        // Debug.Log("Tan: " + Mathf.Atan2(targetPos.y, targetPos.x) );
        // Debug.Log("Angle: " + angle);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - offset));
    }

    void OnTriggerEnter2D(Collider2D col){
        if ( (enemyType == "BossWeapon") && (col.gameObject.tag == "PlayerBomb")){
            takeDamage( player.GetComponent<Player_Behavior>().getDamage() );
        }
    }
}
