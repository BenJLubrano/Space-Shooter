using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int shipId = 0;

    //called whenever a ship is added. The ship will get a new ID
    public int RegisterShip()
    {
        shipId += 1;
        return shipId;
    }
}
