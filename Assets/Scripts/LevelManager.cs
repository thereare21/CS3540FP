using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Text gameText;
    public static bool isGameOver;
    public string nextLevel;
    public string currentLevel;
    void Start()
    {
        isGameOver = false;
        
    }

    public void LevelLost() {
        isGameOver = true;
        gameText.text = "GAME OVER!";
        gameText.gameObject.SetActive(true);

        if (!string.IsNullOrEmpty(nextLevel)) {
            Invoke("LoadLevel", 2);
        }
    }

    public void LevelBeat() {
        isGameOver = true;
        gameText.text = "YOU WIN!";
        gameText.gameObject.SetActive(true);
 
        if (!String.Equals(currentLevel, "Level3Prototype")) {
            Invoke("LoadNextLevel", 2);
            isGameOver = false;
            MenuScript.currentLevel++;
        }
        

    }

    void LoadLevel() {
        SceneManager.LoadScene(currentLevel);
    }

    void LoadNextLevel() {
        SceneManager.LoadScene(nextLevel);
    }
}
