using System.Collections;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class NewTeamController : MonoBehaviour
{
    
    [SerializeField] private TeamController teamController;
    [SerializeField] private string url = "https://avans-schalm-appserver.azurewebsites.net/api/game/teams/count?gameCode={0}";

    public void StartGame()
    {
        StartCoroutine(PostTeam());
    }

    private IEnumerator PostTeam()
    {
        var teamName = teamController.GetTeamName();
        var players = teamController.GetPlayers();

        var team = new Team
        {
            TeamName = teamName,
            PlayerNames = players
        };

        var teamString = JsonConvert.SerializeObject(team);

        using var addTeamRequest = UnityWebRequest.Post(string.Format(url, PlayerPrefs.GetString("GAME_CODE")), "POST");
        
        addTeamRequest.SetRequestHeader("Content-Type", "application/json");
        
        var jsonToSend = new UTF8Encoding().GetBytes(teamString);
        addTeamRequest.uploadHandler = new UploadHandlerRaw(jsonToSend);
        addTeamRequest.downloadHandler = new DownloadHandlerBuffer();

        yield return addTeamRequest.SendWebRequest();
        Debug.Log(addTeamRequest.result != UnityWebRequest.Result.Success ? addTeamRequest.error : "Team Added!");
    }
}
