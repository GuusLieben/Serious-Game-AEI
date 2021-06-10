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
            teamName = teamName,
            playerNames = players
        };

        string teamString = team.ToString();

        using (UnityWebRequest addTeamRequest =
            UnityWebRequest.Post("https://avans-schalm-appserver.azurewebsites.net/api/game/join?gameCode=" + this.teamController.GetGroupCode(), teamString))
        {
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
