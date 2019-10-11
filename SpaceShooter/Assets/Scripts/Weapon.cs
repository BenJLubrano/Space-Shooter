using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ScriptableObject that acts as a container for weapon stats
[System.Serializable]
[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField] public float damage = 1;
    [SerializeField] public float fireRate = 1;
    [SerializeField] public float projectileSpeed = 1;
    [SerializeField] public Sprite projectileImage;
    [SerializeField] public float range = 25f;
    [SerializeField] public AudioClip sound;

}
