using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class JudgingSceneController : MonoBehaviour
{

    private readonly List<string> _clicked = new List<string>();
    [SerializeField] private string url = "https://avans-schalm-appserver.azurewebsites.net/api/game/score?gameCode={0}";

    [SerializeField] private TMP_Text buttonA;
    [SerializeField] private TMP_Text buttonB;
    [SerializeField] private TMP_Text buttonC;

    private void Start()
    {
        SetValues(new []{
            "Test", "Kaas", "Kees"
        });
}

    public void OnClicked(Button button)
    {
        if (!_clicked.Contains(button.name))
        {
            button.image.color = new Color(0.9294118F, 0.145098F, 0.3254902F);
            _clicked.Add(button.name);
        }
        else
        {
            button.image.color = new Color(0.7058824F, 0.5960785F, 0.6235294F);
            _clicked.Remove(button.name);
        }

        print(button.name);
    }

    public void SetValues(string[] words)
    {
        buttonA.text = words[0];
        buttonB.text = words[1];
        buttonC.text = words[2];
    }

    public void OnSubmit()
    {
        var amount = _clicked.Count;
        var gameCode = PlayerPrefs.GetString("GAME_CODE");
       StartCoroutine( MakeRequest(amount, gameCode));
    }

    private IEnumerator MakeRequest(int amount, string gamecode)
    {
        using UnityWebRequest postScore = UnityWebRequest.Post(string.Format(url, gamecode), amount.ToString());
        postScore.SetRequestHeader("Content-Type", "application/json");
        
        var jsonToSend = new UTF8Encoding().GetBytes(amount.ToString());
        postScore.uploadHandler = new UploadHandlerRaw(jsonToSend);
        postScore.downloadHandler = new DownloadHandlerBuffer();

        yield return postScore.SendWebRequest();

        SceneManager.LoadScene("GameScene");
    }
}
