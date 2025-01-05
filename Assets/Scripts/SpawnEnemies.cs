using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{
    const int WAVE_1_ENEMIES = 5;
    const int WAVE_1_POWERUPS = 1;
    [SerializeField] float minRange;
    [SerializeField] float maxRange;

    [SerializeField] GameObject[] enemies;
    [SerializeField] GameObject[] powerUps;

    [SerializeField] int enemiesToSpawn = WAVE_1_ENEMIES;
    [SerializeField] int powerUpsToSpawn = WAVE_1_POWERUPS;

    int maxPowerUpsToSpawn = 5;

    public List<GameObject> enemiesAlive = new List<GameObject>();

    Transform player;

    public delegate void WaveCompleted();
    public static WaveCompleted waveCompleted;

    public delegate void EnemyCountUpdated(int enemiesAlive);
    public static EnemyCountUpdated enemyCountUpdated;

    public delegate void WaveStarted(int waveNumber);
    public static WaveStarted waveStarted;

    bool waveActive;
    bool gameOver = false;

    public int waveNumber = 0;

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
        Player.playerDied += GameOver;
        UIManager.resetGame += ResetGame;
        UIManager.startGame += SpawnNewWave;
    }

    private void OnDisable()
    {
        waveCompleted -= WaveFinshed;
        Enemy.enemyDied -= RemoveEnemyFromList;
        Player.playerDied -= GameOver;
        UIManager.resetGame -= ResetGame;
        UIManager.startGame += SpawnNewWave;
    }

    void Update()
    {
        if (!gameOver && !waveActive && Input.GetKeyDown(KeyCode.Space))
        {
            SpawnNewWave();
        }
    }

    void SpawnNewWave()
    {
        if (waveActive == false)
        {
            waveActive = true;
            StartCoroutine(SpawnNextWave());
        }
    }

    public IEnumerator SpawnNextWave()
    {
        waveNumber += 1;
        waveStarted?.Invoke(waveNumber);

        powerUpsToSpawn = Random.Range(0, maxPowerUpsToSpawn);
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

        Debug.Log("Enemies to spawn: "+ enemiesToSpawn+ " Range to add: "+ enemiesToSpawn * 0.5f+ " - "+ enemiesToSpawn * 1.25f);

        enemiesToSpawn += Random.Range(Mathf.RoundToInt(enemiesToSpawn * 0.5f), Mathf.RoundToInt(enemiesToSpawn * 1.25f));

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
        waveActive = false;
    }

    void GameOver(GameObject player)
    {
        gameOver = true;
    }

    void ResetGame()
    {
        waveNumber = 0;
        enemiesToSpawn = WAVE_1_ENEMIES;
        powerUpsToSpawn = WAVE_1_POWERUPS;
        foreach (var enemy in enemiesAlive)
        {
            Destroy(enemy);
        }
        enemiesAlive.Clear();

        gameOver = false;

        waveActive = true;
        StartCoroutine(SpawnNextWave());
    }
}
