using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island_Behaviour : MonoBehaviour
{
    [SerializeField]private Transform islandLimit;
    void Start()
    {
        islandLimit = GameObject.Find("IslandLimit").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(islandLimit != null){
            if( this.gameObject.transform.position.y <= islandLimit.transform.position.y) { 
                Destroy(this.gameObject); 
            }
        }
    }
}
