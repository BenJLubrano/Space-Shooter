using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

//This script controls the targeting for the NPC ships
public class TargetingController : MonoBehaviour
{
    [SerializeField] NpcController controller;
    [SerializeField] CircleCollider2D targetingZone;
    [SerializeField] float radius;
    [SerializeField] List<string> enemyFactions = new List<string>();
    List<GameObject> targets = new List<GameObject>();

    private void Awake()
    {
        targetingZone.radius = radius;
        enemyFactions.Add("Player");
    }

    //Activates when a collider enters the targeting zone of the ship
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tag in enemyFactions)
        {
            if (tag == collision.gameObject.tag) //if the tag is in the list of factions that this ship will attack
            {
                targets.Add(collision.gameObject); //add the target to the
                controller.UpdateTargets(targets);
                break;
            }
        }
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
