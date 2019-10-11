using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ScriptableObject that contains data for the NPCs
//This might be removed and instead be turned into scripts since it doesn't really make sense to keep it as a scriptable object
[CreateAssetMenu(fileName = "Basic NPC", menuName = "NPC/Basic NPC", order = 0)]
public class BaseNpc : ScriptableObject
{
    public float aggroRange = 10f;
}
