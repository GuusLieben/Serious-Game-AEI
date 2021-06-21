using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using Models;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class TeamController : MonoBehaviour
{
    
    private List<Player> _playerNames;
    private string _groupCode;
    
    [SerializeField] private TMP_Text textGroupCode;
    [SerializeField] private TMP_Text teamName;
    [SerializeField] private string groupTitle = "Groepscode: {0}";
    [SerializeField] private string url = "https://avans-schalm-appserver.azurewebsites.net/api/game/";

    private void Start()
    {
        _playerNames = new List<Player>();
        StartCoroutine(GenerateCode());
    }

    public List<Player> GetPlayers()
    {
        _playerNames = JsonConvert.DeserializeObject<List<Player>>(PlayerPrefs.GetString("PLAYER_NAMES"));
        return _playerNames;
    }

    public string GetTeamName()
    {
        return teamName.text;
    }

    private void UpdateGroupCodeText()
    {
        textGroupCode.text = string.Format(groupTitle, _groupCode);
    }

    private IEnumerator GenerateCode()
    {
        using var createTeamRequest =
            UnityWebRequest.Post(url, teamName.text);
        yield return createTeamRequest.SendWebRequest();

        if (createTeamRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(createTeamRequest.error);
        }
        else
        {
            Debug.Log("Created a new game!");

            _groupCode = JsonConvert.DeserializeAnonymousType(createTeamRequest.downloadHandler.text, new {gameCode = ""})?.gameCode;

            PlayerPrefs.SetString("GAME_CODE", _groupCode);
            UpdateGroupCodeText();
        }
    }
}
