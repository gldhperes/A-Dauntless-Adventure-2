using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Boss_Behaviour : MonoBehaviour
{
    [Header("Boss Settings")] public Game_Events game_Events;

    public float speed;
    public int lifeOfTurrets;
    public List<Transform> turrets;
    public int childCount;
    public bool bossArrived = false;

    [Header("Boss 4 Settings")] public Transform boss4Spawn;
    public bool boss4arrived = false;


    [Header("Update Drops Settings")] public int dropUpgradePoints;
    public float dropAreaOffset;
    public float timeForDrop;

    [Header("Waypoints Settings")] public GameObject waypointsGO;
    public AnimationCurve _curve;
    public Transform[] waypoints;
    public float distanceOffset = 0.05f;
    public int index = 0;
    public int auxIndex;
    public float t = 0;

    public Vector2 p0;
    public Vector2 p1;
    public Vector2 p2;

    public Vector2 posA;
    public Vector2 posB;

    [Header("Stealth Settings")] public bool stealthMode;
    public float endStealth;
    public float currentStealthAlpha;
    public float stealthTime;
    public bool decreaseAplha = true;
    public float fireRateToIncrease;

    [Header("Enemywaves Settings")]
    [SerializeField]
    private GameObject bossWaveTransform;

    [SerializeField] private List<Transform> bossWaveTransformChildrens;
    [SerializeField] private List<GameObject> enemyPlanesWave;
    [SerializeField] private bool spawningWave;

    void Awake()
    {
        boss4Spawn = GameObject.FindGameObjectWithTag("Boss4Spawn").transform;
    }

    void Start()
    {
        getChildrens();
        setTurretLifeByBoss();
        setBossWaveTransform();
    }




    public void setBossArrived(bool b)
    {
        bossArrived = b;
        foreach (Transform tur in turrets)
        {
            if (tur.gameObject.tag.Contains("Boss"))
            {
                tur.gameObject.GetComponent<EnemyGround>().setBossArriveAnimation(b);
            }
            else if (tur.gameObject.tag.Contains("B3"))
            {
                tur.gameObject.GetComponent<Boss3Cannon>().setBossArriveAnimation(b);
            }
        }
    }

    private void getChildrens()
    {
        Transform[] childrens = this.gameObject.transform.GetComponentsInChildren<Transform>();
        childCount = childrens.Length;

        for (int i = 1; i < childrens.Length; i++)
        {
            // Começa em i=1 pois, i=0 é o pai
            // Se os Filhos tiverem na TAG o "Boss", entao diminui coloca no vetor turrets
            if (childrens[i].gameObject.tag.Contains("Boss") || childrens[i].gameObject.tag.Contains("B3"))
            {
                turrets.Add(childrens[i]);
            }
        }

        childCount = turrets.Count;
    }

    private void setTurretLifeByBoss()
    {
        foreach (Transform tur in turrets)
        {
            if (tur != null)
            {
                if (tur.tag.Contains("Boss"))
                {
                    Enemy_Behaviour tmp = tur.GetComponent<Enemy_Behaviour>();
                    tmp.setLifeByBoss(lifeOfTurrets);
                    tmp.setParentBoss(this);
                }
            }
        }
    }

    void Update()
    {
        move();
        // waveDestroyed();
        changeAplhaForStealthMode();
    }


    public void move()
    {
        if (!bossArrived) return;

        if (this.gameObject.name.Contains("Boss 4")) return;

        // Calculate Velocity
        t = Mathf.MoveTowards(t, 1f, speed * Time.deltaTime);


        // Gets the position of points
        auxIndex = index;
        p0 = waypoints[index].position;

        auxIndex = (index + 1 < waypoints.Length) ? index + 1 : 0;
        p1 = (waypoints[auxIndex] != null) ? waypoints[auxIndex].position : waypoints[0].position;

        if (index + 2 < waypoints.Length) p2 = waypoints[index + 2].position;
        else if (auxIndex == 0 && index + 2 >= waypoints.Length) p2 = waypoints[auxIndex + 1].position;
        else p2 = waypoints[0].position;


        // Do the lerp calculation
        posA = Vector2.Lerp(p0, p1, t);
        posB = Vector2.Lerp(p1, p2, t);

        // Move
        transform.position = Vector2.Lerp(posA, posB, _curve.Evaluate(t));

        if (t >= 1f)
        {
            // // Gets the position of points
            // auxIndex = index;
            // p0 = waypoints[index].position;

            // auxIndex = (index + 1 < waypoints.Length) ? index + 1 : 0;
            // p1 = (waypoints[auxIndex] != null) ? waypoints[auxIndex].position : waypoints[0].position;

            // if (index + 2 < waypoints.Length) p2 = waypoints[index + 2].position;
            // else if (auxIndex == 0 && index + 2 >= waypoints.Length) p2 = waypoints[auxIndex + 1].position;
            // else p2 = waypoints[0].position;


            index += 2;
            if (index >= waypoints.Length)
            {
                index = 0;
            }

            if (auxIndex >= waypoints.Length)
            {
                auxIndex = 0;
            }

            t = 0;
        }
    }

    public void setSpeed(float s)
    {
        this.speed = s;
    }

    public void decreaseTurret()
    {
        childCount -= 1;

        if (childCount <= 0)
        {
            Animator anim = GetComponent<Animator>();

            if (gameObject.name.Contains("Boss 4"))
            {
                anim.Play("boss4Dead");
            }
            else
            {
                anim.Play("bossDead");
            }
        }

        else if (gameObject.name.Contains("Boss 1"))
        {
            activeStealth();
        }

        else if (gameObject.name.Contains("Boss 2"))
        {
            checkSpawnWave();
        }

        else if (gameObject.name.Contains("Boss 4"))
        {
            activeStealth();
            checkSpawnWave();
        }
    }

    public void setWaypoints(GameObject ways)
    {
        waypointsGO = ways;

        int waypointsCount = waypointsGO.transform.childCount;
        waypoints = new Transform[waypointsCount];

        Transform[] childrens = waypointsGO.transform.GetComponentsInChildren<Transform>();

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = childrens[i + 1];
        }

        p0 = waypoints[0].position;
        p1 = waypoints[1].position;
        p2 = waypoints[2].position;
    }

    public void setGameEvents(Game_Events ge)
    {
        this.game_Events = ge;
    }


    // ===========================
    // BOSS 1
    // ============================
    // ===========================
    // STEALTH MODE
    // ============================

    public void activeStealth()
    {
        // Ativa o modo stealth
        StartCoroutine(ActivateStealth());
    }

    private IEnumerator ActivateStealth()
    {
        resetStealthEnd();
        this.stealthMode = true;
        change_FireRate_And_Invulnerable();
        yield return new WaitForSeconds(stealthTime);
        this.stealthMode = false;
        change_FireRate_And_Invulnerable();
        resetStealthEnd();
    }

    private void change_FireRate_And_Invulnerable()
    {
        foreach (Transform tur in turrets)
        {
            if (tur != null)
            {
                if (tur.tag.Contains("Boss"))
                {
                    Enemy_Behaviour tmp = tur.GetComponent<Enemy_Behaviour>();

                    if (this.stealthMode)
                    {
                        tmp.setFireRate(fireRateToIncrease);
                    }
                    else
                    {
                        tmp.resetFireRate();
                    }

                    tmp.setInvulnerable(this.stealthMode);
                }
                else if (tur.tag.Contains("B3Cannon"))
                {
                    Boss3Cannon tmp = tur.GetComponent<Boss3Cannon>();
                    tmp.setInvulnerable(this.stealthMode);
                }
            }
        }
    }

    private void changeAplhaForStealthMode()
    {
        if (this.stealthMode)
        {
            changeAlphaOverTime();
        }
    }

    private void changeAlphaOverTime()
    {
        // Altera endStealth com o passar do tempo
        // Ate a metado do tempo ele diminui o alpha
        // Depois ele troca a chave e aumenta o alpha ate 1
        if ((this.endStealth > this.stealthTime / 2) && (this.decreaseAplha))
        {
            this.endStealth -= Time.deltaTime;
        }
        else
        {
            if ((this.currentStealthAlpha < .5f) && (this.decreaseAplha))
            {
                this.decreaseAplha = false;
            }

            this.endStealth += Time.deltaTime;
        }

        this.currentStealthAlpha = this.endStealth / this.stealthTime;

        // Muda cor desse Objeto (boss)
        Color myAlpha = gameObject.GetComponent<SpriteRenderer>().color;
        myAlpha.a = this.currentStealthAlpha;
        gameObject.GetComponent<SpriteRenderer>().color = myAlpha;

        // para cada filho do boss altera o Aplha
        foreach (Transform tur in turrets)
        {
            if (tur != null)
            {
                // Debug.Log("name: " + tur.name);
                Color tmp = tur.GetComponent<SpriteRenderer>().color;
                tmp.a = this.currentStealthAlpha;
                tur.GetComponent<SpriteRenderer>().color = tmp;
            }
        }
    }

    public bool getStealthMode()
    {
        return this.stealthMode;
    }

    private void resetStealthEnd()
    {
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
    private void setBossWaveTransform()
    {


        if (gameObject.name.Contains("Boss 2") || gameObject.name.Contains("Boss 4"))
        {
            this.bossWaveTransform = GameObject.FindGameObjectWithTag("Boss2SpawnWaves");

            if (this.bossWaveTransform == null)
            {
                Debug.LogError("Objeto com a tag 'Boss2SpawnWaves' não foi encontrado!");
                return;
            }

            Debug.Log(bossWaveTransform.name);
            Debug.Log(bossWaveTransform.transform.childCount);

            for (int i = 0; i < bossWaveTransform.transform.childCount; i++)
            {
                Transform child = bossWaveTransform.transform.GetChild(i);
                bossWaveTransformChildrens.Add(child);
            }
        }

    }

    private void checkSpawnWave()
    {
        if (spawningWave && bossArrived) return;

        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        bossWaveTransform.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x,
            bossWaveTransform.gameObject.transform.position.y);

        foreach (Transform wave in bossWaveTransformChildrens)
        {
            GameObject enemyBossPlane =
                Instantiate(game_Events.getPlaneEnemy(), wave.transform.position,
                    new Quaternion(0, 0, 180, 0)) as GameObject;
            enemyBossPlane.GetComponent<Enemy_Behaviour>().setFromBoss();
            enemyPlanesWave.Add(enemyBossPlane);

            StartCoroutine(ShotPlanesFromBoss(enemyBossPlane));

        }
        spawningWave = true;

        yield return new WaitForSeconds(6f);


    }

    public IEnumerator ShotPlanesFromBoss(GameObject enemyBossPlane)
    {
        Destroy(enemyBossPlane, 15f);
        enemyBossPlane?.GetComponent<EnemyPlane>()?.Shot();

        yield return new WaitForSeconds(3f);
        spawningWave = false;
    }




    // ===========================
    // GETTERS
    // ============================

    public int getDropUpgradePoints()
    {
        return this.dropUpgradePoints;
    }

    public float getDropAreaOffset()
    {
        return this.dropAreaOffset;
    }

    public float getTimeForDrop()
    {
        return this.timeForDrop;
    }

    public Game_Events getGameEvents()
    {
        return this.game_Events;
    }

    public Sprite GetBossSprite()
    {
        return gameObject.GetComponent<SpriteRenderer>().sprite;
    }
}