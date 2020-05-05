using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public Vector2 Bounds;
    public Vector3 Offset;
    public Transform target;
    public List<Transform> Targets;

    public float minDistance = 5f;
    public float buffer = 0.5f;
    public float snap = 2;

    Camera camera;
    // Start is called before the first frame update
    void Start(){
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate(){
        if(target == null)return;


        //var frustumHeight = 2.0f * difference * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);

        Vector2 maxSize = Vector2.zero;


        foreach(Transform t in Targets){
            Vector3 dist = (target.position - t.transform.position);

            if(Mathf.Abs(dist.x) > maxSize.x)
                maxSize.x = Mathf.Abs(dist.x);
           
            if(Mathf.Abs(dist.z) > maxSize.y)
                maxSize.y = Mathf.Abs(dist.z);
        }

        maxSize *= (2 + buffer);
        float height = maxSize.y;
        if(maxSize.x > maxSize.y * camera.aspect){
            height = maxSize.x / camera.aspect;
        }
   
        if(height <= 0){
            height = 2.0f * minDistance * Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        }


        var distance = height * 0.5f / Mathf.Tan(camera.fieldOfView * 0.5f * Mathf.Deg2Rad);
        
        if(distance < minDistance)
            distance = minDistance;

        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, distance, target.position.z), Time.deltaTime * snap);



        //Debug.Log(frustumHeight);
    }

    private void OnDrawGizmos() {
        if(target != null && Bounds.magnitude > 0){
            Gizmos.DrawWireCube(Offset, new Vector3(Bounds.x, 1, Bounds.y));
        }    
    }
}
