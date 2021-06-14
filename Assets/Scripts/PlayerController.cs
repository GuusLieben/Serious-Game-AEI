using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private int playerId;
    
    private string _playerName;
    public string PlayerName => _playerName;

    private void Start()
    {
        if (_playerName != null)
        {
            inputField.text = _playerName;
        }
    }

    public void SaveUserName()
    {
        List<string> playerNames = new List<string>();

        string oldJson = PlayerPrefs.GetString("PLAYER_NAMES", "");
        if (!string.IsNullOrEmpty(oldJson))
        {
            playerNames = JsonConvert.DeserializeObject<List<string>>(oldJson);
        }

        while(playerNames.Count < playerId + 1)
        {
            playerNames.Add("");
        }

        playerNames[playerId] = inputField.text;

        string newJson = JsonConvert.SerializeObject(playerNames);
        PlayerPrefs.SetString("PLAYER_NAMES", newJson);

        _playerName = inputField.text;
    }
}
