using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Ship
{
    GameObject ship;
    Rigidbody2D rb;

    float weaponCooldown = 0f;
    // Start is called before the first frame update
    void Start()
    {
        ship = this.gameObject;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        base.Update();
        weaponCooldown -= Time.deltaTime;
        float verticalMove = Input.GetAxis("Vertical") * speed;
        verticalMove = (verticalMove < 0) ? verticalMove * speedPenalty : verticalMove;
        float horizontalMove = Input.GetAxis("Horizontal") * speed * speedPenalty;
        float rotation = Input.GetAxis("Rotation") * turnSpeed;
        if(rotation + verticalMove + horizontalMove != 0f)
        {
            rb.angularVelocity = 0f;
        }
        Vector2 force = new Vector2(horizontalMove, verticalMove);
        rb.MoveRotation(rb.rotation + rotation);

        rb.AddForce(transform.TransformDirection(force));

        if (Input.GetButton("Brake"))
        {
            rb.drag = brakeStrength;
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
            GameObject shot = Instantiate(projectile, transform.up/1.5f + transform.position, transform.rotation);
            //shot.GetComponent<Projectile>().AddVelocity(rb.velocity);
        }
    }
}
