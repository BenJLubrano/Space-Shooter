using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : Projectile
{
    float startLength;
    float maxLength;
    float growSpeed;

    public override void Initialize(Weapon weapon, Ship shooter, GameObject target = null, List<string> factions = null)
    {
        base.Initialize(weapon, shooter, target, factions);
        LaserWeapon laserWeapon = (LaserWeapon)weapon;
        startLength = laserWeapon.startLength;
        maxLength = laserWeapon.maxLength;
        growSpeed = laserWeapon.growSpeed;
        transform.parent = shooter.gameObject.transform;
    }

    protected override void Update()
    {
        base.Update();
        float size = growSpeed * Time.deltaTime;
        transform.localScale += new Vector3(size, size, 0f);
    }
}
