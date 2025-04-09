// using System;
using System.Collections;
using UnityEngine;


public class Enemy_Behaviour : MonoBehaviour
{
    [Header("GameObjects Settings")]
    public Game_Events game_Events;
    public GameObject player;
    public GameObject bullet;
    
    [Tooltip("Variavel que indica se o objeto foi instanciado por um boss ou nao. Default: False")]
    [SerializeField] private bool fromBoss = false;
    
    [SerializeField] private Boss_Behaviour boss_Behaviour;
    public bool invulnerable;

    [SerializeField] private GameObject explosionPrefab;
    

    [Header("Camera Settings")]
    public Camera mainCam;
    public float offset;
    [SerializeField]
    private float upBound;
    [SerializeField]
    private float downBound;
    [SerializeField]
    private float rightBound;
    [SerializeField]
    private float leftBound;
    public bool enterCamView;
    
    [Header("Enemy Settings")]
    public string enemyType;
    public int life = 1;
    public float speed;
    public int level;
    public int damage;

    [Header("FireRate Settings")]
    public float fireRate;
    public float thisFireRate;
    public float timer;

   

    public void resetTimerToShot(){
        timer = fireRate;
    }
    
    public void takeDamage(int damage){
        if(invulnerable){ return; }

        if ( (enemyType == "BossWeapon")){

            if( boss_Behaviour.getStealthMode() ){
                boss_Behaviour.activeStealth();
            }
        }

        life -= damage;
        StartCoroutine( AnimateTakingDamage() );
        CheckLife();
    }

    private IEnumerator AnimateTakingDamage(){
        SpriteRenderer sprite = gameObject.GetComponent<SpriteRenderer>();
        sprite.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sprite.color = Color.white;
    }

    public void CheckLife(){
        // Debug.Log("enemyType: "+ enemyType);
        // Debug.Log("enemy: "+ this.transform.gameObject.name);
        // Debug.Log("life: " + life);

        if(life <= 0){
            if ( (enemyType == "BossWeapon") && (gameObject.tag.Contains("Boss"))){
                boss_Behaviour.decreaseTurret();
            }

            if ( (enemyType == "Plane") || (enemyType == "Ground") ){
                // game_Events.enemyCountdown();
                // 1 - Fazer Event para Game_Events adicionar à barra de Enemys defeated
                // game_Events.OnEnemyDefeated?.Invoke();
                Game_Events.OnEnemyDefeated?.Invoke();
            }

            
            // 2- Fazer Event para Player
            player.GetComponent<Player_Behavior>().fillBombSpecial(.5f); 
            
            
            DropItem();
            DeathAnimation();
        }
    }

    private void DeathAnimation()
    {
            // 3 - Animação de morte
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        
    }

    public void setSprite(Sprite _sprite){
        transform.gameObject.GetComponent<SpriteRenderer>().sprite = _sprite;
    }

    private void DropItem(){
        if (fromBoss) return;

        int dropShield = Random.Range(0, 100);
        int dropUpgrade = Random.Range(0, 100);
        int dropLife = Random.Range(0, 100);
        
        // Debug.Log(dropUpgrade);
        
        if(dropLife <= game_Events.dropLifeRate){
            drop("Life");
        }else if(dropShield <= game_Events.dropShieldRate){
            drop("Shield");
        } else if(dropUpgrade <= game_Events.dropUpgradeRate){
            drop("Upgrade");
        }       
    }

    private void drop(string d){
         
        GameObject drop = Instantiate( game_Events.getDropGO(), transform.position, Quaternion.identity);
        drop.GetComponent<Drop_Behaviour>().setTagName(d);

        if (d == "Upgrade"){
            drop.GetComponent<Drop_Behaviour>().setSprite( game_Events.getUpgradeSprite()); 
        }else if (d == "Life"){
            drop.GetComponent<Drop_Behaviour>().setSprite( game_Events.getLifeSprite()); 
        }else {
            drop.GetComponent<Drop_Behaviour>().setSprite( game_Events.getShieldSprite()); 
        }
        
    }


    // REFERENTE AO BOSS
    public void setParentBoss(Boss_Behaviour boss_Behaviour){
        this.boss_Behaviour = boss_Behaviour;
    }
    public void setLifeByBoss(int life){
        this.life = life;
    }
    public void setFireRate(float fireRateToIncrease){
        this.fireRate /= fireRateToIncrease;
    }
    public void resetFireRate(){
        this.fireRate = this.thisFireRate;
    }

    public void setInvulnerable(bool invulnerable){
        this.invulnerable = invulnerable;
    }

    public void setFromBoss(){
        this.fromBoss = true;
    }

}
