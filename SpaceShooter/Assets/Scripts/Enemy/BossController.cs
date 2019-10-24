using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : NpcController
{
    [SerializeField] List<GameObject> turrets = new List<GameObject>();

    private void Awake()
    {
        UpdateBars();
        GameObject turretContainer = transform.Find("Turrets").gameObject;
        
        foreach(Transform turret in turretContainer.transform)
        {
            turrets.Add(turret.gameObject);
        }
    }
}
