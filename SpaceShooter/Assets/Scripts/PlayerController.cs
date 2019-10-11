using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls the player, both movement and ingame functions
public class PlayerController : Ship
{
    GameObject ship;
    Rigidbody2D rb;

    float weaponCooldown = 0f;

    void Start()
    {
        ship = this.gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        base.Update(); //call the base ship update to perform generic functions
        weaponCooldown -= Time.deltaTime;

        //player movement stuff

        float verticalMove = Input.GetAxis("Vertical") * speed;
        verticalMove = (verticalMove < 0) ? verticalMove * speedPenalty : verticalMove;
        float horizontalMove = Input.GetAxis("Horizontal") * speed * speedPenalty;
        float rotationMove = Input.GetAxis("Rotation") * turnSpeed;

        if(rotationMove + verticalMove + horizontalMove != 0f) //if the player is moving in a certain direction or rotating
        {
            rb.angularVelocity = 0f; //set angular velocity to zero, which stops the player from rotating due to outside forces
        }

        Vector2 force = new Vector2(horizontalMove, verticalMove);
        rb.MoveRotation(rb.rotation + rotationMove);

        rb.AddForce(transform.TransformDirection(force));

        if (Input.GetButton("Brake")) //if the player is braking
        {
            rb.drag = brakeStrength; //up the drag, which causes the player to stop faster
            rb.angularVelocity = 0f;
        }
        else
        {
            rb.drag = defaultDrag;
        }

        if (Input.GetButton("Shoot"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        if(weaponCooldown <= 0f)
        {
            weaponCooldown = 1 / shipWeapon.fireRate;
            GameObject shot = Instantiate(projectile, transform.up/1.5f + transform.position, transform.rotation); //create the projectile
        }
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("The player has died!");
    }
}
