using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{

    public void LoadNext()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1).buildIndex);
    }

    public bool hasNextLevel()
    {
        try
        {
            return SceneManager.GetSceneByBuildIndex(SceneManager.GetActiveScene().buildIndex + 1) != null;
        }
        catch (System.Exception)
        {

            return false;
        }
    }
}
