using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekingProjectile : Projectile
{
    float lifespan;
    float followRange;
    float turnSpeed;
    float maxTurnSpeed;
    float flyStraightTime;
    float missileSpeed;
    AudioClip explosionSound;
    bool flyingStraight = true;
    float currentLifeSpan = 0f;
    Rigidbody2D missileRb;

    float lastDistanceToTarget;
    float turnSpeedIncrease = .1f;
    public override void Initialize(Weapon weapon, ShipController shooter, GameObject target = null, List<string> factions = null)
    {
        base.Initialize(weapon, shooter, target, factions);
        SeekingWeapon seekingWeapon = (SeekingWeapon)weapon;
        lifespan = seekingWeapon.lifespan;
        followRange = seekingWeapon.followRange;
        turnSpeed = seekingWeapon.turnSpeed;
        maxTurnSpeed = seekingWeapon.maxTurnSpeed;
        flyStraightTime = seekingWeapon.flyStraightTime;
        missileSpeed = seekingWeapon.projectileSpeed;
        explosionSound = seekingWeapon.explosionSound;
        missileRb = GetComponent<Rigidbody2D>();
        
    }

    protected override void Update()
    {
        float distanceToTarget = Vector2.Distance(transform.position, target.transform.position);
        currentLifeSpan += Time.deltaTime;

        if (target != null && Vector2.Distance(transform.position, target.transform.position) > followRange)
        {
            target = null;
            flyingStraight = true;
        }

        if (flyingStraight && currentLifeSpan > flyStraightTime && target != null)
            flyingStraight = false;

        if(!flyingStraight)
        {
            Vector2 direction = transform.position - target.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            Quaternion rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
            turnSpeed = AdjustedTurnSpeed(turnSpeed, distanceToTarget, maxTurnSpeed);
            Debug.Log(turnSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        }

        missileRb.AddForce(transform.up * missileSpeed * 100 * missileRb.drag * Time.deltaTime);

        if (waitingForDestroy)
        {
            if (!audioSource.isPlaying)
            {
                Destroy(gameObject);
            }
        }
        lastDistanceToTarget = distanceToTarget;
    }

    float AdjustedTurnSpeed(float speed, float distance, float max = -1)
    {
        float tempSpeed = turnSpeed;
        if (Mathf.Abs(lastDistanceToTarget - distance) < .2 && distance < 2)
        {
            turnSpeedIncrease += 5 * Time.deltaTime;
            tempSpeed += tempSpeed * turnSpeedIncrease * Time.deltaTime;
        }
        return tempSpeed > max && max > 0 ? max : tempSpeed;
    }

    protected override void LateUpdate()
    {
        if (currentLifeSpan > lifespan && !waitingForDestroy)
        {
            Deactivate();
        }
    }

    protected override void Deactivate()
    {
        audioSource.clip = explosionSound;
        audioSource.Play();
        base.Deactivate();
        OnHit();
    }

}
