using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Controller : BossController
{

    [SerializeField] Boss3Shielding leftShielding;
    [SerializeField] Boss3Shielding rightShielding;
    float leftSpawnTimer;
    float rightSpawnTimer;
    List<GameObject> spawnableShips = new List<GameObject>();

    private void Start()
    {
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z))
        {
            SpawnShip(0);
        }
        if(Input.GetKeyDown(KeyCode.X))
        {
            SpawnShip(1);
        }
    }


    void SpawnShip(int side)
    {
        StallTime(7.5f);
        if(side == 0)
        {
            leftShielding.Open();
        }
        else
        {
            rightShielding.Open();
        }
    }
}
