using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Basic Enemy", menuName = "Enemy/Basic Enemy", order = 0)]
public class BaseEnemy : ScriptableObject
{
    public float aggroRange = 10f;
    // Update is called once per frame
    public virtual void Update()
    {
        Debug.Log("base");
    }

}
