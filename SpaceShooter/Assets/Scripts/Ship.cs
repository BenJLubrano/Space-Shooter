using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//base ship class that all players and enemies will inherit from
public class Ship : MonoBehaviour
{
    [SerializeField] public string faction;
    [SerializeField] public float health;
    [SerializeField] public float speed;
    [SerializeField] public float speedPenalty;
    [SerializeField] public float turnSpeed;
    [SerializeField] public float defaultDrag;
    [SerializeField] public float brakeStrength;
    [SerializeField] public Weapon shipWeapon;
    [SerializeField] public GameObject projectile;
    [SerializeField] public AudioSource audioSource;

    bool waitingForDestroy = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
