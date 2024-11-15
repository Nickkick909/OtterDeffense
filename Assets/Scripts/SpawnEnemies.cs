using UnityEngine;
using UnityEngine.UIElements;

public class SpawnEnemies : MonoBehaviour
{

    [SerializeField] float minRange;
    [SerializeField] float maxRange;

    [SerializeField] GameObject[] enemies;

    [SerializeField] int enemiesToSpawn = 5;

    Transform player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                float randomDistance = Random.Range(minRange, maxRange);
                int randomIndex = Random.Range(0, enemies.Length);

                float randomDegrees = Random.Range(0, 360) * Mathf.Deg2Rad;

                float xAwayFromPlayer = Mathf.Sin(randomDegrees) * randomDistance;
                float zAwayFromPlayer = Mathf.Cos(randomDegrees) * randomDistance;

                Instantiate(enemies[randomIndex], new Vector3(player.position.x + xAwayFromPlayer, 0, player.position.z + zAwayFromPlayer), Quaternion.identity);
            }
        }
    }


}
