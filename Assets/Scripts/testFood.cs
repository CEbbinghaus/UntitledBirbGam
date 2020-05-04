using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testFood : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            testManager.tm.heldSeeds++;
            Debug.Log($"Collected a seed! Now carrying {testManager.tm.heldSeeds} seeds");
            Destroy(gameObject);
        }
    }
}
