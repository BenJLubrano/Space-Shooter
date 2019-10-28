using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [Header("Targeting")]
    [SerializeField] int shooterId;
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
    public virtual void Initialize(Weapon weapon, Ship shooter, GameObject target = null, List<string> factions = null)
    {
        this.weapon = weapon;
        damage = weapon.damage;
        renderer.sprite = weapon.projectileImage;
        
        shooterId = shooter.GetId();
        enemyFactions = factions;
        this.target = target;

        if (weapon.sound != null)
        {
            audioSource.clip = weapon.sound;
            audioSource.volume = weapon.volume;
            audioSource.Play();
        }
        hitBox.size *= weapon.scale;
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
            Ship colliderShip = collision.gameObject.GetComponent<Ship>(); //first make sure that what we're colliding with is indeed a ship
            if (colliderShip != null) //if it got a ship script, it must be a ship
            {
                string colliderFaction = colliderShip.GetFaction(); //get the faction of the colliding ship
                if (enemyFactions.Contains(colliderFaction)) //if it's in the list of enemy factions (factions that the projectile can hit)
                {
                    if (colliderShip.CanBeHitBy(shooterId))
                    {
                        colliderShip.TakeDamage(damage);
                        Deactivate();
                        if (collision.gameObject.GetComponent<Ship>().getShield() < 1)
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
        Instantiate(onHitExplosion, transform.position, transform.rotation);
    }
}
