using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : ShipStats
{ 
    [Header("Player Specific", order = 1)]
    [SerializeField] int experience;
    [SerializeField] int units;
    [SerializeField] PlayerStatsUI statsUI;
    [SerializeField] List<Sprite> factionSprites = new List<Sprite>();
    [SerializeField] List<WeaponUI> weaponHolders = new List<WeaponUI>();

    bool uiIsActive = false;
    PlayerController pc;
    private void Awake()
    {
        units = 1000;
        base.Awake();
        staticReputation = false;
        pc = (PlayerController)shipController;
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

    protected override void AddWeapon(Weapon newWeapon)
    {
        weaponHolders[weapons.Count].SlotImage(newWeapon.projectileImage);
        base.AddWeapon(newWeapon);
    }
    public override void SetCurrentWeapon(int value)
    {
        base.SetCurrentWeapon(value);
        if(value < weapons.Count)
        {
            weaponHolders[value].GetSelected();
            for (int i = 0; i < weaponHolders.Count; i++)
            {
                if (i != value)
                {
                    weaponHolders[i].AccelerateFade();
                }
            }
            shipController.SetCooldown(value, .5f);
        }
    }

    private void Update()
    {
        if (uiIsActive)
            DisplayPlayerStatsUI();
    }

    public void ToggleUI()
    {
        if (uiIsActive)
            statsUI.Hide();
        else
            DisplayPlayerStatsUI();
        uiIsActive = !uiIsActive;
    }

    void DisplayPlayerStatsUI()
    {
        int currentHealth = (int)pc.GetCurrentHealth();
        int currentShield = (int)pc.GetCurrentShield();

        string healthTxt = currentHealth.ToString() + " / " + maxHealth.ToString();
        string shieldTxt = currentShield.ToString() + " / " + maxShield.ToString();
        string speedTxt = speed.ToString();
        Sprite factionSprite;
        if (reputation < 0)
            factionSprite = factionSprites[2];
        else if (reputation > 10)
            factionSprite = factionSprites[1];
        else
            factionSprite = factionSprites[0];
        statsUI.UpdateDisplay(ship.baseSprite, factionSprite, healthTxt, shieldTxt, speedTxt, faction, reputation.ToString(), units.ToString(), false, false, false);
    }
}
