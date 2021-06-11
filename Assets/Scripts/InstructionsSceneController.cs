using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionsSceneController : MonoBehaviour
{

    public void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
    }
    
}
