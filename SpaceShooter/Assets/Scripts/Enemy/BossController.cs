using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : NpcController
{
    [SerializeField] List<TurretController> turrets = new List<TurretController>();
    [SerializeField] BossHitZone hitZone;

    public bool canShoot = false;
    [SerializeField] float internalClock = 0f;
    [SerializeField] float nextActionTime;

    [SerializeField] Vector2 moveTime = new Vector2(2,5);
    [SerializeField] Vector2 rotateTime = new Vector2(3,10);

    [SerializeField] float actionDuration;
    [SerializeField] float lastActionTime = 0f;
    [SerializeField] float waitTime = 5f;
    [SerializeField] bool canPerformAction = true;
    [SerializeField] bool isWaiting = false;

    //intervalClock keeps track of the time of the boss.
    //moveTime is how long the ship gets to move for
    //rotateTime is how long the ship gets to rotate for
    //actionDuration is the duration of whatever action was done (move or rotate)
    //lastActionTime is the time that the last action was completed. This + actionDuration is the time when the ship has to "cooldown"
    //nextActionTime is the time when the next action can be started. This has to be set
    bool canMove, canRotate;
    private void Awake()
    {
        UpdateBars();
        GameObject turretContainer = transform.Find("Turrets").gameObject;
        
        foreach(Transform turret in turretContainer.transform)
        {
            TurretController turretController = turret.GetComponentInChildren<TurretController>();
            turretController.SetBoss(this);
            turrets.Add(turretController);
        }
    }

    protected override void Update()
    {
        DoUpdateChecks();
        if(shield <= 0)
        {
            gameObject.layer = 0;
        }
        if(currentTarget == null)
            LookForTargets();

        internalClock += Time.deltaTime;
        if (hitZone.ShipsInZone().Count > 0 && weaponCooldown <= 0)
        {
            canRotate = false;
            canMove = false;
            Shoot(null, enemyFactions);
            shipRb.velocity = Vector2.zero;
            isWaiting = true;
            nextActionTime += 10f;
            return; //Don't do anything else if the boss is shooting
        }

        
        if(canPerformAction) //if it is time to perform an action
        {
            lastActionTime = internalClock;
            PerformAction();
        }
        if(internalClock >= lastActionTime + actionDuration) //if it is time to wait after performing an action
        {
            canMove = false;
            canRotate = false;
            isWaiting = true;
        }
        if(isWaiting)
        {
            if(internalClock >= lastActionTime + actionDuration + waitTime)
            {
                isWaiting = false;
                canPerformAction = true;
            }
        }
        else
        {
            if (canMove)
            {
                turnSpeed = defaultTurnSpeed * .1f;
                Move();
            }
            else if (canRotate)
            {
                turnSpeed = defaultTurnSpeed;
                Rotate();
            }
        }
        nextActionTime = lastActionTime + actionDuration + waitTime;
    }


    protected override bool TargetInAttackRange()
    {
        return TurretsInRange();
    }

    void PerformAction()
    {
        bool turretInRange = TurretsInRange();

        int choice = turretInRange ? Random.Range(0, 2) : 1;
        if(choice == 0)
        {
            canRotate = true;
            canMove = false;
            actionDuration = Random.Range(rotateTime.x, rotateTime.y);
            waitTime = 2;
        }
        else if (choice == 1)
        {
            canMove = true;
            canRotate = true;
            actionDuration = Random.Range(moveTime.x, moveTime.y);
            waitTime = 10;
        }

        if(!turretInRange)
        {
            Debug.Log("No turret was in range!");
            waitTime = 0;
        }

        canPerformAction = false;
    }

    bool TurretsInRange()
    {
        foreach (TurretController turret in turrets)
        {
            if (turret.CanHitTarget())
            {
                return true;
            }
        }
        return false;
    }
    protected override void PrepareForDeath()
    {
        base.PrepareForDeath();
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public bool IsMoving()
    {
        return canMove || canRotate;
    }
}
