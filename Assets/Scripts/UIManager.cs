using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject titleText;
    [SerializeField] GameObject startWaveText;

    bool titleShowing;
    bool startWaveShowing;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        titleShowing = true;
        startWaveShowing = true;
    }

    private void OnEnable()
    {
        SpawnEnemies.waveCompleted += ShowNextWaveText;
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

            }
        }
    }

    void ShowNextWaveText()
    {
        startWaveText.SetActive(true);
        startWaveShowing = true;
    }
}
