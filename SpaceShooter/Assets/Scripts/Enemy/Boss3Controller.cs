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

    private void Update()
    {
        leftSpawnTimer 
    }
    void SpawnShip(int side)
    {
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
