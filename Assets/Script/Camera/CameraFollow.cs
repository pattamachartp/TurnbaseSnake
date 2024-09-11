using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    private float velocity;
    private float maxSpeed = 0.005f;

    void LateUpdate()
    {
        if (target == null)
            return;

        if(transform.position.x != target.position.x || transform.position.y != target.position.y)
        transform.position = new Vector3(
          Mathf.SmoothDamp(transform.position.x, target.position.x , ref velocity, maxSpeed)
            , Mathf.SmoothDamp(transform.position.y, target.position.y, ref velocity, maxSpeed)
            , transform.position.z);
    }   
}
