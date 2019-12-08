using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : ShipStats
{ 
    [Header("Player Specific", order = 1)]
    [SerializeField] int experience;
    [SerializeField] int units;
    [SerializeField] List<WeaponUI> weaponHolders = new List<WeaponUI>();

    private void Awake()
    {
        units = 1000;
        base.Awake();
        staticReputation = false;
        PlayerController pc = (PlayerController)shipController;
        pc.UpdateReputationDisplay();
        shipId = 0;

        for(int i = 0; i < weapons.Count; i++)
        {
            weaponHolders[i].SlotImage(weapons[i].projectileImage);
        }
    }

    public void AddExperience(int exp)
    {
        experience += exp;
        if (experience >= level * 50) //player levels up
        {
            experience -= level * 50;
            level += 1;
        }
    }

    public int GetUnits()
    {
        return units;
    }

    public void ModifyUnits(int amount)
    {
        units += amount;
    }

    public void ResetUnits(int amount)
    {
        units = amount;
    }

    public override void AlterReputation(float targetRep, bool killed)
    {
        base.AlterReputation(targetRep, killed);
        PlayerController pc = (PlayerController)shipController;
        pc.UpdateReputationDisplay();

    }

    public override void SetCurrentWeapon(int value)
    {
        base.SetCurrentWeapon(value);
        weaponHolders[value].GetSelected();
        for(int i = 0; i < weaponHolders.Count; i++)
        {
            if(i != value)
            {
                weaponHolders[i].AccelerateFade();
            }
        }
    }
}
