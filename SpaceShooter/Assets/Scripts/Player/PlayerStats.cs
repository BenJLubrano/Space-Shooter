﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : ShipStats
{ 
    [Header("Player Specific", order = 1)]
    [SerializeField] int experience;
    [SerializeField] int units;

    private void Awake()
    {
        base.Awake();
        staticReputation = false;
        PlayerController pc = (PlayerController)shipController;
        pc.UpdateReputationDisplay();
        shipId = 0;
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
    public override void AlterReputation(float targetRep, bool killed)
    {
        base.AlterReputation(targetRep, killed);
        PlayerController pc = (PlayerController)shipController;
        pc.UpdateReputationDisplay();

    }
}
