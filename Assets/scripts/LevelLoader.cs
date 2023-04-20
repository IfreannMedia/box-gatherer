using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    public void LoadLevel(string name)
    {
        StartCoroutine(LoadAsync(name));
    }

    public void LoadLevel(int i)
    {
        StartCoroutine(LoadAsync(SceneManager.GetSceneAt(i).name));
    }

    IEnumerator LoadAsync(string name)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }


}
