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
    [SerializeField] protected float shieldRegenRate = 0f;
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
    [SerializeField] protected Rigidbody2D shipRb;
    [SerializeField] protected Collider2D shipCollider;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected AudioClip deathSound;
    [SerializeField] protected AudioClip moveSound;
    [SerializeField] protected Image healthBar;
    [SerializeField] protected Image shieldBar;
    [SerializeField] GameObject explosion;


    protected bool isDead = false;
    protected float lastDamaged = 0f;

    protected void Awake()
    {
        if (maxShield <= 0 && shieldBar != null)
        {
            shieldBar.fillAmount = 0;
        }
    }

    protected void Update()
    {
        DoUpdateChecks();
    }

    protected void DoUpdateChecks()
    {
        if (isDead && !audioSource.isPlaying)
        {
            Die();
        }
        else if (!isDead)
        {
            weaponCooldown -= Time.deltaTime;
            lastDamaged += Time.deltaTime;
            if (lastDamaged >= 15) //regenerate shields if damage has not been taken in the last 5 seconds
            {
                shield += shieldRegenRate * Time.deltaTime;
                if (shield > maxShield)
                    shield = maxShield;
                UpdateBars();
            }
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
        lastDamaged = 0f;
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
            PrepareForDeath();
        }
        try
        {
            UpdateBars();
        }
        catch
        {
            Debug.LogWarning(gameObject.name + " does not have a health bar");
        }
    }

    protected void UpdateBars()
    {
        if(maxShield > 0 && shieldBar != null) //has a shield
        {
            shieldBar.fillAmount = shield / maxShield;
        }
        if (healthBar != null)
        {
            healthBar.fillAmount = health / maxHealth;
        }
    }

    //simply returns the faction of the ship
    public string GetFaction()
    {
        return faction;
    }

    //returns the ID of the ship
    public int GetId()
    {
        return shipId;
    }

    //called when the ship dies
    protected virtual void PrepareForDeath()
    {
        StartCoroutine("Explosion");
        audioSource.clip = deathSound;
        audioSource.Play();
        shipCollider.enabled = false;
        spriteRenderer.enabled = false;
        isDead = true;
    }

    IEnumerator Explosion()
    {
        if (explosion != null)
        {
            GameObject expl = Instantiate(explosion, transform.position, transform.rotation);
            expl.transform.localScale = transform.localScale;
        }
        
        yield return null;
    }

    //Not used for most ships, but is used for WeakPoints
    public virtual bool CanBeHitBy(int id)
    {
        return true;
    }

    //Base Die() simply destroys the ship
    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
