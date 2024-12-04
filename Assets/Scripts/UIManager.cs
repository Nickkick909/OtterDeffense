using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject titleText;
    [SerializeField] GameObject startWaveText;
    [SerializeField] GameObject playerHealth;
    [SerializeField] GameObject enemiesAlive;
    [SerializeField] GameObject gameOver;

    bool titleShowing;
    bool startWaveShowing;
    bool playerHealthShowing;
    bool enemiesAliveShowing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        titleShowing = true;
        startWaveShowing = true;
        playerHealth.SetActive(false);
        enemiesAlive.SetActive(false);
        gameOver.SetActive(false);
    }

    private void OnEnable()
    {
        SpawnEnemies.waveCompleted += ShowNextWaveText;
        SpawnEnemies.enemyCountUpdated += UpdateEnemyAliveCount;
        Player.playerDied += ShowGameOverScreen;
    }
    // Update is called once per frame
    void Update()
    {
        if (startWaveShowing || titleShowing)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (titleShowing)
                {
                    titleShowing = false;
                    titleText.SetActive(false);
                }

                if (startWaveShowing)
                {
                    startWaveShowing = false;
                    startWaveText.SetActive(false);
                }

                if (!playerHealthShowing)
                {
                    playerHealth.SetActive(true);
                }

                if (!enemiesAliveShowing)
                {
                    enemiesAlive.SetActive(true);
                }

            }
        }

    }

    void ShowNextWaveText()
    {
        startWaveText.SetActive(true);
        startWaveShowing = true;
    }

    void UpdateEnemyAliveCount(int enemiesAlive)
    {
        this.enemiesAlive.GetComponent<TMP_Text>().text = "Enemies alive: " + enemiesAlive.ToString();
    }

    void ShowGameOverScreen(GameObject player)
    {
        gameObject.SetActive(true);

    }
}
