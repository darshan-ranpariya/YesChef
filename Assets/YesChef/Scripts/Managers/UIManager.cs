using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YesChef.Core;

namespace YesChef.Managers
{
    public class UIManager : MonoBehaviour
    {
        [Header("Panels")] [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject hudPanel;
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject gameOverPanel;

        [Header("HUD Texts")] [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI timerText;
        [SerializeField] private TextMeshProUGUI highScoreText;

        [Header("Game Over Texts")] [SerializeField]
        private TextMeshProUGUI finalScoreText;

        [SerializeField] private GameObject newHighScoreNotification;

        private int currentHighScore;

        private void OnEnable()
        {
            GameEvents.OnGameStateChanged += HandleGameStateChanged;
            GameEvents.OnTimeUpdated += HandleTimeUpdated;
            GameEvents.OnScoreChanged += HandleScoreChanged;
            GameEvents.OnHighScoreLoaded += HandleHighScoreLoaded;
        }

        private void OnDisable()
        {
            GameEvents.OnGameStateChanged -= HandleGameStateChanged;
            GameEvents.OnTimeUpdated -= HandleTimeUpdated;
            GameEvents.OnScoreChanged -= HandleScoreChanged;
            GameEvents.OnHighScoreLoaded -= HandleHighScoreLoaded;
        }

        private void HandleGameStateChanged(GameState state)
        {
            mainMenuPanel.SetActive(state == GameState.MainMenu);
            hudPanel.SetActive(state == GameState.Playing || state == GameState.Paused);
            pausePanel.SetActive(state == GameState.Paused);
            gameOverPanel.SetActive(state == GameState.GameOver);

            if (state == GameState.GameOver)
            {
                var finalScore = int.Parse(scoreText.text.Replace("Score: ", ""));
                finalScoreText.text = $"Final Score: {finalScore}";
                newHighScoreNotification.SetActive(finalScore > currentHighScore);
            }
        }

        private void HandleTimeUpdated(int seconds)
        {
            var ts = TimeSpan.FromSeconds(seconds);
            timerText.text = ts.ToString(@"m\:ss");
        }


        private void HandleScoreChanged(int newScore)
        {
            scoreText.text = $"Score: {newScore}";
        }

        private void HandleHighScoreLoaded(int score)
        {
            currentHighScore = score;
            highScoreText.text = $"High Score: {currentHighScore}";
        }
    }
}