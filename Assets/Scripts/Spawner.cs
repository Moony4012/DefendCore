using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public const int _waveCount = 3;
    public const float _waveDuration = 20.0f;

    [SerializeField]
    private SpawnSequence _sequence;

    [System.Serializable]
    private class SpawnInfo 
    {
        public float _time = 0;
        public GameObject _prefab = null;
        public Vector2 _startPosition;
        public Vector2 _endPosition;
    }

    private float _time = 0;
    private List<SpawnInfo> _spawnInfos = new List<SpawnInfo>();

    // Handler Spawn of Alien (+boss +Asteroid?) and Round 1,2,3
    void Start()
    {
        SpawnSequence.Wave[] waves = new SpawnSequence.Wave[_waveCount];
        waves[0] = _sequence._waves1;
        waves[1] = _sequence._waves2;
        waves[2] = _sequence._waves3;

        for (int waveIndex = 0; waveIndex < waves.Length; ++waveIndex)
        {
            SpawnSequence.Wave wave = waves[waveIndex];
            float waveTimeOffset = _waveDuration * waveIndex;

            for (int entityIndex = 0; entityIndex < wave._entities.Count; entityIndex++)
            {
                SpawnSequence.EntityToSpawn entity = wave._entities[entityIndex];

                for (int timeIndex = 0; timeIndex < entity._times.Count; timeIndex++)
                {
                    float randomAngleInRadian = Random.Range(0, 360) * Mathf.Deg2Rad;
                    Vector2 randomDirection = new Vector2(Mathf.Cos(randomAngleInRadian), Mathf.Sin(randomAngleInRadian));
                    float endDistance = Random.Range(4.5f, 5.5f);

                    SpawnInfo spawnInfo = new SpawnInfo();
                    spawnInfo._time = entity._times[timeIndex].RandomValue * _waveDuration + waveTimeOffset;
                    spawnInfo._prefab = entity._entity;
                    spawnInfo._startPosition = new Vector2(0, 0) + randomDirection * 10.0f;
                    spawnInfo._endPosition = new Vector2(0, 0) + randomDirection * endDistance;
                    _spawnInfos.Add(spawnInfo);
                }
            }
        }
    }

    public bool HasFinish()
    {
        return _spawnInfos.Count == 0;
    }

    void Update()
    {
        _time += Time.deltaTime;
        GameObject.FindObjectOfType<HUD>().OnBarChanged(_time);

        foreach (SpawnInfo spawnInfo in _spawnInfos)
        {
            if (_time >= spawnInfo._time)
            {
                GameObject entity = GameObject.Instantiate(spawnInfo._prefab);
                entity.transform.position = spawnInfo._startPosition;
                if (entity.GetComponent<Alien>() != null)
                {
                    entity.GetComponent<Alien>().SetEndPosition(spawnInfo._endPosition);
                }
                _spawnInfos.Remove(spawnInfo);
            }
        }
    }
}
