using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    
    public static int Score {
        get { return score; }
        set {
            score = value;
            TextMeshProUGUI scoreText = GameObject.FindGameObjectWithTag("Canvas").GetComponentInChildren<TextMeshProUGUI>();
            scoreText.text = "Score: " + score;
        }
    }
    private static int score;
    public TextMeshProUGUI scoreUI;

    // Start is called before the first frame update
    void Start()
    {
        Score = 0;
    }

    public static void AddScore(int scoreVal)
    {
        Score += scoreVal;

    }

    public void updateUI()
    {

    }
}
