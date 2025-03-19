using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Behaviour : MonoBehaviour
{   
    [Header("Auto Config")]
    [SerializeField]
    private Camera mainCam;
    [SerializeField]
    private float upBound;
    [SerializeField]
    private float downBound;
    [SerializeField]
    private float rightBound;
    [SerializeField]
    private float leftBound;
    
    [Header("Manual Config")]
    [SerializeField]
    private float sideBoundOffset;
    [SerializeField]
    private float upDownBoundOffset;
   

    void Awake(){
        setCameraBounds();
    }

    private void setCameraBounds(){
        mainCam = gameObject.GetComponent<Camera>();
        upBound = (mainCam.transform.position.y + mainCam.orthographicSize) - upBound ;
        downBound = (mainCam.transform.position.y - mainCam.orthographicSize) + upDownBoundOffset;      
        rightBound = (mainCam.transform.position.x + mainCam.orthographicSize) - sideBoundOffset;
        leftBound = (mainCam.transform.position.x - mainCam.orthographicSize) + sideBoundOffset;
    }

    public float getUpBound(){ return this.upBound; }
    public float getDownBound(){ return this.downBound; }
    public float getRightBound(){ return this.rightBound; }
    public float getLeftBound(){ return this.leftBound; }
}
