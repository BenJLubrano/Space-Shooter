using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Controller : BossController
{

    [SerializeField] Boss3Shielding leftShielding;
    [SerializeField] Boss3Shielding rightShielding;
    [SerializeField] LaserTurret leftLaser;
    [SerializeField] LaserTurret rightLaser;
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

        if(Input.GetKeyDown(KeyCode.C))
        {
            leftLaser.FireLaser(5);
            rightLaser.FireLaser(5);
            StallTime(10);
        }
        
        if(currentTarget != null)
        {
            leftLaser.SetTarget(currentTarget);
            rightLaser.SetTarget(currentTarget);
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
