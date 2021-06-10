using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Models;
using Newtonsoft.Json;

public class WaitingRoomSceneManager : MonoBehaviour
{
    private static string url = "https://avans-schalm-appserver.azurewebsites.net/api/game/teams/count?gameCode=";
    
    private string gameCode;

    private bool isReady;
    // Start is called before the first frame update
    void Start()
    {
        isReady = false;
        PlayerPrefs.SetString("GAME_CODE", "TswNEv");
        gameCode = PlayerPrefs.GetString("GAME_CODE");
        Invoke("StartPolling", 5);
        
    }

    async void StartPolling()
    {
        while (!isReady)
        {
            await Task.Delay(3000);

            StartCoroutine(MakeRequest());
        }
    }

    IEnumerator MakeRequest()
    {
        using UnityWebRequest getTeamsRequest = UnityWebRequest.Get(url + gameCode);
        yield return getTeamsRequest.SendWebRequest();
        if (getTeamsRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Couldn't fetch teams: " + getTeamsRequest.error);
        }
        else
        {
            string text = getTeamsRequest.downloadHandler.text;
            int count = JsonConvert.DeserializeAnonymousType(text, new {count = 0}).count;
            if (!isReady && count == 2)
            {
                isReady = true;
                // Go to new scene to start the game
                //SceneManager.LoadScene("name");
            }
        }
    }
}
