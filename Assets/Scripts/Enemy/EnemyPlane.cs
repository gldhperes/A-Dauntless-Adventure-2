using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyPlane : Enemy_Behaviour
{
    [SerializeField ]private float angle;
    [SerializeField] private List<Vector2> waypoints;
    [ReadOnly] [SerializeField] private float t;
    [SerializeField] private bool shoted;
    [SerializeField] private float shotRange;

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
        speed = speed + (.2f * level);
        fireRate = fireRate - (.1f * level);
        damage *= level;
    }

    void Update()
    {
        Move();
        
        // timerToShot();
    }

    private void Move()
    {
        if (t < 1f)
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
               
                if (t >= shotRange )
                {
                    Shot();
                    shoted = true;
                }

            }
        }
        else if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }

    // private void timerToShot()
    // {
    //     var timer_to_shot = Random.Range(0, 3);
    //     if (!shoted)
    //     {
    //         if (timer > timer_to_shot)
    //         {
    //             timer -= Time.deltaTime;
    //         }
    //         else
    //         {
    //             shoted = true;
    //             resetTimerToShot();
    //             shot();
    //         }
    //     }
    //}


    private void Shot()
    {
        GameObject _bullet = Instantiate(bullet, this.transform.position, this.transform.rotation) as GameObject;
        _bullet.GetComponent<Bullet_Behaviour>().setGameObj(this.gameObject as GameObject);
        _bullet.GetComponent<Bullet_Behaviour>().setDamage(this.damage);
        _bullet.transform.tag = this.transform.tag;
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