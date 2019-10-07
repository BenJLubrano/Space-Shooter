﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] Weapon weapon;

    [SerializeField] BoxCollider2D hitBox;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] AudioSource audioSource;

    Vector3 speedMod = Vector2.zero;
    bool waitingForDestroy = false;
    Vector2 startPos;
    // Start is called before the first frame update
    void Awake()
    {
        startPos = transform.position;
        renderer.sprite = weapon.projectileImage;
        if(weapon.shootSound != null)
        {
            audioSource.clip = weapon.shootSound;
            audioSource.Play();
        }
    }

    // Update is called once per frame
    void Update()
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

    private void LateUpdate()
    {
        if (Vector2.Distance(transform.position, startPos) >= weapon.range)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //deal damage and make a sound, then deactivate
        if(collision.gameObject.tag == "Asteroid")
        {
            collision.gameObject.GetComponent<Asteroid>().TakeDamage(weapon.damage);
            Deactivate();
        }

        if(collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Ship>().TakeDamage(weapon.damage);
            Deactivate();
        }
        
    }

    //now nothing can interact with it, but we need to wait for the sound to stop playing
    void Deactivate()
    {
        hitBox.enabled = false;
        renderer.enabled = false;
        waitingForDestroy = true;
    }

    public void AddVelocity(Vector2 shooterVelocity)
    {
        speedMod = shooterVelocity;
    }
}
