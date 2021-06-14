using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class GameStatus
    {
        private Guid statusId;
        private bool isFinished;
        private Guid winningTeam;
        private Guid currentTeamId;
        private string currentTeamName;
        private string mimePlayer;
        private int roundCount;
        private string[] currentWords;
        private Dictionary<Guid, int> teamPositions;
    }
}