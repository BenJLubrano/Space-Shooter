using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

//controller for all NPC ships
public class NpcController : Ship
{

    [SerializeField] BaseNpc npcType; //the type of NPC that the ship is (This might be changed)
    [SerializeField] List<GameObject> targets;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Collider2D collider;
    [SerializeField] SpriteRenderer renderer;

    Ship currentTarget;

    void Update()
    {
        base.Update(); //call base ship update
        if(currentTarget == null)
        {
            LookForTargets();
        }
        else
        {
            if(OutOfRange())
            {
                Deaggro();
            }
        }
        Move();
    }

    //Logic related to targeting
    void LookForTargets()
    {
        if (targets.Count == 0)
            return;
        GameObject tempTarget = targets[0];
        float closest = Vector2.Distance(gameObject.transform.position, tempTarget.transform.position);
        foreach(GameObject target in targets)
        {
            float tempDist = Vector2.Distance(gameObject.transform.position, target.transform.position);
            if (tempDist < closest)
            {
                tempTarget = target;
                closest = tempDist;
            }
        }
        currentTarget = tempTarget.GetComponent<Ship>();
    }

    //Used to get target updates from the TargetingController
    public void UpdateTargets(List<GameObject> newTargets)
    {
        targets = newTargets;
    }

    //Whether or not the target is out of range
    bool OutOfRange()
    {
        return Vector2.Distance(currentTarget.gameObject.transform.position, gameObject.transform.position) > npcType.aggroRange;
    }

    //Called when the npc "loses interest" in a target
    void Deaggro()
    {
        //going to do other stuff here later
        currentTarget = null;
    }
    
    void Move()
    {
        if(currentTarget != null)
        {
            transform.up = currentTarget.gameObject.transform.position - transform.position;
            rb.AddForce(transform.up * speed);
        }
    }

    //This is a temporary function, sort of a cheat to get the melee enemies to work. This will be revised into a more modular attack system
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("player");
            audioSource.clip = shipWeapon.sound;
            audioSource.Play();
            collision.gameObject.GetComponent<Ship>().TakeDamage(shipWeapon.damage);
        }
    }

    protected override void Die()
    {
        collider.enabled = false;
        renderer.enabled = false;
        base.Die();
    }
}
