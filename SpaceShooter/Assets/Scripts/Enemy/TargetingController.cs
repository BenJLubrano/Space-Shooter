using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class TargetingController : MonoBehaviour
{
    [SerializeField] EnemyController controller;
    [SerializeField] CircleCollider2D targetingZone;
    [SerializeField] float radius;
    List<GameObject> targets = new List<GameObject>();
    [SerializeField] List<string> enemyFactions = new List<string>();

    private void Awake()
    {
        targetingZone.radius = radius;
        enemyFactions.Add("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (string tag in enemyFactions)
        {
            if (tag == collision.gameObject.tag)
            {
                Debug.Log("Found target");
                targets.Add(collision.gameObject);
                controller.UpdateTargets(targets);
                break;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        foreach (GameObject target in targets)
        {
            if (target == collision.gameObject)
            {
                targets.Remove(target);
                controller.UpdateTargets(targets);
                break;
            }
        }
    }


}
