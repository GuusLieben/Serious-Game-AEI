using System;
using System.Collections.Generic;
using Models;

[Serializable]
public class Team
{
    public Guid TeamId;
    public string TeamName; 
    public List<Player> Players;
    
}
