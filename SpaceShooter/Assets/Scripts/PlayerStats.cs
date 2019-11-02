using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] string characterName;
    [SerializeField] int level;
    [SerializeField] float reputation;
    [SerializeField] int units;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ModifyReputation(float change)
    {
        reputation += change;
    }
    public void AddUnits(int amount)
    {
        units += amount;
    }
    public void TakeUnits(int amount)
    {
        units -= amount;
    }
}
