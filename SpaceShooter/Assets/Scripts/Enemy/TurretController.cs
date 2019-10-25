using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : NpcController
{
    [SerializeField] GameObject boss;
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
        if (currentTarget != null && TargetInAttackRange() && weaponCooldown <= 0 && !isDead && AngleToTargetOffset() > angleClamp.x && AngleToTargetOffset() < angleClamp.y)
        {
            //Turn this into a method in Ship.cs called "Shoot()" that will handle the cooldown setting etc, since it's universal for all ships
            Shoot(currentTarget, enemyFactions);
        }

        //Debug.Log(Vector2.SignedAngle(transform.up, currentTarget.transform.position - transform.position));
        //Rotate(angleClamp.x, angleClamp.y, defaultRotation);
        TurretRotate();
        //AngleToBoss();
    }

    void TurretRotate()
    {
        float angle = AngleToTarget();
        float offsetAngle = AngleToTargetOffset();
        Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        if (offsetAngle > angleClamp.y || offsetAngle < angleClamp.x) //TODO: It would be good to set the rotation back to whatever it was supposed to be after it's done rotating back, but also it might just be better to add an enclosing gameobject
        {
            angle = defaultRotation;
            rotation = transform.parent.rotation * Quaternion.Euler(0, 0, defaultRotation);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
    }

    //May not need this
    float AngleToBoss()
    {
        Vector2 direction = currentTarget.transform.position - boss.transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //float offset = boss.transform.eulerAngles.z;
        //offset = offset < 0 ? offset + 360 : offset;
        //angle -= offset;
        angle = angle < 0 ? angle + 360 : angle;
        return angle < 0 ? angle + 360 : angle;
    }

    float AngleToTargetOffset()
    {
        Vector2 direction = currentTarget.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float offset = boss.transform.eulerAngles.z;
        offset = offset < 0 ? offset + 360 : offset;
        angle -= offset;
        angle = angle < 0 ? angle + 360 : angle;
        return angle < 0 ? angle + 360 : angle;
    }

    //This problem appears to be fixed... don't know how.. More testing required.
    //The problem with the rotation has to do with the turrets being placed at an offset on the boss ship.
    //What needs to happen is, we have to somehow calculate the angle of the player from the middle of the ship. This could possibly be done by simply getting the angle to target of the the boss to the player
}
