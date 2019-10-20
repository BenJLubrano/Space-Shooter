using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Base ship class that all players and enemies will inherit from
//It will control the functions that all ships share
public class Ship : MonoBehaviour
{
    [Header("Ship Stats")]
    [SerializeField] protected int shipId;
    [SerializeField] protected string faction;
    [SerializeField] protected float maxHealth;
    [SerializeField] protected float health;
    [SerializeField] protected float maxShield;
    [SerializeField] protected float shield;
    [SerializeField] protected float speed;
    [SerializeField] protected float weaponCooldown = 0f;

    [Header("Player Movement Variables")]
    //These variables are used for the player controller, but they exist here in case we want to have the enemy ships move like the player
    [SerializeField] protected float speedPenalty;
    [SerializeField] protected float defaultTurnSpeed = 5;
    [SerializeField] protected float turnSpeed = 5;
    [SerializeField] protected float defaultDrag = 5;
    [SerializeField] protected float brakeStrength = 1;

    [Header("References")]
    [SerializeField] protected Weapon shipWeapon;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected Rigidbody2D shipRb;
    [SerializeField] protected AudioClip deathSound;
    [SerializeField] protected AudioClip moveSound;
    [SerializeField] protected Image healthBar;
    [SerializeField] protected Image shieldBar;

    bool isDead = false;

    protected void Update()
    {
        weaponCooldown -= Time.deltaTime;
        if (isDead && !audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    //basic shoot function used by all ships
    protected virtual void Shoot(GameObject target = null, List<string> factions = null)
    {
        if (weaponCooldown <= 0f)
        {
            weaponCooldown = 1 / shipWeapon.fireRate;
            GameObject shot = Instantiate(shipWeapon.projectile, transform.up / 1.5f + transform.position, transform.rotation); //create the projectile

            if(factions == null) //if the faction is null, assume that the projectile can hit all ships except the ones from the shooter's faction
            {
                factions = new List<string>();
                factions.Add("Federation");
                factions.Add("Pirate");
                factions.Add("Neutral");
                factions.Remove(this.faction);
            }

            shot.GetComponent<Projectile>().Initialize(shipWeapon, shipId, target, factions);
        }
    }

    //called by a projectile to inflict damage onto a ship
    public virtual void TakeDamage(float damage)
    {
        if (isDead)
            return;

        if (shield >= damage)
        {
            shield -= damage;
        }
        else
        {
            damage -= shield;
            shield = 0;
            health -= damage;
        }
        if(health <= 0)
        {
            health = 0;
            Die();
        }
        UpdateBars();
    }

    void UpdateBars()
    {
        if(shieldBar != null) //has a shield
        {
            shieldBar.fillAmount = shield / maxShield;
        }
        healthBar.fillAmount = health / maxHealth;
    }
    //simply returns the faction of the ship
    public string GetFaction()
    {
        return faction;
    }

    //called when the ship dies
    protected virtual void Die()
    {
        audioSource.clip = deathSound;
        audioSource.Play();
        isDead = true;
    }

}
