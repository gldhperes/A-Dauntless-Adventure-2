using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Behaviour : MonoBehaviour
{
    [Header ("Components")]
    public float bombSpeed;
    
    [Tooltip("Definido pelo Player o dano da bomba")]
    [SerializeField] private int damage;
    public float explosionArea;
    public float angle;
    public float offset;

    [Header ("Explosion Settings")]
    public GameObject bombColliderGO;    
    public GameObject explosionGO;
    [Range (0, 0.15f)] public float distOffset;
    

    public void setExplosionLevel(float area){
        this.explosionArea = area;
    }

    public void setDamage(int damage){
        this.damage= damage;
    }

    public void setSpeed( float bombSpeed ){
        this.bombSpeed = bombSpeed;
    }

    public void setLocation(Vector3 mousePos, GameObject bombColliderPosition)
    {
        this.bombColliderGO = bombColliderPosition;
        Vector3 targetPos = mousePos;
        Vector3 thisPos = transform.position;
        // targetPos.x = thisPos.x - targetPos.x;
        targetPos.x = targetPos.x - thisPos.x;
        // targetPos.y = thisPos.y - targetPos.y;
        targetPos.y = targetPos.y - thisPos.y;
        
        // Pega o valor da Tangente e trasnforma em angulo
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        // Debug.Log("mousePos: " + targetPos);
        // Debug.Log("bomb: " + thisPos);
        // Debug.Log("x: " + targetPos.x);
        // Debug.Log("y: " + targetPos.y);
        // Debug.Log("Tan: " + Mathf.Atan2(targetPos.y, targetPos.x) );
        // Debug.Log("Angle: " + angle);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - offset));
    }
    
  
    void Update(){
        move();
        checkColliderPosition();
        checkUnbounds();
    }

    private void checkColliderPosition() {
        float dist = Vector3.Distance(bombColliderGO.transform.position, transform.position);
        if(dist <= distOffset){
            bombHit();
        }
        
    }

    private void checkUnbounds()
    {
        float upBound = (Camera.main.transform.position.y + Camera.main.orthographicSize) ;
        float downBound = (Camera.main.transform.position.y - Camera.main.orthographicSize) ;
        float rightBound = (Camera.main.transform.position.x + Camera.main.orthographicSize*2 ) ;
        float leftBound = (Camera.main.transform.position.x - Camera.main.orthographicSize*2) ;
        if (this.gameObject.transform.position.y >= upBound){
            Destroy(this.gameObject);
        }else if (this.gameObject.transform.position.y <= downBound){
            Destroy(this.gameObject);
        }else if (this.gameObject.transform.position.x >= rightBound){
            Destroy(this.gameObject);
        }else if (this.gameObject.transform.position.y <= leftBound){
            Destroy(this.gameObject);
        }
    }

    

    private void move() {
        transform.Translate(Vector3.up * bombSpeed * Time.deltaTime, Space.Self);
        Destroy(this.gameObject, 5f);
    }

    private void bombHit() {
        Destroy(bombColliderGO);

        GameObject explosion = Instantiate(explosionGO, new Vector2( this.transform.position.x, this.transform.position.y), Quaternion.identity);
        explosion.GetComponent<BombExplosion_Behaviour>().setScale( explosionArea );
        explosion.GetComponent<BombExplosion_Behaviour>().setDamage( this.damage );
        
        Destroy(this.gameObject);
    }
    
}
