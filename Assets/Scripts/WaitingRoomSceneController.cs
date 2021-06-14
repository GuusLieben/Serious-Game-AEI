using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitingRoomSceneController : MonoBehaviour
{
    private string _gameCode;
    private bool _isReady;
    
    [SerializeField] private string url = "https://avans-schalm-appserver.azurewebsites.net/api/game/teams/count?gameCode={0}";
    [SerializeField] private Text title;

    private void Start()
    {
        _isReady = false;
        // Development only, will later be populated by other scene(s)
        // PlayerPrefs.SetString("GAME_CODE", "TFUrCM");
        _gameCode = PlayerPrefs.GetString("GAME_CODE");
        Invoke(nameof(StartPolling), 5);
        title.text = _gameCode;
        Debug.Log("WaitingRoom started");
    }

    private async void StartPolling()
    {
        while (!_isReady)
        {
            await Task.Delay(3000);
            StartCoroutine(MakeRequest());
        }
    }

    private IEnumerator MakeRequest()
    {
        using UnityWebRequest getTeamsRequest = UnityWebRequest.Get(string.Format(url, _gameCode));
        yield return getTeamsRequest.SendWebRequest();
        if (getTeamsRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Couldn't fetch teams: " + getTeamsRequest.error);
        }
        else
        {
            var text = getTeamsRequest.downloadHandler.text;
            var count = JsonConvert.DeserializeAnonymousType(text, new {count = 0}).count;
            if (!_isReady && count == 2)
            {
                _isReady = true;
                // Go to new scene to start the game
                SceneManager.LoadScene("InstructionsScene");
            }
        }
    }
}