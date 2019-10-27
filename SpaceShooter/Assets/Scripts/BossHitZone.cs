using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitZone : MonoBehaviour
{
    [SerializeField] BossController boss;

    [SerializeField] public List<GameObject> shipsInHitZone = new List<GameObject>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ship ship = collision.GetComponent<Ship>();
        if(ship != null)
        {
            if(boss.enemyFactions.Contains(ship.GetFaction()))
            {
                shipsInHitZone.Add(collision.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (shipsInHitZone.Contains(collision.gameObject))
            shipsInHitZone.Remove(collision.gameObject);
    }
}
