using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSpawner : MonoBehaviour
{
    [SerializeField] float spawnRate = 5;
    [SerializeField] int maxShips = 10;
    [SerializeField] BoxCollider2D spawnArea;
    [SerializeField] List<GameObject> ships;

    float timeSinceLastSpawn = 0f;
    int spawnedShips = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += Time.deltaTime;
        if(timeSinceLastSpawn >= spawnRate && spawnedShips < maxShips)
        {
            SpawnShip();
        }
    }

    void SpawnShip()
    {
        Vector3 spawnPosition = transform.position;
        spawnPosition.x += Random.Range(-(spawnArea.size.x / 2), spawnArea.size.x / 2);
        spawnPosition.y += Random.Range(-(spawnArea.size.y / 2), spawnArea.size.y / 2);
        Instantiate(ships[Random.Range(0, ships.Count)], spawnPosition, Quaternion.identity);
        timeSinceLastSpawn = 0f;
        spawnedShips += 1;
    }
}
