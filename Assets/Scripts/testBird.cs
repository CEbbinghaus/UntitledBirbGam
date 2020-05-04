using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBird : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    void FixedUpdate()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, 26, transform.position.z);
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.fixedDeltaTime, 0);
        transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
    }
}
