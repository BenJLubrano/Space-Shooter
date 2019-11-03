using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ship", menuName = "Ship")]
public class Ship : ScriptableObject
{
    public Sprite model;
    public AnimatorOverrideController animatorController;
    public GameObject explosion;

    [Header("Basic Ship Variables")]
    public int health;
    public int shield;
    public int speed;
    public int shieldRegenRate;
    public int shieldRegenTime;

    [Header("Movement Variables")]
    public float mass;
    public float drag;
    public float turnSpeed;
}
