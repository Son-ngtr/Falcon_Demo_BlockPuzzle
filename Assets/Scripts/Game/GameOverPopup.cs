using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Lớp thông báo sự kiện trò chơi
public class GameOverPopup : MonoBehaviour
{
    public GameObject gameOverPopup;
    public GameObject loosePopup;
    public GameObject newBestScorePopup;

    void Start()
    {
        gameOverPopup.SetActive(false);
    }

    private void OnEnable()
    {
        GameEvents.GameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameEvents.GameOver -= OnGameOver;
    }

    private void OnGameOver(bool newBestScore)
    {
        gameOverPopup.SetActive(true);
        loosePopup.SetActive(true);
        newBestScorePopup.SetActive(false);
    }
}
