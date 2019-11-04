using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int shipId = 0;

    //called whenever a ShipController is added. The ShipController will get a new ID
    public int RegisterShip()
    {
        shipId += 1;
        return shipId;
    }
}
