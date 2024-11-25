using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemies : MonoBehaviour
{

    [SerializeField] float minRange;
    [SerializeField] float maxRange;

    [SerializeField] GameObject[] enemies;

    [SerializeField] int enemiesToSpawn = 5;

    public List<GameObject> enemiesAlive = new List<GameObject>();

    Transform player;

    public delegate void WaveCompleted();
    public static WaveCompleted waveCompleted;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void OnEnable()
    {
        waveCompleted += WaveFinshed;
        Enemy.enemieDied += RemoveEnemyFromList;
    }

    private void OnDisable()
    {
        waveCompleted -= WaveFinshed;
        Enemy.enemieDied -= RemoveEnemyFromList;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
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

            yield return new WaitForSeconds(0.1f);
        }

        yield return null;

        // Spawn in random power ups
    }

    void RemoveEnemyFromList(GameObject enemy)
    {
        enemiesAlive.Remove(enemy);
    }

    void WaveFinshed()
    {
        // Once a wave is finished

        // Display some text like "Wave completed"
        // Do this in a UI controller delegate function

        // Either a count down or wait for input to start next wave
    }
}
