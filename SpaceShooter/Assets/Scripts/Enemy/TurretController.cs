using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : NpcController
{
    [SerializeField] Vector2 angleClamp;
    [SerializeField] float defaultRotation;
    private void Awake()
    {
        base.Awake();
        
        targetingController.Initialize(enemyFactions);
    }

    private void Update()
    {
        DoUpdateChecks();
        if (currentTarget == null)
        {
            LookForTargets();
        }
        else
        {
            if (OutOfRange() || currentTarget.GetComponent<Ship>().IsDead())
            {
                Deaggro();
            }
        }

        //if the ship has a target
        if (currentTarget != null && TargetInAttackRange() && weaponCooldown <= 0 && !isDead && AngleToTarget() > angleClamp.x && AngleToTarget() < angleClamp.y)
        {
            //Turn this into a method in Ship.cs called "Shoot()" that will handle the cooldown setting etc, since it's universal for all ships
            Shoot(currentTarget, enemyFactions);
        }

        //Debug.Log(Vector2.SignedAngle(transform.up, currentTarget.transform.position - transform.position));
        Rotate(angleClamp.x, angleClamp.y, defaultRotation);
    }
}
