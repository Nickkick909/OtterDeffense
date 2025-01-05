using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Player;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject titleText;
    [SerializeField] GameObject startWaveText;
    [SerializeField] GameObject playerHealth;
    [SerializeField] GameObject enemiesAlive;
    [SerializeField] GameObject gameOver;
    [SerializeField] GameObject startGamebutton;
    [SerializeField] GameObject tutorialButton;
    [SerializeField] GameObject tutorialCard;
    [SerializeField] GameObject scoreCard;
    [SerializeField] GameObject weaponCard;
    //[SerializeField] GameObject currentScoreText;
    //[SerializeField] GameObject currentScoreEndGameText;
    [SerializeField] GameObject highScoreText;
    [SerializeField] GameObject waveNumber;

    [SerializeField] TMP_Text currentScoreTMP;
    [SerializeField] TMP_Text currentScoreEndGameTMP;
    [SerializeField] TMP_Text highScoreTMP;
    [SerializeField] TMP_Text attackTypeTMP;
    [SerializeField] TMP_Text ammoCountTMP;

    bool titleShowing;
    bool startWaveShowing;
    bool playerHealthShowing;
    bool enemiesAliveShowing;

    ProgressBarCircle playerHealthBar;

    public delegate void ResetGame();
    public static ResetGame resetGame;

    public delegate void StartGameButton();
    public static StartGameButton startGame;

    public int currentRoundScore = 0;
    public int highScore = 0;

    void Start()
    {
        titleShowing = true;
        startWaveShowing = true;
        playerHealth.SetActive(false);
        scoreCard.SetActive(false);
        weaponCard.SetActive(false);
        enemiesAlive.SetActive(false);
        gameOver.SetActive(false);
        waveNumber.SetActive(false);

        playerHealthBar = playerHealth.GetComponent<ProgressBarCircle>();

        highScore = PlayerPrefs.GetInt("highScore");
    }

    private void OnEnable()
    {
        SpawnEnemies.waveCompleted += ShowNextWaveText;
        SpawnEnemies.enemyCountUpdated += UpdateEnemyAliveCount;
        SpawnEnemies.waveStarted += UpdateWaveNumber;
        Player.playerDied += ShowGameOverScreen;
        Player.playerHealth += UpdatePlayerHealthBar;
        Enemy.enemyDied += UpdateCurrentScore;
        Player.updateAttackType += UpdateAttackType;
        Player.updateAttackAmmo += UpdateAttackAmmo;
    }

    private void OnDisable()
    {
        SpawnEnemies.waveCompleted -= ShowNextWaveText;
        SpawnEnemies.enemyCountUpdated -= UpdateEnemyAliveCount;
        SpawnEnemies.waveStarted -= UpdateWaveNumber;
        Player.playerDied -= ShowGameOverScreen;
        Player.playerHealth -= UpdatePlayerHealthBar;
        Enemy.enemyDied -= UpdateCurrentScore;
        Player.updateAttackType -= UpdateAttackType;
        Player.updateAttackAmmo -= UpdateAttackAmmo;
    }

    void Update()
    {
        if(startWaveShowing || titleShowing)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartGame();

            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        

    }
    public void StartGame()
    {
        if (gameOver.activeSelf)
        {
            gameOver.SetActive(false);
            playerHealth.SetActive(true);
            resetGame?.Invoke();

            currentRoundScore = 0;
        }

        titleShowing = false;
        titleText.SetActive(false);

        startWaveShowing = false;
        startWaveText.SetActive(false);
        

        playerHealth.SetActive(true);
        scoreCard.SetActive(true);
        weaponCard.SetActive(true);

        enemiesAlive.SetActive(true);
      
        startGamebutton.SetActive(false);
        tutorialButton.SetActive(false);

        waveNumber.SetActive(true);
        

        startGame?.Invoke();
    }

    public void ShowTutorial()
    {
        tutorialCard.SetActive(true);
    }

    public void HideTutorial()
    {
        tutorialCard.SetActive(false);
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
        if (currentRoundScore > highScore)
        {
            highScore = currentRoundScore;
            PlayerPrefs.SetInt("highScore", highScore);

           
        }

        highScoreTMP.text = highScore.ToString();

        gameOver.SetActive(true);
        startGamebutton.SetActive(true);
        scoreCard.SetActive(true);

        weaponCard.SetActive(false);
        scoreCard.SetActive(false);
        playerHealth.SetActive(false);
        startWaveText.SetActive(false);
    }

    void UpdatePlayerHealthBar(float health)
    {
        if (playerHealthBar == null)
        {
            playerHealthBar = playerHealth.GetComponent<ProgressBarCircle>();
        }

        if (playerHealthBar != null)
        {
            playerHealthBar.BarValue = health;
        }
    }

    void UpdateCurrentScore(GameObject enemy)
    {
        currentRoundScore += 5;

        currentScoreTMP.text = currentRoundScore.ToString();
        currentScoreEndGameTMP.text = currentRoundScore.ToString();
    }

    void UpdateWaveNumber(int newWaveNumber)
    {
        waveNumber.GetComponent<TMP_Text>().text = "Wave #" + newWaveNumber.ToString();
    }

    void UpdateAttackType(AttackType attackType)
    {
        attackTypeTMP.text = attackType.ToString();
    }

    void UpdateAttackAmmo(int ammo)
    {
        ammoCountTMP.text = ammo.ToString();
    }
}
