using System;
using System.Collections.Generic;

[Serializable]
public class Team
{
    public Guid TeamId;
    public string TeamName;
    public List<string> PlayerNames;
}
