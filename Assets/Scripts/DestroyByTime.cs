using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    [Tooltip("Tempo de vida do objeto. O valor é dado pelo tempo da animação")]
    [SerializeField] private float lifeTime;
    void Start()
    {
        Destroy(this.gameObject, lifeTime);
    }
}
