using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationController : MonoBehaviour
{
    public void GoToScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}
