using System;
using System.Collections.Generic;

[Serializable]
public class Team
{
    public Guid teamId;
    public string TeamName;
    public List<string> PlayerNames;
}
