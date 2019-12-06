﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

//Controller for all NPC ships, this script will act as a driver or central control unit for the ships
//It will get the variables from NpcShip.cs and will call functions from that script.
public class NpcController : ShipController
{
    [SerializeField] protected List<GameObject> targets;
    [SerializeField] public List<string> enemyFactions;
    [SerializeField] protected TargetingController targetingController;
    [SerializeField] protected bool usePrediction = true;
    
    //We can just remove the "targets" gameobject and substitute it for this. The targeting controller will have to by modified but it will work.
    //Basically, whenever a ship enters the aggro zone (or attacks the ship) they get added to an aggro table, and when they leave the aggro zone, they get removed.
    //On attacking a ship, the initial aggro value should be damage - playerReputation, so that high rep players don't immediately get aggro when attacking federation ships
    protected AggroTable aggroTable = new AggroTable();
    [SerializeField] List<AggroElement> aggroElements = new List<AggroElement>();

    float lastFrameVelocity;
    protected ShipController currentTarget;
    protected Vector2 targetPosition;
    protected Rigidbody2D targetRb;
    protected float lastDistanceToTarget = 1000f;

    private void Awake()
    {
        base.Awake();
        aggroTable.Initialize(this);
    }
    protected override void Update()
    {
        base.Update(); //call base ShipController update

        LookForTargets();
        aggroTable.ReduceAllAggro(Time.deltaTime / 5);

        if (currentTarget != null && (OutOfRange() || currentTarget.IsDead() || !aggroTable.IsInTable(currentTarget)))
        {
            Deaggro();
        }

        //if the ShipController has a target
        if (currentTarget != null && TargetInWeaponRange() && weaponCooldown <= 0 && !isDead && TargetWithinShootAngle())
        {
            Shoot(currentTarget.gameObject, enemyFactions);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (currentTarget != null)
        {
            if(usePrediction && targetRb != null)
            {
                targetPosition = PredictTargetLocation();
            }
            else
            {
                targetPosition = currentTarget.transform.position;
            }

        }

        Move();
    }

    public override void Initialize(ShipStats newStats)
    {
        base.Initialize(newStats);
        UpdateFactions();

        targetingController.Initialize(enemyFactions);

        if(shipRb != null)
            speed = (speed * shipRb.drag);
    }

    public override void SetStats(ShipStats shipStats)
    {
        base.SetStats(shipStats);
        UpdateFactions();
    }

    public override void UpdateFactions()
    {
        enemyFactions.Clear();
        if (stats.reputation > 10)
            enemyFactions.Add("Pirate");
        else if (stats.reputation < 0)
        {
            enemyFactions.Add("Federation");
            enemyFactions.Add("Neutral");
        }
        //enemyFactions.Remove(stats.faction);
    }

    void HandleAnimation()
    {
        //animations for npcs go here
    }

    //Logic related to targeting
    protected virtual void LookForTargets()
    {
        AggroElement tempTarget = aggroTable.GetTopAggro();
        if (tempTarget == null)
            return;
        currentTarget = tempTarget.ship;
        targetRb = currentTarget.GetComponent<Rigidbody2D>();

        /*float currentShortestDistance = Vector2.Distance(transform.position, tempTarget.ship.transform.position);
        foreach(AggroElement aggroTarget in aggroTable.GetElements())
        {
            float tempDist = Vector2.Distance(transform.position, aggroTarget.ship.transform.position);
            if (tempDist < currentShortestDistance)
            {
                currentShortestDistance = tempDist;
                tempTarget = aggroTarget;
            }
        }*/

        /*
        float closest = Vector2.Distance(gameObject.transform.position, tempTarget.transform.position);
        foreach(GameObject target in targets) //TODO: Add something that has to do with aggro tables here
        {
            float tempDist = Vector2.Distance(gameObject.transform.position, target.transform.position);
            if (tempDist < closest)
            {
                tempTarget = target;
                closest = tempDist;
            }
        }
        currentTarget = tempTarget;
        */
    }

    //Used to get target updates from the TargetingController
    public virtual void UpdateTargets(List<GameObject> newTargets)
    {
        targets = newTargets;
        if(!targets.Contains(currentTarget.gameObject))
        {
            currentTarget = null;
        }
    }

    //Whether or not the target is out of aggro range
    protected bool OutOfRange()
    {
        return Vector2.Distance(currentTarget.gameObject.transform.position, gameObject.transform.position) > targetingController.radius * transform.localScale.magnitude;
    }

    //Whether or not the target is in range to attack. Might do some more complicated calculations here later?
    protected virtual bool TargetInWeaponRange()
    {
        return Vector2.Distance(currentTarget.transform.position, transform.position) <= shipWeapon.range;
    }

    protected virtual bool TargetInAttackRange()
    {
        return Vector2.Distance(currentTarget.transform.position, transform.position) <= shipWeapon.npcFollowDistance;
    }

    //Called when the npc "loses interest" in a target
    protected void Deaggro()
    {
        //going to do other stuff here later, like returning to patrol point
        currentTarget = null;
        targetRb = null;
    }
    
    protected virtual void Move()
    {
        if (currentTarget != null)
        {
            Rotate();
            float distance = Vector2.Distance(currentTarget.transform.position, transform.position);
            if (!TargetInWeaponRange()) //if the target is further away than half of the weapons range
            {
                Debug.Log("Target not in attack range");
                if (Mathf.Abs(lastDistanceToTarget - distance) < 1f && distance < 1f)
                {
                    thrusterPower -= acceleration * 2 * Time.deltaTime;
                    if (thrusterPower < 0)
                        thrusterPower = 0;
                }
                else
                {
                    thrusterPower += acceleration * Time.deltaTime;
                    if (thrusterPower > 1)
                        thrusterPower = 1;
                }
            }
            else
            {
                thrusterPower -= acceleration * 2 * Time.deltaTime;
                if (thrusterPower < 0)
                    thrusterPower = 0;
            }

            lastDistanceToTarget = distance;
            shipRb.AddForce((transform.up * speed * speedConst * shipRb.mass) * Time.deltaTime * thrusterPower);
        }
    }

    //this function handles the rotation of the enemy
    protected void Rotate()
    {
        if (currentTarget == null)
            return;
        float angle = AngleToTarget();
        Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
        //CODE FOR INSTANT ROTATE
        //transform.up = currentTarget.gameObject.transform.position - transform.position; //change rotation to face target
    }

    protected float AngleToTarget()
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return angle < 0 ? angle + 360 : angle;
    }

