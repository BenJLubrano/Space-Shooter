﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Laser Weapon", menuName = "Weapon/Laser Weapon", order = 2)]
public class LaserWeapon : Weapon
{
    [SerializeField] public float startLength;
    [SerializeField] public float maxLength;
    [SerializeField] public float growSpeed;

}
