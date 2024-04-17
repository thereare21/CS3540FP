using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuScript : MonoBehaviour
{

    public static float timePlayed;

    //we will keep track of the level we stopped at as the player completes each level
    public static int currentLevel;

    public GameObject startPanel;
    public GameObject currentPanel;

    public GameObject exitButton;

    public TMPro.TextMeshProUGUI timePlayedText;

    public static float mouseSensitivity;

    private void Awake()
    {
        timePlayed = 0f;
    }


    // Start is called before the first frame update
    void Start()
    {
        Object.DontDestroyOnLoad(transform.gameObject);
        currentLevel = 1;
        currentPanel = startPanel;
        
    }

    // Update is called once per frame
    void Update()
    {
        timePlayed += Time.deltaTime;
        timePlayedText.text = "Time Played: " + (int)Time.realtimeSinceStartup;

        Debug.Log(Cursor.lockState);
    }

    public static void LoadNextSceneFromStart()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void ShowPanel(GameObject newPanel)
    {
        //Debug.Log("ran");
        //show panel, hide other panel
        currentPanel.SetActive(false);
        newPanel.SetActive(true);
        currentPanel = newPanel;
        exitButton.SetActive(true);
    }

    public void GoToMenu()
    {
        ShowPanel(startPanel);
        exitButton.SetActive(false);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void SetMouseSensitivity(float value)
    {
        mouseSensitivity = value;
    }

    

    public static void updateSceneIndex()
    {
        currentLevel++;
    }
}
