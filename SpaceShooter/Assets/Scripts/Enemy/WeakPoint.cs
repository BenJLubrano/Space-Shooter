using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : Ship
{
    [Header("Weak Point Effects")]
    [SerializeField] BossController owner;
    [SerializeField] WeakPointZone zone;
    [SerializeField] float damageToOwner;
    [SerializeField] bool reducesSpeed = false;
    [SerializeField] float speedReduction;
    [SerializeField] float turnSpeedReduction;
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DoUpdateChecks();
    }

    public override bool CanBeHitBy(int id)
    {
        return zone.IsInZone(id);
    }

    protected override void PrepareForDeath()
    {
        base.PrepareForDeath();
        owner.TakeDamage(damageToOwner);
        if(reducesSpeed)
        {
            owner.ReduceSpeed(speedReduction);
            owner.ReduceTurnSpeed(turnSpeedReduction);
        }
    }
}
