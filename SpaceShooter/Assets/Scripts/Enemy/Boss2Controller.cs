using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Controller : BossController
{
    [SerializeField] WeaponHolder forwardMissileL;
    [SerializeField] WeaponHolder forwardMissileR;
    [SerializeField] BossHitZone hitZone2;

    float leftCD, rightCD = 0f;
    private void Awake()
    {
        aggroTable.Initialize(this);
        UpdateBars();
        GameObject turretContainer = transform.Find("Turrets").gameObject;
        foreach (Transform turret in turretContainer.transform)
        {
            TurretController turretController = turret.GetComponentInChildren<TurretController>();
            turretController.SetBoss(this);
            turrets.Add(turretController);
        }
        aggroTable.AddShip(GameObject.FindGameObjectWithTag("Player").GetComponent<ShipController>(), 100);
        aggroElements = aggroTable.GetElements();
        UpdateFactions();
    }

    protected override void DoUpdateChecks()
    {
        base.DoUpdateChecks();
        leftCD -= Time.deltaTime;
        rightCD -= Time.deltaTime;
    }

    protected override void Shoot(GameObject target = null, List<string> factions = null)
    {
        List<ShipController> shipsinLZone = hitZone.ShipsInZone();
        List<ShipController> shipsInRZone= hitZone2.ShipsInZone();

        //The target is in one of the zones
        if (shipsInRZone.Contains(currentTarget))
        {
            if (rightCD <= 0)
            {
                StartCoroutine("DoAnim", 1);
            }
        }
        else if (shipsinLZone.Contains(currentTarget))
        {
            if(leftCD <= 0)
                StartCoroutine("DoAnim", 0);
            /*shipAnimator.SetBool("IsOpen", true);
            if (leftCD <= 0)
            {
                forwardMissileL.ForceShoot(currentTarget.gameObject, factions);
                leftCD = 15f;
            }*/
        }

        /*if(shipsinLZone.Count > 0 && leftCD <= 0)
        {
            shipAnimator.SetBool("IsOpen", true);
            forwardMissileL.ForceShoot(shipsinLZone[0].gameObject, factions);
            leftCD = 15f;
        }
        if (shipsInRZone.Count > 0 && rightCD <= 0)
        {
            shipAnimator.SetBool("IsOpen", true);
            forwardMissileR.ForceShoot(shipsInRZone[0].gameObject, factions);
            rightCD = 15f;
        }*/
    }

    IEnumerator DoAnim(int side)
    {
        if (side == 0)
            leftCD = 15f;
        else
            rightCD = 15f;
        StallTime(5);
        shipAnimator.SetBool("IsOpen", true);
        yield return new WaitForSeconds(2);
        if(side == 0)
            forwardMissileL.ForceShoot(currentTarget.gameObject, enemyFactions);
        else
            forwardMissileR.ForceShoot(currentTarget.gameObject, enemyFactions);
        shipAnimator.SetBool("IsOpen", false);
        shipAnimator.SetTrigger("Close");
    }

    public void MissileShot()
    {
        StallTime(15f);
    }
}
