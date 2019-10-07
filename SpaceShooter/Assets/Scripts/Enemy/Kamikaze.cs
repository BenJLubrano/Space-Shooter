using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Kamikaze", menuName = "Enemy/Kamikaze", order = 2)]
public class Kamikaze : BaseEnemy
{
    // Update is called once per frame
    public override void Update()
    {
        Debug.Log("I am a kamikaze enemy");
    }
}
