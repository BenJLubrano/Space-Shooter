using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : NpcController
{
    [SerializeField] BossController boss;
    [SerializeField] Vector2 angleClamp;
    [SerializeField] float defaultRotation;

    float timeOutOfRange = 0f;
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
        if (currentTarget != null && TargetInAttackRange() && weaponCooldown <= 0 && !isDead && IsBetween(angleClamp.x, angleClamp.y, AngleToTargetOffset()))//AngleToTargetOffset() > angleClamp.x && AngleToTargetOffset() < angleClamp.y)
        {
            //Turn this into a method in Ship.cs called "Shoot()" that will handle the cooldown setting etc, since it's universal for all ships
            Shoot(currentTarget, enemyFactions);
        }

        TurretRotate();
    }

    void TurretRotate()
    {
        if (currentTarget == null || OutOfRange())
        {
            ResetRotation();
            return;
        }

        if (!TargetInAttackRange())
        {
            if (boss.IsMoving())
            {
                timeOutOfRange += Time.deltaTime;
                if (timeOutOfRange > 1f)
                {
                    ResetRotation();
                    timeOutOfRange = 0f;
                    return;
                }
            }
            else
            {
                ResetRotation();
            }
        }
        
        float angle = AngleToTarget();
        Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        if(!IsBetween(angleClamp.x, angleClamp.y, AngleToTargetOffset()))
        {
            if (boss.IsMoving())
            {
                timeOutOfRange += Time.deltaTime;
                if (timeOutOfRange > 1f)
                {
                    ResetRotation();
                    timeOutOfRange = 0f;
                }
            }
            else
            {
                ResetRotation();
            }
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        }
        
    }

    void ResetRotation()
    {
        Quaternion rotation = transform.parent.rotation * Quaternion.Euler(0, 0, defaultRotation - 90);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
    }

    float AngleToPoint(Vector3 point)
    {
        Vector2 direction = point - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
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

    bool IsBetween(float min, float max, float angle)
    {
        if(min > max)
        {
            if(angle < min && angle > max)
            {
                return false;
            }
        }
        else
        {
            if(angle < min || angle > max)
            {
                return false;
            }
        }
        return true;
    }

    public void SetBoss(BossController boss)
    {
        this.boss = boss;
    }
}
