using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;


public class EnemyPlane : Enemy_Behaviour
{
    [SerializeField] private float angle;
    [SerializeField] private List<Vector2> waypoints;
    [ReadOnly][SerializeField] private float t;
    [SerializeField] private bool shoted;
    [SerializeField] private float shotRange;

    [Header("Rotation Animations")]
    [SerializeField] private float rotationDuration;
    [SerializeField] private float rotationDegree;

    void Start()
    {
        this.shotRange = Random.Range(.2f, .7f);
        game_Events = GameObject.FindWithTag("Game_Events").GetComponent<Game_Events>();
        player = game_Events.getPlayer();
        bullet = game_Events.getEnemyBullet();
        mainCam = game_Events.getMainCam();
        setStats();
    }

    public void SetWaypoints(GameObject waypointsMoveSet)
    {
        Vector2 p0 = waypointsMoveSet.transform.GetChild(0).position;
        waypoints.Add(p0);

        Vector2 p1 = waypointsMoveSet.transform.GetChild(1).position;
        waypoints.Add(p1);

        Vector2 p2 = waypointsMoveSet.transform.GetChild(2).position;
        waypoints.Add(p2);
    }

    private void setStats()
    {
        if (gameObject.transform.tag != "Enemy") return;

        level = game_Events.getGameLevel();
        life *= level;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (fromBoss)
        {

            transform.Translate(Vector3.up * 10f * Time.deltaTime);


        }
        else if (t < 1f)
        {
            t = Mathf.MoveTowards(t, 1f, speed * Time.deltaTime);
            // Do the lerp calculation
            var posA = Vector2.Lerp(waypoints[0], waypoints[1], t);
            var posB = Vector2.Lerp(waypoints[1], waypoints[2], t);

            // Move
            transform.position = Vector2.Lerp(posA, posB, t);

            // Shot randonly by T moviment
            if (!shoted)
            {

                if (t >= shotRange)
                {
                    Shot();
                    AnimateRotationAfterShot();
                    shoted = true;
                }

            }
        }
        else if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }




    public void Shot()
    {
        GameObject _bullet = Instantiate(bullet, this.transform.position, this.transform.rotation) as GameObject;
        _bullet.GetComponent<Bullet_Behaviour>().setGameObj(this.gameObject as GameObject);
        _bullet.GetComponent<Bullet_Behaviour>().setDamage(this.damage);
        _bullet.transform.tag = this.transform.tag;
    }

    private void AnimateRotationAfterShot()
    {


        float targetY = 0f;

        if (waypoints[0].x < waypoints[2].x)
            targetY = rotationDegree;
        else if (waypoints[0].x > waypoints[2].x)
            targetY = -rotationDegree;


        StopAllCoroutines();
        StartCoroutine(RotarSuavemente(targetY, rotationDuration)); // 0.5s de duração, por exemplo
    }

    private IEnumerator RotarSuavemente(float targetY, float duracao)
    {
        float tempo = 0f;

        // Captura apenas o Y atual
        float initialY = transform.eulerAngles.y;
        float initialZ = transform.eulerAngles.z;

        // Corrige ângulos para evitar rotação errada (ex: 350° → -10°)
        if (initialY - targetY > 180f) targetY += 360f;
        else if (targetY - initialY > 180f) initialY += 360f;

        while (tempo < duracao)
        {
            float y = Mathf.Lerp(initialY, targetY, tempo / duracao);

            transform.rotation = Quaternion.Euler(0f, y, initialZ); // <-- Só Y rotaciona

            tempo += Time.deltaTime;
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0f, targetY, initialZ);
    }







    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            try
            {
                player.GetComponent<Player_Behavior>().takeDamage(life);
                life = 0;
                CheckLife();
            }
            catch
            {

            }
        }
    }
}