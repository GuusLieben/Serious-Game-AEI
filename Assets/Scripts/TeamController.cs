using System;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class TeamController : MonoBehaviour
{
    private List<string> _playerNames;

    private string _groupCode;

    [SerializeField] private List<PlayerController> players;

    [SerializeField] private TMP_Text _textGroupCode;

    [SerializeField] private TMP_Text _teamName;

    [SerializeField] private string _groupTitle = "Groepscode: {0}";

    

    void Start()
    {
        _playerNames = new List<string>();
        StartCoroutine(GenerateCode());
    }

    public List<string> GetPlayers()
    {
        _playerNames.Clear();
        foreach (PlayerController player in players)
        {
            _playerNames.Add(player.GetPlayerName());
        }

        return _playerNames;
    }

    public string GetTeamName()
    {
        return _teamName.text;
    }

    private void SetText()
    {

        _textGroupCode.text = string.Format(_groupTitle, _groupCode);
    }

    private IEnumerator GenerateCode()
    {
        using (UnityWebRequest createTeamRequest =
            UnityWebRequest.Post("https://avans-schalm-appserver.azurewebsites.net/api/game/", _teamName.text))
        {
            yield return createTeamRequest.SendWebRequest();

            if (createTeamRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(createTeamRequest.error);
            }
            else
            {
                Debug.Log("Team Added!");

                _groupCode = JsonConvert
                    .DeserializeAnonymousType(createTeamRequest.downloadHandler.text, new {gameCode = ""}).gameCode;

                PlayerPrefs.SetString("GAME_CODE", _groupCode);

                SetText();
            }
        }
    }

    public string GetGroupCode()
    {
        return _groupCode;
    }
}
