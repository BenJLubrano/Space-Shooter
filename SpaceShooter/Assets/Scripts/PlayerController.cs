using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls the player, both movement and ingame functions
public class PlayerController : Ship
{
    public Animator animator;
    [SerializeField] Animator shieldAnim;
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
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        //player movement stuff
        float verticalMove = (Input.GetAxis("Vertical") * speed * (speedConst * shipRb.mass) * Time.deltaTime);
        verticalMove = (verticalMove < 0) ? verticalMove * speedPenalty : verticalMove;
        float horizontalMove = Input.GetAxis("Horizontal") * speed * (speedConst * shipRb.mass) * speedPenalty * Time.deltaTime;
        float rotationMove = Input.GetAxis("Rotation") * turnSpeed;

        if (rotationMove + verticalMove + horizontalMove != 0f) //if the player is moving in a certain direction or rotating
        {
            shipRb.angularVelocity = 0f; //set angular velocity to zero, which stops the player from rotating due to outside forces
        }

        HandleAnimation(Mathf.Abs(verticalMove));

        Vector2 force = new Vector2(horizontalMove, verticalMove);
        shipRb.MoveRotation(shipRb.rotation + rotationMove);
        shipRb.AddForce(transform.TransformDirection(force));
        Debug.Log("player: " + shipRb.velocity.magnitude);
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
    }

    void HandleAnimation(float currentSpeed)
    {
        if (Input.GetButton("Vertical"))
        {
            animator.SetBool("IsAccelerating", true);
        }
        else
        {
            animator.SetBool("IsAccelerating", false);
        }

        
        animator.SetFloat("AnimatorSpeed", currentSpeed*50 / (this.speed*shipRb.mass));

        if (shield >= 1)
        {
            shieldAnim.SetBool("hasShield", true);
        }
        else if (shield < 1)
        {
            shieldAnim.SetBool("hasShield", false);
        }
    }

    public void SetSpawn(Transform point)
    {
        spawnPoint = point;
    }

    //since we don't want the player to be destroyed on death, we should override the default Die()
    protected override void Die()
    {
        //eventually will do more stuff here
        //StartCoroutine("Respawn") For some reason this causes the player to get stuck.
        RespawnFunc();
    }

    protected override void PrepareForDeath()
    {
        base.PrepareForDeath();
        shieldAnim.gameObject.GetComponent<SpriteRenderer>().enabled = false;
    }

    //Simple method to respawn the player
    IEnumerator Respawn()
    {
        Debug.Log("Waiting for respawn");
        yield return new WaitForSeconds(5);
        RespawnFunc();
        
    }

    void RespawnFunc()
    {
        transform.position = spawnPoint.position;
        shipCollider.enabled = true;
        spriteRenderer.enabled = true;
        shipRb.velocity = Vector2.zero;
        shipRb.angularVelocity = 0;
        shield = maxShield;
        health = maxHealth;
        UpdateBars();
        shieldAnim.gameObject.GetComponent<SpriteRenderer>().enabled = true;
        shieldAnim.SetBool("hasShield", true);
        isDead = false;
    }
}
