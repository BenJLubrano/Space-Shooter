using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
//This script controls the player, both movement and ingame functions
public class PlayerController : ShipController
{
    [SerializeField] Animator shieldAnim;
    [SerializeField] Transform spawnPoint;
    //TEMPORARY
    [SerializeField] TextMeshProUGUI reputationText;

    [SerializeField] bool mouseMovement = false;
    void Update()
    {
        base.Update(); //call the base ShipController update to perform generic functions
        if (Input.GetButtonDown("Toggle Mouse Rotation"))
            ToggleMouseMovement();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        //player movement stuff
        if(Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0)
        {
            if (Input.GetAxisRaw("Vertical") > 0 && thrusterPower < 0) //If the player was going backwards and now wants to go forwards, don't make them have to wait for double the time
                thrusterPower = 0;
            thrusterPower = thrusterPower + Input.GetAxisRaw("Vertical") * acceleration * Time.deltaTime;
            if (thrusterPower < -1)
                thrusterPower = -1;
            if (thrusterPower > 1)
                thrusterPower = 1;
        }
        else
        {
            if (thrusterPower < 0)
                thrusterPower = thrusterPower + 1 * Time.deltaTime > 0 ? 0 : thrusterPower + 1 * Time.deltaTime;
            else if (thrusterPower > 0)
                thrusterPower = thrusterPower - 1 * Time.deltaTime < 0 ? 0 : thrusterPower - 1 * Time.deltaTime;
        }
        float verticalMove = thrusterPower * speed * (speedConst * shipRb.mass) * Time.deltaTime;
        verticalMove = (verticalMove < 0) ? verticalMove * speedPenalty : verticalMove;
        float horizontalMove = Input.GetAxis("Horizontal") * speed * (speedConst * shipRb.mass) * speedPenalty * Time.deltaTime;

        float rotationMove = 0;
        if(mouseMovement && Input.GetAxis("Rotation") == 0) //mouse rotate
        {
            //Commented out code rotates towards a mouse position... but it felt jittery at times
            /*Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);*/
            float mouseRotation = Input.GetAxis("Mouse X");

            //These lines clamp the rotation speed. Moving your mouse faster won't make it rotate faster.
            if (mouseRotation > 1)
                mouseRotation = 1;
            else if (mouseRotation < -1)
                mouseRotation = -1;

            rotationMove = -1 * mouseRotation * turnSpeed;
        }
        else
        {
            rotationMove += Input.GetAxis("Rotation") * turnSpeed;
        }


        if (rotationMove + verticalMove + horizontalMove != 0f) //if the player is moving in a certain direction or rotating
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
            shipRb.drag = defaultDrag * 5; //up the drag, which causes the player to stop faster
            shipRb.angularVelocity = 0f;
        }
        else
        {
            turnSpeed = defaultTurnSpeed;
            shipRb.drag = defaultDrag;
        }

        if (Input.GetButton("Shoot"))
        {
            Shoot(null, new List<string> { "Federation", "Neutral", "Pirate" });
        }
    }
    
    void HandleAnimation(float currentSpeed)
    {
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            shipAnimator.SetBool("IsAccelerating", true);
        }
        else
        {
            shipAnimator.SetBool("IsAccelerating", false);
        }


        shipAnimator.SetFloat("ThrusterPower", thrusterPower);

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

    public void UpdateReputationDisplay()
    {
        try
        {
            reputationText.text = "Reputation: " + stats.reputation;
        }
        catch
        {
            //There is no reputation Text
        }
    }

    public void ToggleMouseMovement()
    {
        mouseMovement = !mouseMovement;
        if(mouseMovement)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
