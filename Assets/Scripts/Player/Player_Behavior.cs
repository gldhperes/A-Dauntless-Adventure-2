using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Behavior : MonoBehaviour
{
    [Header("Components")] public Game_Events game_Events;
    public Player_Canvas player_Canvas;
    public SO_Data data;
    public Player_Sprites player_Sprites;
    public SpriteRenderer mySpriteRenderer;
    public Sprite mySprite;

    [SerializeField] private GameObject explosionPrefab;
    // [SerializeField] private FixedJoystick joystick;
    // [SerializeField] private Vector2 joystickMovement;

    [Header("Player Stats")] public int life;
    private int maxLife = 3;
    public event Action<int> OnHPChanged;


    [SerializeField] private float speed;

    [SerializeField] private int shotDamage;

    [SerializeField] private float explosionArea;


    [Header("Shot Settings")] [SerializeField]
    private GameObject bullet;

    // [SerializeField]
    private bool mayShot = true;

    // [SerializeField]
    private bool canShot = false;

    // [SerializeField]
    public float fireRate;

    [SerializeField] private float fireRateAux;

    // [SerializeField]
    private float amount;

    [SerializeField] private Image shotImage;


    // [SerializeField]
    private float sizeSprite;

    [Header("Bomb Settings")] [SerializeField]
    private Image bombImage;

    [SerializeField] private GameObject bombGO;

    [SerializeField] private GameObject bombColliderGO;

    [SerializeField] private bool bombEnable;

    public float bombSpecialFull;
    public float bombSpecial;
    public float bombFillRate;

    // [Header("Upgrade Settings")]
    // [SerializeField]
    private int upgradePoints;
    public event Action<int> OnUpgradePointsChanged;


    // [Tooltip ("Lista recebida quando o boss é morto")]
    // [SerializeField] 
    private List<GameObject> dropsGO;

    // [Tooltip ("Quantidade de upgrades que tem q receber do boss")]
    // [SerializeField]
    private int upgradePointsToReceive;

    [Tooltip("Tempo para chamar o Hangar depois que boss morre")]
    public float timeToCallHangar;


    [Header("Shield Settings")] [SerializeField]
    private AudioClip shieldSound;

    [SerializeField] private Image shieldImage;
    [SerializeField] private bool shieldUp = false;

    //Shield Colours
    [SerializeField] private Color myColor;

    [SerializeField] private Color[] colors =
        { Color.blue, Color.cyan, Color.green, Color.yellow, Color.red, Color.magenta };

    [SerializeField] private float colorTime;
    [SerializeField] private int colorIndex = 0;

    // Shield Timers
    [SerializeField] private float thisShotTime = 0;

    [Tooltip("Divisão entre valores que vai de 0 a 1 e completa o Fill_Amount da imagem do Escudo")] [SerializeField]
    private float thisShieldTime;

    [Tooltip("Tempo que aumenta até acabar o tempo do Escudo")] [SerializeField]
    private float endShield = 0;

    [Tooltip("Tempo máximo do Escudo")] [SerializeField]
    private float shieldTime;


    // [Header("Move Settings")]
    // Mudar para private depois de testes
    // [SerializeField]
    private bool canMove = false;

    // [SerializeField]
    private bool moveUp = true;

    // [SerializeField]
    private bool moveDown = true;

    // [SerializeField]
    private bool moveLeft = true;

    // [SerializeField]
    private bool moveRight = true;


    // [Header("Speed Settings")]    
    // [SerializeField]
    private float upways;

    // [SerializeField]
    private float sideways;

    [Header("Camera Settings")] [SerializeField]
    private Camera mainCam;

    [SerializeField] private Camera_Behaviour camera_Behaviour;
    [SerializeField] private float upBound;
    [SerializeField] private float downBound;
    [SerializeField] private float rightBound;
    [SerializeField] private float leftBound;
    [SerializeField] private float sideBoundOffset;
    [SerializeField] private float upDownBoundOffset;


    public void StartPlayer()
    {
        game_Events = GameObject.FindWithTag("Game_Events").GetComponent<Game_Events>();
        data = game_Events.getData();
        // player_Canvas = GameObject.FindGameObjectWithTag("PlayerCanvas").GetComponent<Player_Canvas>();
        // player_Canvas.setPlayerBehaviour(this);

        player_Sprites = GameObject.Find("Player Sprites").GetComponent<Player_Sprites>();

        mySpriteRenderer = GetComponent<SpriteRenderer>();
        myColor = mySpriteRenderer.color;

        mySprite = player_Sprites.playerSprite;
        setSprite();

        mainCam = game_Events.getMainCam();
        camera_Behaviour = game_Events.GetCameraBehaviour();
        bullet = game_Events.getPlayerBullet();
        // shotImage = player_Canvas.getShotImage();
        // bombImage = player_Canvas.getBombImage();
        // shieldImage = player_Canvas.getShieldImage();
        // amount = shotImage.fillAmount;

        bombEnable = data.playerBombEnable;

        resetFireReady();
        // shieldImage.fillAmount = 1f;

        getCameraBounds();
        // setPlayerJoystick();
    }

    // private void setPlayerJoystick()
    // {
    //     this.joystick = game_Events.getJoystick();
    // }


    private void getCameraBounds()
    {
        upBound = camera_Behaviour.getUpBound();
        downBound = camera_Behaviour.getDownBound();
        rightBound = camera_Behaviour.getRightBound();
        leftBound = camera_Behaviour.getLeftBound();
    }

    public void setSprite()
    {
        mySpriteRenderer.sprite = mySprite;
    }

    // ==================================================
    // START PLAYER STATUS
    // ==================================================

    public void StartPlayerStatus()
    {
        // Starts Life
        this.life = data.playerLife;
        this.maxLife = life;
        startsLifeText();

        // Starts Upgrade
        this.upgradePoints = data.upgradePoints;
        startsThisUpgradeText();

        // Starts velocidade
        this.speed = data.playerSpeed;

        // Starts raio da bomba
        this.explosionArea = data.playerBombArea;

        // Starts damage
        this.shotDamage = data.playerDamage;
    }

    private void startsLifeText() => updateThisLifeText();
    private void startsThisUpgradeText() => updateThisUpgradeText();


    void Update()
    {
        // setSprite();
        checkShot();
        checkBomb();
        // updateBombImage();

        playerBound();
        move();

        changeColor();
        // animateFireReady();
        // animateShieldEnd();
    }

    private void playerBound()
    {
        // Se estiver em cima, nao pode andar pra cima
        if (this.gameObject.transform.position.y >= (upBound))
        {
            moveUp = false;
        }
        else
        {
            moveUp = true;
        }

        // Se estiver em baixo, nao pode andar pra baixo
        if (this.gameObject.transform.position.y <= (downBound))
        {
            moveDown = false;
        }
        else
        {
            moveDown = true;
        }

        // Se estiver na direita, nao pode andar pra direta
        if (this.gameObject.transform.position.x >= (rightBound))
        {
            moveRight = false;
        }
        else
        {
            moveRight = true;
        }

        // Se estiver na Esquerda, nao pode andar pra Esquerda
        if (this.gameObject.transform.position.x <= (leftBound))
        {
            moveLeft = false;
        }
        else
        {
            moveLeft = true;
        }
    }

    public void setCanMove(bool b) => this.canMove = b;

    private void move()
    {
        // joystickMovement.x = joystick.Horizontal;
        // joystickMovement.y = joystick.Vertical;

        upways = Input.GetAxis("Vertical");
        sideways = Input.GetAxis("Horizontal");

        if (canMove)
        {
            if (moveUp && (upways > 0))
            {
                transform.Translate(new Vector2(0, speed * upways) * Time.deltaTime, Space.World);
            }

            if (moveDown && (upways < 0))
            {
                transform.Translate(new Vector2(0, speed * upways) * Time.deltaTime, Space.World);
            }

            if (moveRight && (sideways > 0))
            {
                transform.Translate(new Vector2(speed * sideways, 0) * Time.deltaTime, Space.World);
            }

            if (moveLeft && (sideways < 0))
            {
                transform.Translate(new Vector2(speed * sideways, 0) * Time.deltaTime, Space.World);
            }
        }
    }

    // ================================================
    // Shots Behaviour ================================
    // ================================================
    public void setCanShot(bool b)
    {
        this.canShot = b;
    }

    private void checkShot()
    {
        if (Input.GetButton("Fire1") && mayShot && canShot)
        {
            shot();
            StartCoroutine(FireRate());
            // fireRateShot();
        }
    }


    private void shot()
    {
        GameObject _bullet = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
        _bullet.GetComponent<Bullet_Behaviour>().setGameObj(this.gameObject);
        _bullet.GetComponent<Bullet_Behaviour>().setDamage(this.shotDamage);
        _bullet.transform.tag = this.transform.tag;
        mayShot = false;
        // _bullet.transform.SetParent(this.transform);
    }

    private IEnumerator FireRate()
    {
        amount = fireRate;
        yield return new WaitForSeconds(fireRate);
        resetFireReady();
        mayShot = true;
    }

    private void fireRateShot()
    {
        // Se os dois nao forem verdadeiros
        if (!mayShot && !canShot) return;

        amount = fireRate;

        fireRateAux = -Time.deltaTime;

        if (fireRateAux <= 0)
        {
            resetFireReady();
            mayShot = true;
        }
    }

    // private void animateFireReady()
    // {
    //     if (!mayShot)
    //     {
    //         amount -= Time.deltaTime;
    //         shotImage.fillAmount = amount / fireRate;
    //     }
    // }

    private void resetFireReady()
    {
        amount = 1f;
        fireRateAux = fireRate;
    }

    // ================================================
    // BOMB BBEHAVIOUR ================================
    // ================================================
    private void updateBombImage()
    {
        bombImage.fillAmount = bombSpecial / bombSpecialFull;
    }

    private void checkBomb()
    {
        if ((bombSpecial >= bombSpecialFull))
        {
            if (Input.GetButtonDown("Fire2") && canShot && bombEnable)
            {
                useBomb();
                resetBombSpecial();
            }
        }
    }

    public void fillBombSpecial(float bombFill)
    {
        // Se bomba nao estiver habilitada, entao retorna
        if (!bombEnable) return;

        bombSpecial += bombFill * bombFillRate;
    }

    private void useBomb()
    {
        // Metodo que instancia a bomba na posição do mouse
        Vector3 mousePos;
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject bomb = Instantiate(bombGO, this.transform.position, Quaternion.identity);
        bomb.GetComponent<Bomb_Behaviour>().setExplosionLevel(explosionArea);
        bomb.GetComponent<Bomb_Behaviour>().setDamage(getDamage());

        //Instancia um colisor de bomba na posição do mouse para bomba explodir
        GameObject bombCollisor = Instantiate(bombColliderGO, mousePos, Quaternion.identity);

        bomb.GetComponent<Bomb_Behaviour>().setLocation(mousePos, bombCollisor);
    }

    private void resetBombSpecial()
    {
        this.bombSpecial = 0;
        // this.bombSpecial = bombSpecialFull;
    }


    // ================================================
    // LIFE BEHAVIOUR =================================
    // ================================================

    public void takeDamage(int damage)
    {
        if (!shieldUp)
        {
            this.life -= damage;
            StartCoroutine(TakingDamage());

            if (this.life <= 0)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
                game_Events.playerDied();
                Destroy(this.gameObject);
            }

            updateThisLifeText();
        }
    }

    private IEnumerator TakingDamage()
    {
        mySpriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        mySpriteRenderer.color = Color.white;
    }

    public void gainLife()
    {
        if (this.life < this.maxLife)
        {
            this.life += 1;
            updateThisLifeText();
        }
    }


    private void updateThisLifeText() => game_Events.OnPlayerLifeChanged?.Invoke(this.life);


    public int getLifePoints()
    {
        return this.life;
    }

    // ================================================
    // UPGRADE BEHAVIOUR ==============================
    // ================================================
    public void gainUpgradePoints()
    {
        this.upgradePoints += 1;
        updateThisUpgradeText();
    }

    public int getUpgradePoints()
    {
        return this.upgradePoints;
    }

    private void updateThisUpgradeText() => game_Events.OnPlayerUpgradeChanged?.Invoke(this.upgradePoints);

    // Vindo do boss ==========
    public void receiveUpgradeList(List<GameObject> list, int quant)
    {
        this.dropsGO = list;
        this.upgradePointsToReceive = quant;
    }

    public void upgradeReceived()
    {
        int maxUpgradeReceived = upgradePointsToReceive;

        if (upgradePointsToReceive >= maxUpgradeReceived)
        {
            // Chamar funcção no gameEvents para mudar de tela

            StartCoroutine(CallBossDead());
        }

        upgradePointsToReceive += 1;
    }

    private IEnumerator CallBossDead()
    {
        yield return new WaitForSeconds(timeToCallHangar);
        game_Events.bossDefeated();
    }

    // ================================================
    // SHIELD BEHAVIOUR ===============================
    // ================================================
    public void gainShield()
    {
        StartCoroutine(DesactiveShield());
    }

    private IEnumerator DesactiveShield()
    {
        this.shieldUp = true; // Fica invulneravel

        // gameObject.GetComponent<AudioSource>().clip = shieldSound;
        // gameObject.GetComponent<AudioSource>().Play();

        resetShieldEnd();
        yield return new WaitForSeconds(shieldTime);
        this.shieldUp = false;
        changeColorToOriginal();
    }

    private void changeColorToOriginal()
    {
        // gameObject.GetComponent<SpriteRenderer>().color = myColor;
        mySpriteRenderer.color = myColor;
        thisShotTime = 0f;
        colorIndex = 0;
    }

    private void changeColor()
    {
        if (this.shieldUp)
        {
            // gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(gameObject.GetComponent<SpriteRenderer>().color, colors[colorIndex], colorTime * Time.deltaTime);
            mySpriteRenderer.color = Color.Lerp(mySpriteRenderer.color, colors[colorIndex], colorTime * Time.deltaTime);

            thisShotTime = Mathf.Lerp(thisShotTime, 1f, colorTime * Time.deltaTime);
            if (thisShotTime > .99f)
            {
                thisShotTime = 0f;
                colorIndex++;
                colorIndex = (colorIndex >= colors.Length) ? 0 : colorIndex;
            }
        }
    }

    private void animateShieldEnd()
    {
        if (this.shieldUp)
        {
            endShield = endShield + Time.deltaTime; // End Shield é o tempo que acaba o shield
            thisShieldTime = endShield / shieldTime;
            shieldImage.fillAmount = thisShieldTime;
        }
    }

    private void resetShieldEnd()
    {
        endShield = 0f;
        shieldImage.fillAmount = endShield;
    }


    // =============================
    // GETTERS
    // =============================

    public float getSpeed()
    {
        return this.speed;
    }

    public int getDamage()
    {
        return this.shotDamage;
    }
}