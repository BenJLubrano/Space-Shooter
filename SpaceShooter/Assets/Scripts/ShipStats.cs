﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class acts as a container for all of the player-related variables, such as name, reputation, and currency.
public class ShipStats : MonoBehaviour
{
    [Header("Basic Stats", order = 0)]
    [SerializeField] public int shipId;
    [SerializeField] public string shipName = "Unnamed Ship";
    [SerializeField] public int level;
    [SerializeField] public string faction;
    [SerializeField] public float reputation;
    [SerializeField] public float speed;
    [SerializeField] public float maxHealth;
    [SerializeField] public float maxShield;
    [SerializeField] public float shieldRegenRate;
    [SerializeField] public float shieldRegenTime;

    [Header("Weapons", order = 2)]
    [SerializeField] public List<Weapon> weapons;
    [SerializeField] public int currentWeapon;

    [Header("References", order = 3)]
    [SerializeField] public Ship ship;
    [SerializeField] public ShipController shipController;

    protected  void Awake()
    {
        Initialize();
    }

    //Set up using the variables from Ship
    void Initialize()
    {
        if (reputation > 10)
        {
            faction = "Federation";
        }
        else if (reputation < 10 && reputation >= 0)
        {
            faction = "Neutral";
        }
        else
        {
            faction = "Pirate";
        }

        try
        {
            shipId = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().RegisterShip();
        }
        catch
        {
            Debug.LogError("GameManager was not found in the scene!");
        }

        maxHealth = ship.health;
        maxShield = ship.shield;
        speed = ship.speed;
        shieldRegenRate = ship.shieldRegenRate;
        shieldRegenRate = ship.shieldRegenTime;

        //Create a function like this when it is time to start changing stats based on level (player will need one to decide based on upgrades)
        //ModifyStats();

        shipController.Initialize(this);
    }

    public void SetCurrentWeapon(int value)
    {
        if (value >= weapons.Count)
            currentWeapon = weapons.Count - 1;
        else if (value < 0)
            currentWeapon = 0;
        else
            currentWeapon = value;
    }

    public Weapon GetWeapon(int value)
    {
        if (value >= weapons.Count)
            return weapons[weapons.Count - 1];
        else if (value < 0)
            return weapons[0];
        else
            return weapons[value];
    }

    public float GetReputation()
    {
        return reputation;
    }

    public void ModifyReputation(float change)
    {
        reputation += change;
    }

}
