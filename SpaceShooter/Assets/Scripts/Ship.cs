using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base ship class that all players and enemies will inherit from
//It will control the functions that all ships share
public class Ship : MonoBehaviour
{
    public int shipID;
    public string faction;
    public float health;
    public float speed;
    public Weapon shipWeapon;
    public GameObject projectile;
    public AudioSource audioSource;

    //audio clips
    public AudioClip deathSound;
    public AudioClip moveSound;

    //These variables are used for the player controller, but they exist here in case we want to have the enemy ships move like the player
    public float speedPenalty;
    public float turnSpeed;
    public float defaultDrag;
    public float brakeStrength;


    bool isDead = false;
    // Update is called once per frame
    protected void Update()
    {
        if(isDead && !audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead)
            return;
        
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        audioSource.clip = deathSound;
        audioSource.Play();
        isDead = true;
    }
}
