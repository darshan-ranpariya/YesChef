using UnityEngine;
using YesChef.Core;

namespace YesChef.Managers
{
    public class GameManager : MonoBehaviour
    {
        private const float GAME_DURATION = 180f;
        private const string HIGH_SCORE_KEY = "YesChef_HighScore";

        public GameState CurrentState { get; private set; }

        private float timeRemaining;
        private int currentScore;
        private int highScore;

        private void OnEnable()
        {
            GameEvents.OnScoreChanged += HandleScoreChanged;
        }

        private void OnDisable()
        {
            GameEvents.OnScoreChanged -= HandleScoreChanged;
        }

        private void Start()
        {
            LoadHighScore();
            ChangeState(GameState.MainMenu);
        }

        private void Update()
        {
            if (CurrentState == GameState.Playing)
            {
                timeRemaining -= Time.deltaTime;

                // Update UI every frame with integer seconds
                GameEvents.OnTimeUpdated?.Invoke(Mathf.CeilToInt(timeRemaining));

                if (timeRemaining <= 0)
                {
                    EndGame();
                }
            }
        }

        public void StartGame()
        {
            currentScore = 0;
            timeRemaining = GAME_DURATION;
            GameEvents.OnScoreChanged?.Invoke(currentScore);

            ChangeState(GameState.Playing);
        }

        public void PauseGame()
        {
            if (CurrentState == GameState.Playing) ChangeState(GameState.Paused);
        }

        public void ResumeGame()
        {
            if (CurrentState == GameState.Paused) ChangeState(GameState.Playing);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        private void EndGame()
        {
            ChangeState(GameState.GameOver);
            CheckHighScore();
        }

        private void HandleScoreChanged(int newScore)
        {
            currentScore = newScore;
        }

        private void LoadHighScore()
        {
            highScore = PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
            GameEvents.OnHighScoreLoaded?.Invoke(highScore);
        }

        private void CheckHighScore()
        {
            if (currentScore > highScore)
            {
                highScore = currentScore;
                PlayerPrefs.SetInt(HIGH_SCORE_KEY, highScore);
                PlayerPrefs.Save();

                GameEvents.OnNewHighScore?.Invoke();
                Debug.Log("New High Score Achieved!");
            }
        }

        private void ChangeState(GameState newState)
        {
            CurrentState = newState;

            Time.timeScale = CurrentState == GameState.Playing ? 1f : 0f;

            GameEvents.OnGameStateChanged?.Invoke(CurrentState);
        }
    }
}