using System.Collections;
using UnityEngine;

public class Boss3Cannon : MonoBehaviour
{   
    [Header("Default Settings")]
    [Tooltip("Definido por boss")]
    [SerializeField] private Game_Events game_Events;

    [Tooltip("Definido por Game_Events")]
    [SerializeField] private Boss_Behaviour boss;

    [Tooltip("Definido pelo Boss")]
    [SerializeField] private string bossName;

    [Tooltip("Definido por Game_Events")]
    [SerializeField] private Transform player;

    [Tooltip("Definido atraves de calculos")]
    [SerializeField] private float angle;

    [Tooltip("Define o ajuste do angulo")]
    [SerializeField] private float offset;

    [Header("Life Settings")]
    [Tooltip("Define a vida do objeto")]
    [SerializeField] private int life;

    [Tooltip("Define se o boss esta na animação de chegada")]
    [SerializeField] private bool bossArrived;

    [Tooltip("Definido pelo Boss se o objeto ficara imune a ataques")]
    [SerializeField] private bool invulnerable;




    
    [Header("Shot Settings")]
    [Tooltip("Definido o Prefab do Laser")]
    [SerializeField] private GameObject laserPrefab; 

    [Tooltip("Definido o Objeto de localização de onde os tiros sairão")]
    [SerializeField] private Transform shotTransform;

    [Tooltip("Define o tempo para atirar")]
    [SerializeField] private float timeToShot;

    [Tooltip("Definido por timeToShot")]
    [SerializeField] private float timeToShotAux;

    [Tooltip("Define o tempo atirando o laser")]
    [SerializeField] private float timeShoting;

    [Tooltip("Definido por timeShoting")]
    [SerializeField] private float timeShotingAux;

    [Tooltip("Define se pode atirar")]
    [SerializeField] private bool shotBool = false;

    [Header("Lock Aim Settings")]

    [Tooltip("Define o Prefab da mira")]
    [SerializeField] private GameObject aimPrefab;

    [Tooltip("Define o objeto da mira")]
    [SerializeField] private GameObject aimGO;

    [Tooltip("Define a cor da mira")]
    [SerializeField] private Color aimColor;

    [Tooltip("Define se aimGO ja foi instanciada")]
    [SerializeField] private bool aimGOInstantiated;

    [Tooltip("Definido a ultima posição do player")]
    [SerializeField] private Vector2 lastPlayerPos;

    [Tooltip("Define o tempo para travar a mira")]
    [SerializeField] private float timeToLockAim;

    [Tooltip("Definido por timeToLockAim")]
    [SerializeField] private float timeToLockAimAux;

    [Tooltip("Definido depois que o tempo de lockAimAux acaba")]
    [SerializeField] private bool lockAim = false;



    void Start(){
        player = GameObject.FindGameObjectWithTag("Game_Events").GetComponent<Game_Events>().getPlayer().transform;
        timeToShotAux = timeToShot;
        timeToLockAimAux = timeToLockAim;
        timeShotingAux = timeShoting;
        boss = gameObject.transform.parent.GetComponent<Boss_Behaviour>();
        game_Events = boss.getGameEvents();
        // GameObject.FindGameObjectWithTag("Game_Events").GetComponent<Game_Events>();

        instantiateAimGO();

        bossName = gameObject.transform.parent.name;
    }

    private void instantiateAimGO(){
        aimGO = Instantiate(aimPrefab, this.transform.position, Quaternion.identity);
        aimGO.GetComponent<Transform>().localScale = new Vector2(2f, 2f);
        aimGO.GetComponent<SpriteRenderer>().color = aimColor;
    }

    void Update(){   
        thisRotation();
        checkLockAim();
        checkShot();
        
    }

    //====================================
    // MIRA E ATIRA ======================
    //====================================

    private void checkLockAim(){
        // se mira travada, entao retorna
        if ( lockAim || !bossArrived) { return; }

        timeToLockAimAux -= Time.deltaTime;

        if(timeToLockAimAux <= 0){
            lockAim = true;
            shotBool = true;
        }else{
            updateAimPosition();
        }
    }

    private void updateAimPosition(){
        if (game_Events.playerIsAlive()){
            aimGO.transform.position = player.transform.position;
        }
    }

    private void checkShot(){
        // So passa se mira travada
        if( !lockAim ) return;

        timeToShotAux -= Time.deltaTime;

        if(timeToShotAux <= 0 && shotBool){
            shotBool = false;
            GameObject laser = Instantiate(laserPrefab, shotTransform.transform);
            laser.GetComponent<Laser_Behaviour>().setBossCannon( this.gameObject );
        }
    }

    public void resetShot(){
        timeToShotAux = timeToShot;
        timeToLockAimAux = timeToLockAim;
        lockAim = false;  
        shotBool = false;
    }


    //====================================
    // OUTROS ======================
    //====================================

    public void setInvulnerable(bool b){
        this.invulnerable = b;
    }

    public void setBossArriveAnimation(bool b){
        this.bossArrived = b;
    }

    private void thisRotation(){
        Vector3 targetPos;
        Game_Events ge = GameObject.FindGameObjectWithTag("Game_Events").GetComponent<Game_Events>();
        // Se mira estiver travada, entao mira para o aimGO, se nao entiver, mira no player
        if (lockAim){
            targetPos = aimGO.transform.position;            
        }
        else {
            if ( ge.playerIsAlive() ){
                targetPos = player.transform.position; 
            }else{
                targetPos = new Vector2(0,0);
            } 
        }

        Vector3 thisPos = transform.position;
        targetPos.x = thisPos.x - targetPos.x;
        targetPos.y = thisPos.y - targetPos.y;
        
        // Pega o valor da Tangente e trasnforma em angulo
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - offset));
    }

    // =================================================
    // VIDA ============================================
    // =================================================

    public void takeDamage(int damage){
        life -= damage;
        checkLife();
    }

    public void checkLife(){
        if(life <= 0){
            boss.decreaseTurret();
            aimGO.SetActive(false);
            Destroy(this.gameObject);
        }
    }

}