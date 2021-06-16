using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class GameStatus
    {
        public Guid statusId;
        public bool isFinished;
        public Team winningTeam;
        public Guid currentTeamId;
        public string currentTeamName;
        public string mimePlayer;
        public int roundCount;
        public string[] currentWords;
        public Dictionary<Guid, int> teamPositions;
    }
}