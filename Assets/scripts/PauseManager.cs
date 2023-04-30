using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{

    public GameObject pauseMenu;

    private void Awake()
    {
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.P))
        {
            bool shouldPause = Time.timeScale != 0.0f;
            Time.timeScale = shouldPause ? 0.0f : 1f;
            pauseMenu.SetActive(shouldPause);
            Cursor.lockState = shouldPause ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.visible = shouldPause;
        }
    }
}
