using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    public void ReloadLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
