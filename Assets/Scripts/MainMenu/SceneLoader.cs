using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public void LoadSceneByIndex(int index )
    {
        if(index >= 0 && index < SceneManager.sceneCountInBuildSettings)
        {
        SceneManager.LoadScene(index);
        }
        else
        {
           Debug.LogError("Scene index out of range");
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
