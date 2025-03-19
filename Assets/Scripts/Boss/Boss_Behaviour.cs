using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Behaviour : MonoBehaviour
{
    [Header("Boss Settings")]
    public Game_Events game_Events;
    public float speed;
    public int lifeOfTurrets;
    public List<Transform> turrets;
    public int childCount;
    public bool bossArrived = false;

    [Header("Boss 4 Settings")]
    public Transform boss4Spawn;
    public bool boss4arrived = false;
  

    [Header("Update Drops Settings")]
    public int dropUpgradePoints;
    public float dropAreaOffset;
    public float timeForDrop;
    
    [Header("Waypoints Settings")]
    public GameObject waypointsGO;
    public Transform[] waypoints;
    public float distanceOffset = 0.05f;
    public int index = 0;
    

    [Header("Stealth Settings")]
    public bool stealthMode = false;
    public float endStealth;
    public float currentStealthAlpha;
    public float stealthTime;
    public bool decreaseAplha = true;
    public float fireRateToIncrease;

    [Header ("Enemywaves Settings")]
    [SerializeField] private Transform bossWaveTransform;
    [SerializeField] private List<Transform> bossWaveTransformChildrens;
    [SerializeField] private List<GameObject> enemyPlanesWave;
    [SerializeField] private bool spawningWave = false;


    void Start()
    {     
        getChildrens();   
        setTurretLifeByBoss();    
        setBossWaveTransform();

        if(this.gameObject.name.Contains("Boss 4")) {
            boss4Spawn = GameObject.FindGameObjectWithTag("Boss4Spawn").transform;
        }
    }

    public void setBossArrived(bool b){
        bossArrived = b;
        foreach (Transform tur in turrets){
            if ( tur.gameObject.tag.Contains("Boss") ){
                tur.gameObject.GetComponent<EnemyGround>().setBossArriveAnimation(b);
            }else if ( tur.gameObject.tag.Contains("B3") ){
                tur.gameObject.GetComponent<Boss3Cannon>().setBossArriveAnimation(b);
            }
        }
    }

    private void getChildrens(){
        
        Transform[] childrens = this.gameObject.transform.GetComponentsInChildren<Transform>();
        childCount = childrens.Length;

        // for (int i = 1; i < childrens.Length; i++){
        //     // Começa em i=1 pois, i=0 é o pai
        //     // Se os Filhos nao contem na TAG o "Boss" ou "B3", entao diminui 1 na quantidade de filhos
        //     if ( !childrens[i].gameObject.tag.Contains("Boss") || !childrens[i].gameObject.tag.Contains("B3") ){
        //         childCount -= 1;
        //     }
        // }


        for (int i = 1; i < childrens.Length; i++){
            // Começa em i=1 pois, i=0 é o pai
            // Se os Filhos tiverem na TAG o "Boss", entao diminui coloca no vetor turrets
            if ( childrens[i].gameObject.tag.Contains("Boss") || childrens[i].gameObject.tag.Contains("B3") ){
                turrets.Add(childrens[i]);
            }
        }

        childCount = turrets.Count;
       
    }

    private void setTurretLifeByBoss(){
        foreach (Transform tur in turrets){   
            if(tur != null){
                if( tur.tag.Contains("Boss") ){
                    Enemy_Behaviour tmp = tur.GetComponent<Enemy_Behaviour>();
                    tmp.setLifeByBoss(lifeOfTurrets);  
                    tmp.setParentBoss(this);
                }
            }
        }
    }

    void Update() {
        move();
        waveDestroyed();
        changeAplhaForStealthMode();
    }

  

    public void move() {
        if( this.gameObject.name.Contains("Boss 4") ){
            if (!boss4arrived){
                
                transform.position = Vector2.MoveTowards(transform.position, boss4Spawn.position, speed * Time.deltaTime);

                if (Vector2.Distance(transform.position, boss4Spawn.position) <= distanceOffset) {  
                    boss4arrived = true;
                    setBossArrived(true);
                    game_Events.getPlayer().GetComponent<Player_Behavior>().setCanShot(true);
                }
            }

        }else{
            transform.position = Vector3.MoveTowards(transform.position, waypoints[index].position, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, waypoints[index].position) <= distanceOffset) {
                index++;
                if (index >= waypoints.Length) {
                    index = 0;
                }
            }
        }

    }

    public void setSpeed(float s){ this.speed = s; }

    public void decreaseTurret(){
        childCount -= 1;

        if(childCount <= 0){
            Animator anim = GetComponent<Animator>();

            if ( gameObject.name.Contains("Boss 4") ) {
                anim.Play("boss4Dead");
            }else{
                anim.Play("bossDead");
            }
        } 

        else if ( gameObject.name.Contains("Boss 1") ) {
            activeStealth();
        } 

        else if ( gameObject.name.Contains("Boss 2") ) {
            checkSpawnWave();
        }

        else if ( gameObject.name.Contains("Boss 4") ) {
            activeStealth();
            checkSpawnWave();
        }
    }
    
    public void setWaypoints(GameObject ways){
        waypointsGO = ways;

        int waypointsCount = waypointsGO.transform.childCount;
        waypoints = new Transform[waypointsCount];

        Transform[] childrens = waypointsGO.transform.GetComponentsInChildren<Transform>();

        for (int i = 0; i < waypoints.Length; i++){
            waypoints[i] = childrens[i+1];
        }
        
    }

    public void setGameEvents(Game_Events ge){
        this.game_Events = ge;
    }



    // ===========================
    // BOSS 1
    // ============================
    // ===========================
    // STEALTH MODE
    // ============================

    public void activeStealth() {
        // Ativa o modo stealth
        StartCoroutine(ActivateStealth());         
    }

    private IEnumerator ActivateStealth() {
        resetStealthEnd();
        this.stealthMode = true;
        change_FireRate_And_Invulnerable();
        yield return new WaitForSeconds(stealthTime); 
        this.stealthMode = false;
        change_FireRate_And_Invulnerable();
        resetStealthEnd();
    }

    private void change_FireRate_And_Invulnerable() {
        foreach (Transform tur in turrets){   
            if(tur != null){
                if( tur.tag.Contains("Boss")){ 
                    Enemy_Behaviour tmp = tur.GetComponent<Enemy_Behaviour>();

                    if(this.stealthMode){
                        tmp.setFireRate(fireRateToIncrease);
                    } else {
                        tmp.resetFireRate();
                    }

                    tmp.setInvulnerable(this.stealthMode);

                }else if (tur.tag.Contains("B3Cannon")){
                    Boss3Cannon tmp = tur.GetComponent<Boss3Cannon>();
                    tmp.setInvulnerable(this.stealthMode);
                }
            }    
        }
    }

    private void changeAplhaForStealthMode(){
        if(this.stealthMode){
            changeAlphaOverTime();
        }
    }

    private void changeAlphaOverTime(){

        // Altera endStealth com o passar do tempo
        // Ate a metado do tempo ele diminui o alpha
        // Depois ele troca a chave e aumenta o alpha ate 1
        if ( (this.endStealth > this.stealthTime/2) && (this.decreaseAplha) ){
            this.endStealth -= Time.deltaTime;

        }else{
            if ( (this.currentStealthAlpha < .5f) && (this.decreaseAplha) ){
                this.decreaseAplha = false;
            }

            this.endStealth += Time.deltaTime;
        }

        this.currentStealthAlpha = this.endStealth/this.stealthTime ;

        // Muda cor desse Objeto (boss)
        Color myAlpha = gameObject.GetComponent<SpriteRenderer>().color;
        myAlpha.a = this.currentStealthAlpha;
        gameObject.GetComponent<SpriteRenderer>().color = myAlpha;

        // para cada filho do boss altera o Aplha
        foreach (Transform tur in turrets)
        {   
            if(tur != null){
                // Debug.Log("name: " + tur.name);
                Color tmp = tur.GetComponent<SpriteRenderer>().color;
                tmp.a = this.currentStealthAlpha;
                tur.GetComponent<SpriteRenderer>().color = tmp;
            }    
        }

    }

    public bool getStealthMode(){
        return this.stealthMode;
    }
   
    private void resetStealthEnd(){
        this.endStealth = this.stealthTime;
        this.currentStealthAlpha = 1f;
        this.decreaseAplha = true;
    }

     // ===========================
    // BOSS 2
    // ============================
    // ===========================
    // ENEMYWAVE
    // ============================

    // Vindo do Start(), checa se o objeto é o Boss 2
    private void setBossWaveTransform(){
        if( gameObject.name.Contains("Boss 2") || gameObject.name.Contains("Boss 4")) { 
            this.bossWaveTransform = GameObject.FindGameObjectWithTag("Boss2SpawnWaves").GetComponent<Transform>();
            
            int childCount = bossWaveTransform.transform.childCount;

            Transform[] childrens = bossWaveTransform.transform.GetComponentsInChildren<Transform>();

            for (int i = 1; i < childrens.Length; i++){
                bossWaveTransformChildrens.Add(childrens[i]);
            }
        }
    }

    private void checkSpawnWave(){
        if ( spawningWave && bossArrived) return;

        spawnWave();
    }

    private void spawnWave(){
        bossWaveTransform.gameObject.transform.position = new Vector2( this.gameObject.transform.position.x, bossWaveTransform.gameObject.transform.position.y) ;

        foreach(Transform wave in bossWaveTransformChildrens){
            GameObject enemyBossPlane = Instantiate( game_Events.getPlaneEnemy(), wave.transform.position, new Quaternion(0, 0 , 180, 0)) as GameObject; 
            enemyBossPlane.GetComponent<Enemy_Behaviour>().setFromBoss();
            enemyPlanesWave.Add(enemyBossPlane);
        }

        spawningWave = true;
    }

    private void waveDestroyed(){
        if( spawningWave && (enemyPlanesWave.Count > 0) ){
            int count = 0;

            for (int i = 0; i < enemyPlanesWave.Count; i++){
                if (enemyPlanesWave[i] == null) { 
                    count += 1; 
                }

                if(enemyPlanesWave.Count == count){
                    spawningWave = false;
                    enemyPlanesWave.Clear();
                }
            }
        }
    }




    // ===========================
    // GETTERS
    // ============================

    public int getDropUpgradePoints(){
        return this.dropUpgradePoints;
    }

    public float getDropAreaOffset(){
        return this.dropAreaOffset;
    }
    
    public float getTimeForDrop(){
        return this.timeForDrop;
    }

    public Game_Events getGameEvents(){
        return this.game_Events;
    }

}
