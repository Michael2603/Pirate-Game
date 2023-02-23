using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ----------------- Summary -----------------
//  This class is responsible for constantly monitoring the current
//  enemies in-game and respawning them when necessary.
// -------------------------------------------

public class SpawnManager : MonoBehaviour
{
    public List<Transform> SpawnList = new List<Transform>();
    public List<string> TagList = new List<string>();
    public GameObject ShooterEnemiesGroup;
    public GameObject EnemyShooter;
    public GameObject EnemyChaser;
    public int MaxEnemyAmount;

    private GameplayManager _gameplayManager;
    private float _shooterSpawnTimer;
    private float _chaserSpawnTimer;

    private void Start()
    {
        _gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        _shooterSpawnTimer = _gameplayManager.EnemySpawnInterval;
        _chaserSpawnTimer = _gameplayManager.EnemySpawnInterval * 1.5f;
    }

    void FixedUpdate()
    {
        // Spawns a shooter enemy after an amount of time when not at the capacity of enemies in the map.
        if (ShooterEnemiesGroup.transform.childCount < MaxEnemyAmount)
        {
            _shooterSpawnTimer -= Time.deltaTime;

            if (_shooterSpawnTimer <= 0)
            {
                _shooterSpawnTimer = _gameplayManager.EnemySpawnInterval;

                int randomIndex = Random.Range(0, SpawnList.ToArray().Length);
                
                GameObject tempBoat = Instantiate(EnemyShooter,
                SpawnList[randomIndex].position,
                SpawnList[randomIndex].rotation,
                ShooterEnemiesGroup.transform);

                tempBoat.tag = TagList[Random.Range(2, TagList.ToArray().Length)];
            }
        }


        // Constantly spawns chasers in the map but on a larger interval.
        _chaserSpawnTimer -= Time.deltaTime;

        if (_chaserSpawnTimer <= 0)
        {
            _chaserSpawnTimer = _gameplayManager.EnemySpawnInterval * 1.5f;

            int randomIndex = Random.Range(0, SpawnList.ToArray().Length);

            GameObject tempBoat = Instantiate(EnemyChaser,
            SpawnList[randomIndex].position,
            SpawnList[randomIndex].rotation);

            tempBoat.tag = TagList[1];
        }
    }
}
