using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Paused : MonoBehaviour
{

    public GameObject pausedPanel;

    bool showPauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        showPauseMenu = false;
        pausedPanel.SetActive(showPauseMenu);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            showPauseMenu = !showPauseMenu;
            pausedPanel.SetActive(showPauseMenu);
            if (showPauseMenu)
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;

            } else
            {
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        if (showPauseMenu)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                SceneManager.LoadScene("MainMenu");
                Time.timeScale = 1f;
                Cursor.lockState = CursorLockMode.None;
            }
        }
    }

    public void HidePauseMenu()
    {
        showPauseMenu = false;
        transform.gameObject.SetActive(showPauseMenu);
    }
}
