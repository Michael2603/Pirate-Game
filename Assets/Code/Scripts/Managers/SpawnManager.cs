using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public List<Transform> SpawnList = new List<Transform>();
    public List<string> TagList = new List<string>();
    public GameObject ShooterEnemiesGroup;
    public GameObject EnemyShooter;
    public int MaxEnemyAmount;

    private GameplayManager _gameplayManager;
    private float _spawnTimer;

    private void Awake()
    {
        _gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        _spawnTimer = _gameplayManager.EnemySpawnInterval;
    }

    void FixedUpdate()
    {
        // Spawns a shooter enemy after an amount of time when not at the capacity of enemies in the map.
        if (ShooterEnemiesGroup.transform.childCount < MaxEnemyAmount)
        {
            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0)
            {
                _spawnTimer = _gameplayManager.EnemySpawnInterval;

                int randomIndex = Random.Range(0, SpawnList.ToArray().Length);
                
                GameObject tempBoat = Instantiate(EnemyShooter,
                SpawnList[randomIndex].position,
                SpawnList[randomIndex].rotation,
                ShooterEnemiesGroup.transform);

                tempBoat.tag = TagList[Random.Range(1, TagList.ToArray().Length)];
            }
        }
    }
}
