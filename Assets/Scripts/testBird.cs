using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testBird : MonoBehaviour
{
    public float moveSpeed;
    public float turnSpeed;
    void FixedUpdate()
    {
        transform.position += transform.forward * moveSpeed * Time.fixedDeltaTime;
        transform.Rotate(0, Input.GetAxis("Horizontal") * turnSpeed * Time.fixedDeltaTime, 0);
    }
}
