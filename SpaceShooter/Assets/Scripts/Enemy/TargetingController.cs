using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

//This script controls the targeting for the NPC ships
public class TargetingController : MonoBehaviour
{
    [SerializeField] NpcController controller;
    [SerializeField] CircleCollider2D targetingZone;
    [SerializeField] public float radius;
    [SerializeField] List<string> enemyFactions = new List<string>();
    List<GameObject> targets = new List<GameObject>();

    private void Awake()
    {
        targetingZone.radius = radius;
        enemyFactions.Add("Player");
    }

    public void Initialize(List<string> factions)
    {
        enemyFactions = factions;
    }

    //Activates when a collider enters the targeting zone of the ship
    private void OnTriggerEnter2D(Collider2D collision)
    {
        string collisionFaction;

        try
        {
            collisionFaction = collision.gameObject.GetComponent<Ship>().GetFaction();
        }
        catch //we can't target it, so just exit.
        {
            return;
        }

        foreach (string tag in enemyFactions)
        {
            if (tag == collisionFaction) //if the tag is in the list of factions that this ship will attack
            {
                targets.Add(collision.gameObject); //add the target to the
                controller.UpdateTargets(targets);
                break;
            }
        }
    }

    public void ManualRemoveTarget(GameObject target)
    {
        if (targets.Contains(target))
            targets.Remove(target);
    }

    //Activates when a collider exits the targeting zone of the ship
    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (GameObject target in targets)
        {
            if (target == collision.gameObject) //if the gameobject was a target
            {
                targets.Remove(target); //remove it from the targets list
                controller.UpdateTargets(targets); //update the list of targets on the controller
                break;
            }
        }
    }

}
