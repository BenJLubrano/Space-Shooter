using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls the player, both movement and ingame functions
public class PlayerController : Ship
{
    public Animator animator;
    public GameObject explosion;
    public GameObject shieldActive, shieldBreak;
    [SerializeField] Transform spawnPoint;
    private void Awake()
    {
        shipId = 0; //make sure the player is always ID 0
    }

    void Update()
    {
        base.Update(); //call the base ship update to perform generic functions

        //do nothing if the player is dead
        if (isDead)
            return;

        //player movement stuff
        float verticalMove = Input.GetAxis("Vertical") * speed;
        verticalMove = (verticalMove < 0) ? verticalMove * speedPenalty : verticalMove;
        float horizontalMove = Input.GetAxis("Horizontal") * speed * speedPenalty;
        float rotationMove = Input.GetAxis("Rotation") * turnSpeed;

        if(rotationMove + verticalMove + horizontalMove != 0f) //if the player is moving in a certain direction or rotating
        {
            shipRb.angularVelocity = 0f; //set angular velocity to zero, which stops the player from rotating due to outside forces
        }

        HandleAnimation(Mathf.Abs(verticalMove));

        Vector2 force = new Vector2(horizontalMove, verticalMove);
        shipRb.MoveRotation(shipRb.rotation + rotationMove);
        shipRb.AddForce(transform.TransformDirection(force));

        if (Input.GetButton("Brake")) //if the player is braking
        {
            turnSpeed = defaultTurnSpeed / 2f;
            shipRb.drag = brakeStrength; //up the drag, which causes the player to stop faster
            shipRb.angularVelocity = 0f;
        }
        else
        {
            turnSpeed = defaultTurnSpeed;
            shipRb.drag = defaultDrag;
        }

        if (Input.GetButton("Shoot"))
        {
            Shoot();
        }

        if(shield >= 1)
        {
            animator.SetBool("hasShield", true);
            //Instantiate(shieldActive, transform.position, transform.rotation);
        }
        else if(shield < 1)
        {
            animator.SetBool("hasShield", false);
        }
    }

    void HandleAnimation(float speed)
    {
        if (Input.GetButton("Vertical"))
        {
            animator.SetBool("IsAccelerating", true);
        }
        else
        {
            animator.SetBool("IsAccelerating", false);
        }
        animator.SetFloat("AnimatorSpeed", speed);
    }

    public void SetSpawn(Transform point)
    {
        spawnPoint = point;
    }

    //since we don't want the player to be destroyed on death, we should override the default Die()
    protected override void Die()
    {
        //eventually will do more stuff here
        StartCoroutine("Explosion");
        Respawn();
    }

    //Simple method to respawn the player
    void Respawn()
    {
        transform.position = spawnPoint.position;
        shipCollider.enabled = true;
        spriteRenderer.enabled = true;

        shield = maxShield;
        health = maxHealth;
        UpdateBars();
        isDead = false;
    }

    IEnumerator Explosion()
    {
        Instantiate(explosion, transform.position, transform.rotation);
        yield return null;
    }
}
