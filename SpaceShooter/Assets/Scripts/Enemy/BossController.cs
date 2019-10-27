using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : NpcController
{
    [SerializeField] List<GameObject> turrets = new List<GameObject>();
    [SerializeField] BossHitZone hitZone;

    public bool canShoot = false;
    private void Awake()
    {
        UpdateBars();
        GameObject turretContainer = transform.Find("Turrets").gameObject;
        
        foreach(Transform turret in turretContainer.transform)
        {
            turrets.Add(turret.gameObject);
        }
    }

    private void Update()
    {
        DoUpdateChecks();

        if(hitZone.shipsInHitZone.Count > 0 && weaponCooldown <= 0)
        {
            Shoot(null, enemyFactions);
        }
    }
    protected override void PrepareForDeath()
    {
        base.PrepareForDeath();
        foreach(GameObject turret in turrets)
        {
            turret.SetActive(false);
        }
    }
}
