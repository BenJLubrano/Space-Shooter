using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [Header("Targeting")]
    [SerializeField] ShipController shooter;
    [SerializeField] List<string> enemyFactions;
    [SerializeField] GameObject target;

    [Header("References")]
    [SerializeField] Weapon weapon;
    [SerializeField] protected BoxCollider2D hitBox;
    [SerializeField] protected SpriteRenderer renderer;
    [SerializeField] AudioSource audioSource;
    Vector3 speedMod = Vector2.zero;

    protected float damage;
    bool waitingForDestroy = false;
    protected Vector2 startPos;

    [SerializeField] GameObject onHitExplosion;

    // Start is called before the first frame update
    void Awake()
    {
        startPos = transform.position;
    }

    //Used to set up some information that the projectile needs
    public virtual void Initialize(Weapon weapon, ShipController shooter, GameObject target = null, List<string> factions = null)
    {
        this.weapon = weapon;
        damage = weapon.damage;
        renderer.sprite = weapon.projectileImage;
        
        this.shooter = shooter;
        enemyFactions = factions;
        this.target = target;

        if (weapon.sound != null)
        {
            audioSource.clip = weapon.sound;
            audioSource.volume = weapon.volume;
            audioSource.Play();
        }
        hitBox.size *= weapon.scale;
        Vector2 shooterSpeed = shooter.GetSpeed();
        speedMod = new Vector3(shooterSpeed.x, shooterSpeed.y, 0);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(waitingForDestroy)
        {
            if(!audioSource.isPlaying)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            transform.position += (transform.up * weapon.projectileSpeed * Time.deltaTime) + speedMod * Time.deltaTime;
        }
        
    }

    //called after physics updates
    private void LateUpdate()
    {
        if (Vector2.Distance(transform.position, startPos) >= weapon.range)
            Deactivate();
    }

    //called when the projectile collides with something
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Asteroid") //if it's an asteroid, we know what to do
        {
            collision.gameObject.GetComponent<Asteroid>().TakeDamage(damage);
            Deactivate();
        }
        else
        {
           ShipController colliderShip = collision.gameObject.GetComponent<ShipController>(); //first make sure that what we're colliding with is indeed a ship
            if (colliderShip != null) //if it got aShipControllerscript, it must be a ship
            {
                string colliderFaction = colliderShip.GetFaction(); //get the faction of the colliding ship
                if (enemyFactions.Contains(colliderFaction)) //if it's in the list of enemy factions (factions that the projectile can hit)
                {
                    if (colliderShip.CanBeHitBy(shooter.GetId()))
                    {
                        colliderShip.TakeDamage(damage, shooter);
                        Deactivate();
                        if (collision.gameObject.GetComponent<ShipController>().getShield() < 1)
                        {
                            OnHit();
                        }
                    }
                }
            }
        }
    }

    //Called when it is time for the projectile to die. Makes it so nothing can interact with it, but sets waitingForDestroy to true so we can finish hearing the sound
    void Deactivate()
    {
        hitBox.enabled = false;
        renderer.enabled = false;
        waitingForDestroy = true;
    }

    void OnHit()
    {
        if(onHitExplosion != null)
            Instantiate(onHitExplosion, transform.position, transform.rotation);
    }
}
