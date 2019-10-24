using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPoint : Ship
{

    [SerializeField] Ship owner;

    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Die()
    {
        owner.TakeDamage(1000);
    }
}
