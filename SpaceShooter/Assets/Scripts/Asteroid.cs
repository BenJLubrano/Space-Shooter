using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] AudioSource destroySound;
    [SerializeField] Collider2D hitBox;
    [SerializeField] SpriteRenderer renderer;

    bool waitingForDestroy = false;

    // Update is called once per frame
    void Update()
    {
        if(waitingForDestroy)
        {
            if(!destroySound.isPlaying)
            {
                Destroy(gameObject);
            }
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health <= 0)
        {
            hitBox.enabled = false;
            renderer.enabled = false;
            destroySound.Play();
            waitingForDestroy = true; //make sure that the gameobject doesn't destroy itself before the sound is done playing
        }
    }
}
