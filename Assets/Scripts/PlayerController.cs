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
        if (PlayerPrefs.HasKey("PLAYER_NAMES"))
        {
            var existing = JsonConvert.DeserializeObject<List<string>>(PlayerPrefs.GetString("PLAYER_NAMES"));
            if (existing != null) existing[playerId] = inputField.text;
        }
        _playerName = inputField.text;
    }
}
