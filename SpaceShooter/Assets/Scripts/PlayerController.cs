using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script controls the player, both movement and ingame functions
public class PlayerController : Ship
{
    private void Awake()
    {
        shipId = 0; //make sure the player is always ID 0
    }

    void Update()
    {
        base.Update(); //call the base ship update to perform generic functions
        
        //player movement stuff

        float verticalMove = Input.GetAxis("Vertical") * speed;
        verticalMove = (verticalMove < 0) ? verticalMove * speedPenalty : verticalMove;
        float horizontalMove = Input.GetAxis("Horizontal") * speed * speedPenalty;
        float rotationMove = Input.GetAxis("Rotation") * turnSpeed;

        if(rotationMove + verticalMove + horizontalMove != 0f) //if the player is moving in a certain direction or rotating
        {
            animator.SetFloat("AnimatorSpeed", Mathf.Abs(verticalMove));
            shipRb.angularVelocity = 0f; //set angular velocity to zero, which stops the player from rotating due to outside forces
        }

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
    }

    /*void Shoot()
    {
        if(weaponCooldown <= 0f)
        {
            weaponCooldown = 1 / shipWeapon.fireRate;
            GameObject shot = Instantiate(shipWeapon.projectile, transform.up/1.5f + transform.position, transform.rotation); //create the projectile
        }
    }*/

    protected override void Die()
    {
        base.Die();
        Debug.Log("The player has died!");
    }
}
