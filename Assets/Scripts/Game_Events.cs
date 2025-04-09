using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class Game_Events : MonoBehaviour
{
    [Header("Game Settings")] public SO_Data data;
    public Player_Sprites player_Sprites;
    public int gameLevel;
    public int checkpointsInScene;
    public int enemysToSpawnCheckpoint;
    public bool gameStarted;
    public AudioSettings audioSettings;


    // [SerializeField] private MobileChecked mobileChecked;


    [Header("Background Settings")] public GameObject background;
    public Level_Islands level_Islands;
    public float bgSpeed;


    [Header("Player Settings")]
    [SerializeField]
    private GameObject playerPrefab;

    public GameObject player;

    public Player_Behavior player_Behavior;
    public Sprite playerSprite;
    [SerializeField] private Transform playerSpawn;

    [SerializeField] private bool playerAlive;
    public Animator playerAnimator;
    public Animator canvasAnimator;

    [Tooltip("Quando player morre, espera-se um tempo para a chamada da cena de gameover")]
    [SerializeField]
    private float timeToCallGameOver;


    [Header("Player HUD")]
    // public Canvas playerCanvas;
    // public GameObject missionComplete;
    public float animCanvasWarningTimer;

    public float animTimeMissionComplete;

    [Tooltip(
        "Variavel de tempo para mudar o estado de gameStarted. Faz referencia ao tempo de animação do player ao iniciar a fase (animação de decolar do player)")]
    public float animTakinoffTimer;

    [SerializeField] private UIDocument uiDocument;
    private FaseScreen faseScreen;
    public Action<int> OnPlayerLifeChanged;
    public Action<int> OnPlayerUpgradeChanged;


    [Header("Bullet Settings")] public GameObject playerBulletGO;
    public GameObject enemyBulletGO;
    public GameObject bulletHitGO;
    public float bulletHitTime;


    [Header("Droprate Settings")] public GameObject dropGO;
    public Sprite upgradeSprite;
    public Sprite lifeSprite;
    public Sprite shieldSprite;
    public int dropUpgradeRate;
    public int dropShieldRate;
    public int dropLifeRate;


    [Header("Boss Settings")] public GameObject[] bossGO;
    public GameObject currentBossGO;
    public Boss_Behaviour currentBossBehaviour;
    public Transform bossSpawnPosition;
    public GameObject bossWaypoints;
    public bool bossInScene;
    public Animator bossAnimator;

    [SerializeField] private Vector2 bossDeadPos;
    [SerializeField] private int quantUpgradesToDrop;
    [SerializeField] private int upgradeDropsCount = 0;
    [SerializeField] private float timeForDrop;
    [SerializeField] private float auxTimeForDrop;
    [SerializeField] private bool bossDropUpgradePoints;
    [SerializeField] private List<GameObject> bossDropGOs;

    [Header("Enemy Settings")] public GameObject enemyPlanePrefab;
    public GameObject enemyGroundPrefab;
    public GameObject respawPlaneEnemys;
    public GameObject spawGroundEnemys;
    [SerializeField] private GameObject enemyMoveSetWaypoints;
    public int spawnGroundEnemyChance;

    [Tooltip("Seta a quantidade maxima de inimigos na ilha")]
    public int maxGroundEnemysInIsland;

    public float spawnIslandTime;
    public float timerToRespawnEnemy;
    [SerializeField] private float timer;
    public int enemysToDefeat;
    public int enemysDefeated;
    public float progressBar;
    public static Action OnEnemyDefeated;
    public event Action<float> OnUpdateProgressBar;
    public event Action OnUpdatePointsEarned;


    public int maxEnemysInScene;
    public int enemysInScene = 0;
    public Sprite[] planeEnemysLVL1;
    public Sprite[] planeEnemysLVL2;
    public Sprite[] planeEnemysLVL3;
    public Sprite[] currentPlaneEnemys;
    public Sprite[] groundEnemysLVL1;
    public Sprite[] groundEnemysLVL2;
    public Sprite[] currentGroundEnemys;


    [Header("Camera Settings")] public Camera mainCam;
    [SerializeField] private Camera_Behaviour camera_Behaviour;

    void Start()
    {
        data = GameObject.Find("SO_DATA").GetComponent<SO_Data>();
        player_Sprites = GameObject.Find("Player Sprites").GetComponent<Player_Sprites>();
        gameLevel = data.gameLevel;

        // timerToRespawnEnemy = timerToRespawnEnemy - (.5f * gameLevel);
        spawnIslandTime = spawnIslandTime - (.5f * gameLevel);

        // Player
        // INITIATE PLAYER CONFIGURATIONS
        player = Instantiate(playerPrefab, playerSpawn.transform.position, Quaternion.identity);
        player_Behavior = player.GetComponent<Player_Behavior>();
        player_Behavior.StartPlayer();
        player_Behavior.StartPlayerStatus();

        playerAnimator = player.GetComponent<Animator>();
        playerAlive = true;

        // UI
        // INITIATE UI ELEMENTS
        faseScreen = uiDocument.gameObject.GetComponent<FaseScreen>();
        faseScreen.InitScreen(data, player_Behavior);
        PlayerTakingOffAnimation();


        // EVENT LISTENERS
        OnPlayerLifeChanged += (life) => faseScreen.UpdatePlayerLife(life);
        OnPlayerUpgradeChanged += (upgrade) =>
        {
            faseScreen.UpdateUpgradePoints(upgrade);
            data.upgradePoints += 1;
        };
        OnEnemyDefeated += EnemyDefeated;
        OnUpdateProgressBar += (progress) => faseScreen.UpdateProgressBar(progress);


        // Camera
        mainCam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        camera_Behaviour = GameObject.FindWithTag("MainCamera").GetComponent<Camera_Behaviour>();

        // Background
        background = GameObject.FindWithTag("Background");

        level_Islands.resetSpawnIslandTimer();
        level_Islands.setGameLevelIsland();
        level_Islands.setIslandTime();

        level_Islands.setSpawnIslandBool(true);
        setCurrentPlaneAndGroundEnemys();


    }

    private void setCurrentPlaneAndGroundEnemys()
    {
        switch (this.gameLevel)
        {
            case 1:
                audioSettings.setStageMusic(gameLevel);
                currentPlaneEnemys = planeEnemysLVL1;
                currentGroundEnemys = groundEnemysLVL1;
                // Selecionar o Array de Ilhas disponiveis
                break;

            case 2:
                audioSettings.setStageMusic(gameLevel);
                currentPlaneEnemys = planeEnemysLVL2;
                currentGroundEnemys = groundEnemysLVL1;
                // Selecionar o Array de Ilhas disponiveis
                break;

            case 3:
                audioSettings.setStageMusic(gameLevel);
                currentPlaneEnemys = planeEnemysLVL3;
                currentGroundEnemys = groundEnemysLVL2;
                // Selecionar o Array de Ilhas disponiveis
                break;

            case 4:
                audioSettings.setStageMusic(gameLevel);
                currentPlaneEnemys = planeEnemysLVL3;
                currentGroundEnemys = groundEnemysLVL2;
                // Selecionar o Array de Ilhas disponiveis
                break;

            default:
                break;
        }
    }


    private void PlayerTakingOffAnimation()
    {
        gameStarted = false;
        StartCoroutine (faseScreen.Animate_TakingOff(animTakinoffTimer, gameLevel));

        StartCoroutine (player_Behavior.Animate_TakingOff(animTakinoffTimer));

        StartCoroutine(Continue_Before_Timer_Callback());

        IEnumerator Continue_Before_Timer_Callback()
        {
            yield return new WaitForSeconds(animTakinoffTimer);

            gameStarted = true;

            player_Behavior.setCanMove(gameStarted);
            player_Behavior.setCanShot(gameStarted);
        }
    }

    void Update()
    {
        moveBackground();
        TimerToRespawnEnemys();
        animateDropingUpgradePoints();
    }


    // ISLAND CONTROLLER ==============================
    private void moveBackground()
    {
        background.transform.Translate(new Vector2(0, bgSpeed * Time.deltaTime) * -1, Space.Self);
    }


    // ====================================================
    // BOSS CONTROLLER ====================================
    // ====================================================

    #region BOSS_CONTROLLER

    // Mission Complete ----------------------------------
    public void animMissionComplete(Vector2 bossPos, int upgradesToDrop)
    {
        // Seta variaveis para morte do boss
        bossDeadPos = bossPos;
        quantUpgradesToDrop = upgradesToDrop;

        timeForDrop = currentBossBehaviour.getTimeForDrop();
        auxTimeForDrop = timeForDrop;

        // missionComplete.SetActive(true);
        StartCoroutine(MissionComplete());
    }

    private IEnumerator MissionComplete()
    {
        audioSettings.setStageClear();

        yield return new WaitForSeconds(animTimeMissionComplete);
        // missionComplete.SetActive(false);
        bossDropUpgradePoints = true;


        // Chama função de drop
        // bossAnimator.Play("bossDropUpgradePoints");
        // bossAnimator.SetBool("dropUpgrade", true);
    }

    private void animateDropingUpgradePoints()
    {
        if (bossDropUpgradePoints)
        {
            timeForDrop -= Time.deltaTime;

            if (timeForDrop <= 0)
            {
                timeForDrop = auxTimeForDrop;
                upgradeDropsCount += 1;

                // Esse IF é quando atinge a quantidade maxima de drops, entao ele sai do laço
                if (upgradeDropsCount >= quantUpgradesToDrop)
                {
                    upgradeDropsCount = 0;
                    bossDropUpgradePoints = false;

                    player_Behavior.receiveUpgradeList(bossDropGOs, quantUpgradesToDrop);

                    foreach (GameObject d in bossDropGOs)
                    {
                        d.GetComponent<Drop_Behaviour>().catchable = true;
                        d.GetComponent<Drop_Behaviour>()
                            .setSpeed(getDropGO().GetComponent<Drop_Behaviour>().getSpeed());
                        d.GetComponent<Drop_Behaviour>().setFollowPlayer(player);
                    }

                    return;
                }

                dropUpgrade();
            }
        }
    }

    private void dropUpgrade()
    {
        float randomX = Random.Range(-1f * currentBossBehaviour.getDropAreaOffset(),
            currentBossBehaviour.getDropAreaOffset());
        float randomY = Random.Range(-1f * currentBossBehaviour.getDropAreaOffset(),
            currentBossBehaviour.getDropAreaOffset());
        Vector2 pos = new Vector2(currentBossGO.transform.position.x + randomX,
            currentBossGO.transform.position.y + randomY);

        GameObject bossDropGO = Instantiate(getDropGO(), pos, Quaternion.identity);

        // Seta variaveis para o drop do Upgradepoint
        Drop_Behaviour bossDropBehaviour = bossDropGO.GetComponent<Drop_Behaviour>();
        bossDropBehaviour.setSpeed(0);
        bossDropBehaviour.setTagName("Upgrade");
        bossDropBehaviour.setSprite(getUpgradeSprite());
        bossDropBehaviour.catchable = false;

        bossDropGOs.Add(bossDropGO);
    }

    public void bossDefeated()
    {
        GameLootLoading gameLootLoading = FindAnyObjectByType<GameLootLoading>();
        // Seta o upgrade points que o player ganhou
        // data.upgradePoints = int.Parse(playerCanvas.GetComponent<Player_Canvas>().upgradeText.text);

        if (gameLevel >= 4)
        {
            gameLootLoading.LoadScene(Scenes_To_Call.Creditos);
            return;
        }

        // Aumentar Game Level + 1
        data.gameLevel += 1;

        // Aumenta o Hangar arrival
        data.hangarArrivals += 1;

        // Voltar para hangar
        gameLootLoading.LoadScene(Scenes_To_Call.Hangar);
    }


    // Boss Arriving -----------------------------------------------

    private void SpawnBoss(float animTime)
    {
        GameObject boss = Instantiate(bossGO[gameLevel - 1], bossSpawnPosition.position, Quaternion.identity);
        currentBossGO = boss;

        AnimateBossArriving animateBossArriving = boss.GetComponent<AnimateBossArriving>();

        currentBossBehaviour = currentBossGO.GetComponent<Boss_Behaviour>();
        currentBossBehaviour.setGameEvents(this);
        currentBossBehaviour.setWaypoints(this.bossWaypoints);

        animateBossArriving.SetSpanwBossPosition(bossSpawnPosition, animTime);

        if (gameLevel >= 4)
        {
            level_Islands.destroyAllIslands();
            return;
        }
    }


    private void destroyAllEnemys()
    {
        GameObject[] destroyEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in destroyEnemy)
        {
            Destroy(enemy);
        }
    }

    private void DestroyBulletsAtScene()
    {
        Bullet_Behaviour[] bullets = FindObjectsOfType<Bullet_Behaviour>();

        foreach (Bullet_Behaviour bullet in bullets)
        {
            Destroy(bullet.gameObject);
        }
    }

    #endregion BOSS_CONTROLLER

    // ENEMYS CONTROLLER ============================
    #region ENEMIES_CONTROLLER

    private void Animate_Boss_Arriving()
    {
        destroyAllEnemys();
        DestroyBulletsAtScene();

        // Sets the music to Play
        if (getGameLevel() == 4)
        {
            audioSettings.setBoss4FightMusic();
        }
        else
        {
            audioSettings.setBossFightMusic();
        }

        level_Islands.setSpawnIslandBool(false);


        // Animate the Warning message
        faseScreen.Animate_Warning(animCanvasWarningTimer, SpawnBoss);
    }



    private void TimerToRespawnEnemys()
    {
        if (timer > 0)
        {
            timer -= Time.deltaTime;
        }
        else if (!bossInScene && gameStarted)
        {
            resetTimerToRespawn();
            StartCoroutine(Spawn_Enemys_Plane());
        }
    }

    private void resetTimerToRespawn()
    {
        timer = timerToRespawnEnemy;
    }

    private IEnumerator Spawn_Enemys_Plane()
    {
        // float respawnSize = respawPlaneEnemys.transform.localScale.x / 2;
        int waypointChild = Random.Range(0, enemyMoveSetWaypoints.transform.childCount);

        int enemiesSpawnNumber = Random.Range(2, 4);

        for (int i = 0; i < enemiesSpawnNumber; i++)
        {
            GameObject enemyPlaneGO =
                Instantiate(enemyPlanePrefab, respawPlaneEnemys.transform.position, new Quaternion(0, 0, 180, 0)) as
                    GameObject;
            EnemyPlane enemyPlane = enemyPlaneGO.GetComponent<EnemyPlane>();
            enemyPlane.setSprite(currentPlaneEnemys[Random.Range(0, currentPlaneEnemys.Length)]);

            // Pega um MoveSet aleatorio 
            GameObject randomMoveSet = enemyMoveSetWaypoints.transform.GetChild(waypointChild).gameObject;
            enemyPlane.SetWaypoints(randomMoveSet);
            enemysInScene += 1;
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void EnemyDefeated()
    {
        enemysDefeated += 1;

        if ((enemysDefeated >= enemysToDefeat) && !bossInScene)
        {
            // Player nao pode atirar nesse periodo
            // Player so pode atirar de novo no final da animação do boss (No script)
            player_Behavior.setCanShot(false);

            bossInScene = true;

            Animate_Boss_Arriving();
        }
        else
        {
            progressBar = (float)enemysDefeated / (float)enemysToDefeat;
            OnUpdateProgressBar?.Invoke(progressBar);
        }
    }
    #endregion ENEMIES_CONTROLLER


    // PLAYER CONTROLLER =================================
    #region PLAYER_CONTROLLER
    public bool playerIsAlive()
    {
        return this.playerAlive;
    }

    public void playerDied()
    {
        playerAlive = false;
        StartCoroutine(CallGameOver());
    }

    private IEnumerator CallGameOver()
    {
        yield return new WaitForSeconds(timeToCallGameOver);
        audioSettings.setGameoverMusic();
        // player_Behavior.player_Canvas.gameOver();
    }

    #endregion PLAYER_CONTROLLER


    // GETTERS ===========================================
    #region GETTERS
    public GameObject getPlayer()
    {
        return this.player;
    }

    public GameObject getPlaneEnemy()
    {
        return this.enemyPlanePrefab;
    }

    public int getGameLevel()
    {
        return this.gameLevel;
    }


    public Camera getMainCam()
    {
        return this.mainCam;
    }

    public Camera_Behaviour GetCameraBehaviour()
    {
        return this.camera_Behaviour;
    }

    public GameObject getEnemyBullet()
    {
        return this.enemyBulletGO;
    }

    public GameObject getRespawn()
    {
        return this.respawPlaneEnemys;
    }

    public GameObject getPlayerBullet()
    {
        return this.playerBulletGO;
    }

    public GameObject getBulletHitGO()
    {
        return this.bulletHitGO;
    }

    public float getBulletHitTime()
    {
        return this.bulletHitTime;
    }

    public GameObject getSpawnGroundEnemysGO()
    {
        return this.spawGroundEnemys;
    }

    public GameObject getBackgorundGO()
    {
        return this.background;
    }

    public int getSpawnGroundEnemyChance()
    {
        return this.spawnGroundEnemyChance;
    }

    public GameObject getEnemyGroundPrefab()
    {
        return this.enemyGroundPrefab;
    }

    public Sprite[] getCurrentGroundEnemys()
    {
        return this.currentGroundEnemys;
    }

    public int getMaxGroundEnemysInIsland()
    {
        return this.maxGroundEnemysInIsland;
    }

    public GameObject getBossGO()
    {
        return this.currentBossGO;
    }

    public AudioSettings getAudioSettings()
    {
        return this.audioSettings;
    }

    public float getSpawnIslandTime()
    {
        return this.spawnIslandTime;
    }

    public SO_Data getData()
    {
        return this.data;
    }

    #endregion GETTERS

    // ========== Drops ==========
    #region DROPS
    public GameObject getDropGO()
    {
        return this.dropGO;
    }

    public Sprite getUpgradeSprite()
    {
        return this.upgradeSprite;
    }

    public Sprite getLifeSprite()
    {
        return this.lifeSprite;
    }

    public Sprite getShieldSprite()
    {
        return this.shieldSprite;
    }

    #endregion DROPS
}