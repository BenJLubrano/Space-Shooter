using System.Collections;
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

    float lastFrameVelocity;
    protected GameObject currentTarget;

    protected override void Update()
    {
        base.Update(); //call base ShipController update
        if(currentTarget == null)
        {
            LookForTargets();
        }
        else
        {
            if(OutOfRange() || currentTarget.GetComponent<ShipController>().IsDead())
            {
                Deaggro();
            }
        }

        //if the ShipController has a target
        if (currentTarget != null && TargetInAttackRange() && weaponCooldown <= 0 && !isDead)
        {
            //Turn this into a method in Ship.cs called "Shoot()" that will handle the cooldown setting etc, since it's universal for all ships
            Shoot(currentTarget, enemyFactions);
        }
    }

    protected virtual void FixedUpdate()
    {
        Move();
    }

    public override void Initialize(ShipStats newStats)
    {
        base.Initialize(newStats);
        InitializeFactions();

        targetingController.Initialize(enemyFactions);
        speed = (speed * shipRb.drag);
    }

    public override void SetStats(ShipStats shipStats)
    {
        base.SetStats(shipStats);
        InitializeFactions();
    }

    public void InitializeFactions()
    {
        if (stats.reputation > 10)
            enemyFactions.Add("Pirate");
        else if (stats.reputation < 0)
        {
            enemyFactions.Add("Federation");
            enemyFactions.Add("Neutral");
        }
        enemyFactions.Remove(stats.faction);

    }

    void HandleAnimation()
    {
        //animations for npcs go here
    }

    //Logic related to targeting
    protected virtual void LookForTargets()
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
    public virtual void UpdateTargets(List<GameObject> newTargets)
    {
        targets = newTargets;
        if(!targets.Contains(currentTarget))
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
    protected virtual bool TargetInAttackRange()
    {
        return Vector2.Distance(currentTarget.transform.position, transform.position) <= shipWeapon.range;
    }

    //Called when the npc "loses interest" in a target
    protected void Deaggro()
    {
        //going to do other stuff here later, like returning to patrol point
        currentTarget = null;
    }
    
    protected virtual void Move()
    {
        if (currentTarget != null)
        {
            Rotate();

            if (!TargetInAttackRange()) //if the target is further away than half of the weapons range
            {
                thrusterPower = thrusterPower > 1 ? 1 : thrusterPower + acceleration * Time.deltaTime;
                shipRb.AddForce((transform.up * speed * speedConst * shipRb.mass) * Time.deltaTime * thrusterPower);
            }
            else
                thrusterPower = thrusterPower < 0 ? 0 : thrusterPower - acceleration * Time.deltaTime;
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
        Vector2 direction = currentTarget.transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return angle < 0 ? angle + 360 : angle;
    }

    protected override void Die()
    {
        //eventually will do more stuff here
        Destroy(gameObject);
    }

}
