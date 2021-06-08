using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamController : MonoBehaviour
{
    [SerializeField]
    private PlayerController[] players;

    PlayerController[] GetPlayers()
    {
        return players;
    }
}