    protected bool TargetWithinShootAngle()
    {
        Vector2 direction = targetPosition - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= transform.rotation.eulerAngles.z;
        angle = angle < 0 ? angle + 360 : angle;
        bool isInAngle = angle > 90 - shipWeapon.degreesOfAccuracy && angle < 90 + shipWeapon.degreesOfAccuracy;
        if (!isInAngle)
            Debug.Log("Target not in shoot angle");
        return isInAngle;
    }

    protected Vector2 PredictTargetLocation()
    {
        Vector2 position = currentTarget.transform.position;

        Vector2 relativeVelocity = targetRb.velocity - shipRb.velocity;
        
        return position + FirstOrderInterceptTime(shipWeapon.projectileSpeed, position - (Vector2)transform.position, relativeVelocity)*relativeVelocity;
    }

    public static float FirstOrderInterceptTime(float shotSpeed, Vector3 targetRelativePosition, Vector3 targetRelativeVelocity)
    {
        float velocitySquared = targetRelativeVelocity.sqrMagnitude;
        if (velocitySquared < 0.001f)
            return 0f;

        float a = velocitySquared - shotSpeed * shotSpeed;

        //handle similar velocities
        if (Mathf.Abs(a) < 0.001f)
        {
            float t = -targetRelativePosition.sqrMagnitude /
            (
                2f * Vector3.Dot
                (
                    targetRelativeVelocity,
                    targetRelativePosition
                )
            );
            return Mathf.Max(t, 0f); //don't shoot back in time
        }

        float b = 2f * Vector3.Dot(targetRelativeVelocity, targetRelativePosition);
        float c = targetRelativePosition.sqrMagnitude;
        float determinant = b * b - 4f * a * c;

        if (determinant > 0f)
        { //determinant > 0; two intercept paths (most common)
            float t1 = (-b + Mathf.Sqrt(determinant)) / (2f * a),
                    t2 = (-b - Mathf.Sqrt(determinant)) / (2f * a);
            if (t1 > 0f)
            {
                if (t2 > 0f)
                    return Mathf.Min(t1, t2); //both are positive
                else
                    return t1; //only t1 is positive
            }
            else
                return Mathf.Max(t2, 0f); //don't shoot back in time
        }
        else if (determinant < 0f) //determinant < 0; no intercept path
            return 0f;
        else //determinant = 0; one intercept path, pretty much never happens
            return Mathf.Max(-b / (2f * a), 0f); //don't shoot back in time
    }

    public override void TakeDamage(float damage, ShipController damager)
    {
        if (isDead)
            return;
        lastDamaged = 0f;
        if (shield >= damage)
        {
            shield -= damage;
        }
        else
        {
            damage -= shield;
            shield = 0;
            health -= damage;
        }

        if (health <= 0)
        {
            health = 0;
            damager.GetStats().AlterReputation(stats.reputation, true);
            damager.RemoveDeadTarget(this);
            PrepareForDeath();
        }
        else
        {
            damager.GetStats().AlterReputation(stats.reputation, false);
        }

        if (!aggroTable.IsInTable(damager))
            aggroTable.AddShip(damager, damage);
        else
            aggroTable.UpdateEntry(damager, damage);

        aggroElements = aggroTable.GetElements();
        try
        {
            UpdateBars();
        }
        catch
        {
            Debug.LogWarning(gameObject.name + " does not have a health bar");
        }
    }

    public void AddTargetFromTargetingController(ShipController ship)
    {
        if(!aggroTable.IsInTable(ship))
            aggroTable.AddShip(ship, CalculateInitalAggro(stats.reputation, ship.GetReputation()));
    }

    public void AttemptToRemoveTargetFromTargetingController(ShipController ship)
    {
        if(aggroTable.IsInTable(ship))
            aggroTable.RemoveElement(ship);
        if (ship.gameObject == currentTarget)
            Deaggro();
    }

    public override void RemoveDeadTarget(ShipController deadTarget)
    {
        AttemptToRemoveTargetFromTargetingController(deadTarget);
    }

    float CalculateInitalAggro(float rep1, float rep2)
    {
        return Mathf.Abs(rep1 - rep2);
    }

    protected override void Die()
    {
        //eventually will do more stuff here
        Destroy(gameObject);
    }

}
