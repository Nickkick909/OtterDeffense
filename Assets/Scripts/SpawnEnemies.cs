using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{

    [SerializeField] float minRange;
    [SerializeField] float maxRange;

    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject[] powerUps;

    [SerializeField] int enemiesToSpawn = 5;
    [SerializeField] int powerUpsToSpawn = 1;

    int maxPowerUpsToSpawn = 5;

    public List<GameObject> enemiesAlive = new List<GameObject>();

    Transform player;

    public delegate void WaveCompleted();
    public static WaveCompleted waveCompleted;

    public delegate void EnemyCountUpdated(int enemiesAlive);
    public static EnemyCountUpdated enemyCountUpdated;

    bool waveActive;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        waveActive = false;

        foreach (GameObject p in powerUps)
        {
            p.SetActive(false);
        }
    }

    private void OnEnable()
    {
        waveCompleted += WaveFinshed;
        Enemy.enemyDied += RemoveEnemyFromList;
    }

    private void OnDisable()
    {
        waveCompleted -= WaveFinshed;
        Enemy.enemyDied -= RemoveEnemyFromList;
    }

    void Update()
    {
        if (!waveActive && Input.GetKeyDown(KeyCode.Return))
        {
            StartCoroutine(SpawnNextWave());
        }
    }

    public IEnumerator SpawnNextWave()
    {
        // Spawn Enemies in a radius around player
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            float randomDistance = Random.Range(minRange, maxRange);
            int randomIndex = Random.Range(0, enemies.Length);

            float randomDegrees = Random.Range(0, 360) * Mathf.Deg2Rad;

            float xAwayFromPlayer = Mathf.Sin(randomDegrees) * randomDistance;
            float zAwayFromPlayer = Mathf.Cos(randomDegrees) * randomDistance;

            enemiesAlive.Add(Instantiate(enemies[randomIndex], new Vector3(player.position.x + xAwayFromPlayer, 0, player.position.z + zAwayFromPlayer), Quaternion.identity));

            enemyCountUpdated?.Invoke(enemiesAlive.Count);

            yield return new WaitForSeconds(0.25f);
        }

        
        yield return null;

        enemiesToSpawn += Random.Range(Mathf.RoundToInt(enemiesToSpawn * 1.25f), Mathf.RoundToInt(enemiesToSpawn * 2f));

        powerUpsToSpawn = Random.Range(0,maxPowerUpsToSpawn);
        // Spawn in random power ups
        for (int i = 0; i < powerUpsToSpawn; i++)
        {
            int randomIndex = Random.Range(0, powerUps.Length);

            int tryCount = 0;
            int tryMax = 3;
            while (powerUps[randomIndex].activeSelf && tryCount < tryMax)
            {
                // Try 3 times to spawn a new power up
                // If we hit already active power ups 3 times, then the universe decided we didn't need that power up lol
                randomIndex = Random.Range(0, powerUps.Length);
                tryCount++;
            }

            powerUps[randomIndex].SetActive(true);
        }

    }

    void RemoveEnemyFromList(GameObject enemy)
    {
        enemiesAlive.Remove(enemy);

        enemyCountUpdated?.Invoke(enemiesAlive.Count);

        if (enemiesAlive.Count < 1)
        {
            waveCompleted?.Invoke();
        }
    }

    void WaveFinshed()
    {
        // Once a wave is finished

        // Display some text like "Wave completed"
        // Do this in a UI controller delegate function

        // Either a count down or wait for input to start next wave
    }
}
