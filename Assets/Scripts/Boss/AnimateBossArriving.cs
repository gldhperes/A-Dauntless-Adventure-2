using System.Collections;
using Unity.Collections;
using UnityEngine;

public class AnimateBossArriving : MonoBehaviour
{

    [SerializeField] private Transform bossSpawnPosition;
    [SerializeField] private Boss_Behaviour bossBehaviour;
    [ReadOnly][SerializeField] private float t;

    private void Start()
    {
        bossBehaviour = GetComponent<Boss_Behaviour>();
    }


    public void SetSpanwBossPosition(Transform bossSpawnPos, float animationTime)
    {
        this.bossSpawnPosition = bossSpawnPos;
        this.transform.position = this.bossSpawnPosition.position;

        // Debug.Log("Spawn Position: " + this.bossSpawnPosition.position);

        // Debug.Log("waypoints Position: " + bossBehaviour.waypoints[0].position);

        if (this.gameObject.name.Contains("Boss 4"))
        {

            Transform boss4Spawn = GameObject.FindGameObjectWithTag("Boss4Spawn").transform;

            StartCoroutine(MoveOverTime(this.bossSpawnPosition.position, boss4Spawn.position, animationTime));

        }
        else
        {
            Transform bossSpawn = GameObject.FindGameObjectWithTag("BossWaypoints").transform;

            StartCoroutine(MoveOverTime(this.bossSpawnPosition.position, bossSpawn.transform.GetChild(0).position, animationTime));
        }
    }


    // void Update()
    // {
    //     Animate();
    // }
    IEnumerator MoveOverTime(Vector2 start, Vector2 end, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            t = Mathf.SmoothStep(0f, 1f, t); // EaseIn/EaseOut

            this.transform.position = Vector2.Lerp(start, end, t);

            elapsed += Time.deltaTime;
            yield return null;
        }

        bossBehaviour.bossArrived = true;
        bossBehaviour.setBossArrived(true);
        bossBehaviour.game_Events.getPlayer().GetComponent<Player_Behavior>().setCanShot(true);
        Destroy(this);
    }




    // private void Animate()
    // {
    //     //Se nao foi setado entao retorna
    //     if (bossSpawnPosition == null) return;

    //     Calculate_T();

    //     if (this.gameObject.name.Contains("Boss 4"))
    //     {
    //         if (!bossBehaviour.boss4arrived)
    //         {
    //             transform.position =
    //                 Vector2.MoveTowards(this.bossSpawnPosition.position, bossBehaviour.boss4Spawn.position, bossBehaviour.speed * Time.deltaTime);

    //             if (t >= 1f)
    //             {
    //                 bossBehaviour.boss4arrived = true;
    //                 bossBehaviour.setBossArrived(true);
    //                 bossBehaviour.game_Events.getPlayer().GetComponent<Player_Behavior>().setCanShot(true);
    //                 Destroy(this);
    //             }
    //         }
    //     }
    //     else
    //     {
    //         if (!bossBehaviour.bossArrived)
    //         {

    //             // Move
    //             this.transform.position = Vector2.Lerp(this.bossSpawnPosition.position, bossBehaviour.waypoints[0].position, t);

    //             if (t >= 1f)
    //             {
    //                 bossBehaviour.bossArrived = true;
    //                 bossBehaviour.setBossArrived(true);
    //                 bossBehaviour.game_Events.getPlayer().GetComponent<Player_Behavior>().setCanShot(true);
    //                 Destroy(this);
    //             }
    //         }
    //     }
    // }

    private void Calculate_T()
    {
        if (t < 1f)
        {
            t = Mathf.MoveTowards(t, 1f, bossBehaviour.speed * Time.deltaTime);
        }

    }
}
