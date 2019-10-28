using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitZone : MonoBehaviour
{
    [SerializeField] BossController boss;

    [SerializeField] List<Ship> shipsInHitZone = new List<Ship>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Ship ship = collision.GetComponent<Ship>();
        if(ship != null)
        {
            if(boss.enemyFactions.Contains(ship.GetFaction()))
            {
                shipsInHitZone.Add(ship);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Ship ship = collision.GetComponent<Ship>();
        if(ship != null && shipsInHitZone.Contains(ship))
            shipsInHitZone.Remove(ship);
    }


    public List<Ship> ShipsInZone()
    {
        return shipsInHitZone;
    }
}
