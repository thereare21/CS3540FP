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

    
    // Start is called before the first frame update
    void Start()
    {
        Object.DontDestroyOnLoad(transform.gameObject);
        timePlayed = 0f;
        currentLevel = 1;
        currentPanel = startPanel;
    }

    // Update is called once per frame
    void Update()
    {
        timePlayed += Time.deltaTime;
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
    }

    public void ShowCredits()
    {
        //show panel for credits, hide other panel
    }

    public void ShowMenu()
    {
        //show main menu
    }

    public static void updateSceneIndex()
    {
        currentLevel++;
    }
}
