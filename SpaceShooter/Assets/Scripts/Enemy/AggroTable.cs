using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

    public void AddShip(ShipController ship, float aggro = 0)
    {
        AggroElement newEntry = new AggroElement(ship, aggro);
        aggroTable.Add(newEntry);
    }
    
    public void UpdateEntry(ShipController ship, float aggro)
    {
        foreach(AggroElement element in aggroTable)
        {
            if (element.ship == ship)
                element.aggro += aggro;
            return;
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

    public AggroElement GetTopAggro()
    {
        AggroElement highestAggro = new AggroElement(null, 0);
        foreach(AggroElement element in aggroTable)
        {
            if(element.aggro >= highestAggro.aggro)
            {
                highestAggro = element;
            }
        }
        return highestAggro;
    }

    public void RemoveElement(ShipController ship)
    {
        foreach(AggroElement element in aggroTable)
        {
            if (element.ship == ship)
            {
                aggroTable.Remove(element);
                return;
            }
        }
    }

    public List<AggroElement> GetElements()
    {
        return aggroTable;
    }
}
