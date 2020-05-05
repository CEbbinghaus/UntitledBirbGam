using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnable : MonoBehaviour
{
    static Spawning spawnScript;

    void Start()
    {
        if(spawnScript == null)
            spawnScript = FindObjectOfType<Spawning>();

        if(Random.Range(0f, 1f) > spawnScript.PercentageSpawn)
            gameObject.SetActive(false);
    }
}
