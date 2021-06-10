using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Scripts.Models;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class TeamController : MonoBehaviour
{
    [SerializeField]
    private List<PlayerController> players;

    private List<string> playerNames = new List<string>();

    [SerializeField] 
    private TMP_Text textGroupCode;

    [SerializeField] 
    private TMP_Text teamName;

    private string groupCode;

    void Start()
    {
        StartCoroutine(GenerateCode());
    }

    public List<string> GetPlayers()
    {
        foreach (PlayerController player in players)
        {
            playerNames.Add(player.GetPlayerName());
        }

        return playerNames;
    }

    public string GetTeamName()
    {
        return teamName.text;
    }

    private void SetText(string groupCode)
    {
        this.textGroupCode.text = "Groepscode: " + groupCode;
    }

    private IEnumerator GenerateCode()
    {
        using (UnityWebRequest createTeamRequest =
            UnityWebRequest.Post("https://avans-schalm-appserver.azurewebsites.net/api/game/", this.teamName.text))
        {
            yield return createTeamRequest.SendWebRequest();

            if (createTeamRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(createTeamRequest.error);
            }
            else
            {
                Debug.Log("Team Added!");

                this.groupCode = JsonUtility.FromJson<TeamGroupCode>(createTeamRequest.downloadHandler.text).gameCode.ToUpper();
                    
                PlayerPrefs.SetString("GAME_CODE", this.groupCode);

                SetText(this.groupCode);
            }
        }
    }

    public string GetGroupCode()
    {
        return this.groupCode;
    }
}
