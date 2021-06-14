using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class AddTeamToExistingGameController : MonoBehaviour
{
    private string _gameCode;
    [SerializeField] private string url = "https://avans-schalm-appserver.azurewebsites.net/api/game/join?gameCode={0}";
    [SerializeField] private Text title;
    [SerializeField] private InputField player1Input;
    [SerializeField] private InputField player2Input;

    private void Start()
    {
        _gameCode = PlayerPrefs.GetString("GAME_CODE");
        title.text = _gameCode;
    }

    public void StartGame()
    {
        StartCoroutine(PostTeam());
    }

    private IEnumerator PostTeam()
    {
        if (player1Input.text.Equals("") || player2Input.text.Equals(""))
        {
            Debug.LogError("Names cannot be empty");
        }
        var players = new List<string>();
        
        players.Add(player1Input.text);
        players.Add(player2Input.text);
        
        // Development only, remove once hooked
        var team = new Team
        {
            TeamId = Guid.NewGuid(),
            TeamName = "Team 2",
            PlayerNames = players
        };

        var teamString = JsonConvert.SerializeObject(team);

        using var addTeamRequest = UnityWebRequest.Post(string.Format(url, PlayerPrefs.GetString("GAME_CODE")), "");
        
        addTeamRequest.SetRequestHeader("Content-Type", "application/json");
        
        var jsonToSend = new UTF8Encoding().GetBytes(teamString);
        addTeamRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        addTeamRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return addTeamRequest.SendWebRequest();

        if (addTeamRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Couldn't add team: " + addTeamRequest.error );
        }
        else
        {
            PlayerPrefs.SetString("TEAM_ID", team.TeamId.ToString());
            SceneManager.LoadScene("WaitingRoomScene");
        }
    }
}
