using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField inputField;
    private string playerName;
    // Start is called before the first frame update
    void Start()
    {
        if (playerName != null)
        {
            inputField.text = playerName;
        }
    }

    public void SaveUserName()
    {
        playerName = inputField.text;
    }

    public string GetPlayerName()
    {
        return playerName;
    }
}
