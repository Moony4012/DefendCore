using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Turret_", menuName = "TurretData")]
public class TurretsData : ScriptableObject
{
    public string _label;
    public int _price;
    public GameObject _prefab;
}