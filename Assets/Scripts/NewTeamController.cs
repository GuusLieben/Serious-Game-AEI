using System;
using System.Collections;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class NewTeamController : MonoBehaviour
{
    
    [SerializeField] private TeamController teamController;
    [SerializeField] private string url = "https://avans-schalm-appserver.azurewebsites.net/api/game/join?gameCode={0}";
    [SerializeField] private string newScene;

    public void StartGame()
    {
        StartCoroutine(PostTeam());
        LoadNewScene();
    }

    public void LoadNewScene()
    {
        SceneManager.LoadScene(newScene);
    }

    private IEnumerator PostTeam()
    {
        var teamName = teamController.GetTeamName();
        var players = teamController.GetPlayers();

        var team = new Team
        {
            TeamId = Guid.NewGuid(),
            TeamName = teamName,
            Players = players
        };
        PlayerPrefs.SetString("TEAM_ID", team.TeamId.ToString());
        
        var teamString = JsonConvert.SerializeObject(team);

        using var addTeamRequest = UnityWebRequest.Post(string.Format(url, PlayerPrefs.GetString("GAME_CODE")), "");
        
        addTeamRequest.SetRequestHeader("Content-Type", "application/json");
        
        Debug.Log(teamString);
        
        var jsonToSend = new UTF8Encoding().GetBytes(teamString);
        addTeamRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        addTeamRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return addTeamRequest.SendWebRequest();
        Debug.Log(addTeamRequest.result != UnityWebRequest.Result.Success ? addTeamRequest.error : "Team Added!");
    }
}
