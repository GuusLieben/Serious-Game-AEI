using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuSceneController : MonoBehaviour
{

    public void GoToGameRulesScene()
    {
        SceneManager.LoadScene("GameRulesScene");
    }

}
