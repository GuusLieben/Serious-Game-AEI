using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
