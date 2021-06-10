using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NewTeamController : MonoBehaviour
{
    [SerializeField]
    private TeamController teamController;

    public void StartGame()
    {
        StartCoroutine(PostTeam());
    }

    IEnumerator PostTeam()
    {
        string teamName = this.teamController.GetTeamName();
        List<string> players = this.teamController.GetPlayers();

        Team team = new Team()
        {
            TeamName = teamName,
            PlayerNames = players
        };

        string teamString = JsonConvert.SerializeObject(team);

        using (UnityWebRequest addTeamRequest =
        UnityWebRequest.Post("https://avans-schalm-appserver.azurewebsites.net/api/game/join?gameCode=" + PlayerPrefs.GetString("GAME_CODE"), "POST"))
        {
            addTeamRequest.SetRequestHeader("Content-Type", "application/json");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(teamString);

            addTeamRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            addTeamRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            yield return addTeamRequest.SendWebRequest();
            Debug.Log("TeamResult: " + addTeamRequest.result);

            if (addTeamRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(addTeamRequest.error);
            }
            else
            {
                Debug.Log("Team Added!");

            }
        }
    }
}
