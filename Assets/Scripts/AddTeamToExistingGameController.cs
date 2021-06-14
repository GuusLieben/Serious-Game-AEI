using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddTeamToExistingGameController : MonoBehaviour
{
    private string _gameCode;
    [SerializeField] private Text title;

    private void Start()
    {
        _gameCode = PlayerPrefs.GetString("GAME_CODE");
        title.text = _gameCode;
    }
}
