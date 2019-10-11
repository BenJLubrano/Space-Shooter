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

    //These variables are used for the player controller, but they exist here in case we want to have the enemy ships move like the player
    public float speedPenalty;
    public float turnSpeed;
    public float defaultDrag;
    public float brakeStrength;


    bool waitingForDestroy = false;
    // Update is called once per frame
    protected void Update()
    {
        if(waitingForDestroy && !audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        audioSource.Play();
        waitingForDestroy = true;
    }
}
