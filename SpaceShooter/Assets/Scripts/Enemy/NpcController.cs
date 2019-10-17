﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

//Controller for all NPC ships, this script will act as a driver or central control unit for the ships
//It will get the variables from NpcShip.cs and will call functions from that script.
public class NpcController : Ship
{

    [SerializeField] NpcShip npcType; //the type of NPC that the ship is (This might be changed)
    [SerializeField] List<GameObject> targets;
    [SerializeField] List<string> enemyFactions;

    [Header("References")]
    [SerializeField] TargetingController targetingController;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Collider2D collider;
    [SerializeField] SpriteRenderer renderer;

    GameObject currentTarget;

    private void Awake()
    {
        //initialize enemyfactions  
        targetingController.Initialize(enemyFactions);
        try
        {
            shipId = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RegisterShip();
        }
        catch
        {
            Debug.LogError("GameManager was not detected in the scene!");
        }
    }

    void Update()
    {
        base.Update(); //call base ship update
        if(currentTarget == null)
        {
            LookForTargets();
        }
        else
        {
            if(OutOfRange())
            {
                Deaggro();
            }
        }


        Move();

        //if the ship has a target
        if(currentTarget != null && TargetInAttackRange() && weaponCooldown <= 0)
        {
            //Turn this into a method in Ship.cs called "Shoot()" that will handle the cooldown setting etc, since it's universal for all ships
            Shoot(currentTarget, enemyFactions);
        }
    }

    //Logic related to targeting
    void LookForTargets()
    {
        if (targets.Count == 0)
            return;
        GameObject tempTarget = targets[0];
        float closest = Vector2.Distance(gameObject.transform.position, tempTarget.transform.position);
        foreach(GameObject target in targets)
        {
            float tempDist = Vector2.Distance(gameObject.transform.position, target.transform.position);
            if (tempDist < closest)
            {
                tempTarget = target;
                closest = tempDist;
            }
        }
        currentTarget = tempTarget;
    }

    //Used to get target updates from the TargetingController
    public void UpdateTargets(List<GameObject> newTargets)
    {
        targets = newTargets;
    }

    //Whether or not the target is out of aggro range
    bool OutOfRange()
    {
        return Vector2.Distance(currentTarget.gameObject.transform.position, gameObject.transform.position) > npcType.aggroRange;
    }

    //Whether or not the target is in range to attack. Might do some more complicated calculations here later?
    bool TargetInAttackRange()
    {
        return Vector2.Distance(currentTarget.transform.position, transform.position) <= shipWeapon.range;
    }

    //Called when the npc "loses interest" in a target
    void Deaggro()
    {
        //going to do other stuff here later, like returning to patrol point
        currentTarget = null;
    }
    
    void Move()
    {
        if (currentTarget != null)
        {
            Rotate();

            if (Vector2.Distance(currentTarget.gameObject.transform.position, transform.position) > shipWeapon.range / 2) //if the target is further away than half of the weapons range
            {
                rb.AddForce(transform.up * speed);
            }
        }
    }

    //this function handles the rotation of the enemy
    void Rotate()
    {
        Vector2 direction = currentTarget.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);

        //CODE FOR INSTANT ROTATE
        //transform.up = currentTarget.gameObject.transform.position - transform.position; //change rotation to face target
    }

    protected override void Die()
    {
        collider.enabled = false;
        renderer.enabled = false;
        base.Die();
    }
}
