using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroElement
{
    public AggroElement(ShipController newShip, float agr)
    {
        ship = newShip;
        aggro = agr;
    }
    public ShipController ship;
    public float aggro;
}

public class AggroTable
{
    List<AggroElement> aggroTable = new List<AggroElement>();

    void AddShip(ShipController ship, float aggro)
    {
        AggroElement newEntry = new AggroElement(ship, aggro);
        aggroTable.Add(newEntry);
    }
    
    void UpdateEntry(ShipController ship, float aggro)
    {
        foreach(AggroElement element in aggroTable)
        {
            if (element.ship == ship)
                element.aggro += aggro;
        }
    }

    public bool IsInTable(ShipController ship)
    {
        foreach (AggroElement element in aggroTable)
        {
            if (element.ship == ship)
                return true;
        }
        return false;
    }

    public GameObject GetTopAggro()
    {
        AggroElement highestAggro = new AggroElement(null, 0);
        foreach(AggroElement element in aggroTable)
        {
            if(element.aggro >= highestAggro.aggro)
            {
                highestAggro = element;
            }
        }
        return highestAggro.ship.gameObject;
    }

    public void RemoveElement(ShipController ship)
    {
        foreach(AggroElement element in aggroTable)
        {
            if (element.ship == ship)
            {
                aggroTable.Remove(element);
            }
        }
    }
}
