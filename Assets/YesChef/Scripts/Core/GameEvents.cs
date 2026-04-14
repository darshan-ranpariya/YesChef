using System;
using YesChef.Stations;

namespace YesChef.Core
{
    public static class GameEvents
    {
        public static Action<CustomerWindow, int, float> OnOrderCompleted;

        public static Action<int> OnScoreChanged;
        public static Action<GameState> OnGameStateChanged;
        public static Action<int> OnTimeUpdated;
        public static Action<int> OnHighScoreLoaded;
        public static Action OnNewHighScore;
    }

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
    }
}