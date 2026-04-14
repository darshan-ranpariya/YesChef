using System;
using YesChef.Stations;

namespace YesChef.Core
{
    /// <summary>
    /// A lightweight static event bus to decouple our systems.
    /// UI and Managers can listen to these without needing direct references to objects.
    /// </summary>
    public static class GameEvents
    {
        // Passes the Window that finished, the base score of its order, and how long it took
        public static Action<CustomerWindow, int, float> OnOrderCompleted;
        
        // Passes the new total score so the UI can update
        public static Action<int> OnScoreChanged; 
    }
}