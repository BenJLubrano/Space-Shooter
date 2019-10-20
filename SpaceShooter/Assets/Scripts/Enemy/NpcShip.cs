using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script that contains data for the different types of NPCs
//NpcController will consult this script for directions, they will both be on the same gameObject
public class NpcShip : MonoBehaviour
{
    public float health = 10f;
    public float aggroRange = 10f;

    //Will control the movement for the NPC, but will be called by NpcController
    public void Move()
    {

    }

}
