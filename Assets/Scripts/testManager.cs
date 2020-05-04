using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testManager : MonoBehaviour
{
    public static testManager tm;

    public int heldSeeds;

    Bounds spawnArea;

    public float checkDistance = 1f;
    public int initialFoodCount;
    public GameObject seedPrefab;

    private void Awake()
    {
        tm = this;
        spawnArea = GetComponent<Collider>().bounds;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < initialFoodCount; i++)
        {
            Vector3 randPoint;
            // Gets a random point. Repeats if there is food or an obstacle too close
            do
            {
                randPoint = new Vector3(
                    Random.Range(spawnArea.min.x, spawnArea.max.x),
                    transform.position.y,
                    Random.Range(spawnArea.min.z, spawnArea.max.z)
                );
            } while (Physics.CheckSphere(randPoint, checkDistance, 1 << 8)); // Invalid check
            Instantiate(seedPrefab, randPoint, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
