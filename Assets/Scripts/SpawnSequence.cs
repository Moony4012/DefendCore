using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Level_X", menuName = "SpawnSequence")]
public class SpawnSequence : ScriptableObject
{
    [Serializable]
    public class EntityToSpawn
    {
        [SerializeField]
        public GameObject _entity;
        [SerializeField]
        public List<MinMaxPair> _times;
    }

    [Serializable]
    public class Wave
    {
        [SerializeField]
        public List<EntityToSpawn> _entities;
    }

    [SerializeField]
    public Wave _waves1;
    [SerializeField]
    public Wave _waves2;
    [SerializeField]
    public Wave _waves3;
}
