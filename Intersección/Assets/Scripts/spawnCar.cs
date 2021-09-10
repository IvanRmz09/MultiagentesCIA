using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnCar : MonoBehaviour
{
    private float nextSpawnTime;

    [SerializeField] public GameObject car;
    [SerializeField] private float spawnDelay;

    private void Update()
    {
        if (ShouldSpawn())
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        nextSpawnTime = Time.time + spawnDelay;
        Instantiate(car, transform.position, Quaternion.identity);
    }
    
    private bool ShouldSpawn()
    {
        return Time.time >= nextSpawnTime;
    }
}
