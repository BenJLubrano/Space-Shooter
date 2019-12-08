using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] float health = 100f;
    [SerializeField] AudioSource destroySound;
    [SerializeField] Collider2D hitBox;
    [SerializeField] SpriteRenderer renderer;
    [SerializeField] Animator animator;
    [SerializeField] float growthSpeed = 2f;
    bool waitingForDestroy = false;

    Vector3 growth;
    private void Start()
    {
        growth = new Vector3(growthSpeed, growthSpeed, growthSpeed);
    }
    // Update is called once per frame
    void Update()
    {
        if(waitingForDestroy)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("Destroyed"))
                transform.localScale += new Vector3(growthSpeed, growthSpeed, 0) * Time.deltaTime;
            if (!destroySound.isPlaying)
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
            animator.SetTrigger("Destroyed");
            hitBox.enabled = false;
            destroySound.Play();
            waitingForDestroy = true; //make sure that the gameobject doesn't destroy itself before the sound is done playing
        }
    }
}
