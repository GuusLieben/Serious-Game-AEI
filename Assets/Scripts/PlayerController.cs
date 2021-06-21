using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Models;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private TMP_InputField ageField;
    [SerializeField] private int playerId;
    
    private string _playerName;

    private void Start()
    {
        if (_playerName != null)
        {
            nameField.text = _playerName;
        }
    }

    public void SaveUserName()
    {
        if (string.IsNullOrEmpty(nameField.text) || string.IsNullOrEmpty(ageField.text)) return;
        List<Player> playerNames = new List<Player>();

        string oldJson = PlayerPrefs.GetString("PLAYER_NAMES", "");
        if (!string.IsNullOrEmpty(oldJson))
        {
            playerNames = JsonConvert.DeserializeObject<List<Player>>(oldJson);
        }

        while(playerNames.Count < playerId + 1)
        {
            playerNames.Add(new Player());
        }

        playerNames[playerId] = new Player()
        {
            Name = nameField.text,
            Age = int.Parse(ageField.text)
        };

        string newJson = JsonConvert.SerializeObject(playerNames);
        PlayerPrefs.SetString("PLAYER_NAMES", newJson);

        _playerName = nameField.text;
    }
}
