using System.Collections;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JoinGameController : MonoBehaviour
{
    [SerializeField] private InputField gameCodeInputField;
    [SerializeField] private string url = "https://avans-schalm-appserver.azurewebsites.net/api/game/joinable?gameCode={0}";
    
    public void OnSearchGameClicked()
    {
        if (gameCodeInputField.text.Equals(""))
        {
            Debug.Log("Game code field is empty");
        }
        else
        {
            StartCoroutine(MakeRequest());
        }
    }

    private IEnumerator MakeRequest()
    {
        using UnityWebRequest getIsJoinableRequest = UnityWebRequest.Get(string.Format(url, gameCodeInputField.text));
        yield return getIsJoinableRequest.SendWebRequest();

        if (getIsJoinableRequest.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Couldn't check if game is joinable: " + getIsJoinableRequest.error);
        }
        else
        {
            var text = getIsJoinableRequest.downloadHandler.text; 
            var isJoinable = JsonConvert.DeserializeAnonymousType(text, new {isJoinable = false}).isJoinable;
            if (isJoinable)
            {
                PlayerPrefs.SetString("GAME_CODE", gameCodeInputField.text);
                SceneManager.LoadScene("JoinExistingGameScene");
            }
        }
    }
    
}
