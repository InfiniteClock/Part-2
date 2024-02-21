using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public TextMeshProUGUI highscore;
    public TextMeshProUGUI difficulty;
    public int mode = 1;                // 1 is easy, 2 is med, 3 is hard

    private void Start()
    {
        highscore.text = PlayerPrefs.GetInt("highscore", 0).ToString();
        DifficultyChange(PlayerPrefs.GetInt("difficulty"));
    }
    public void NewGame()
    {
        PlayerPrefs.SetInt("score", 0);
        PlayerPrefs.SetFloat("health", 0);
        SceneManager.LoadScene(5);
    }
    public void ResumeGame()
    {
        if (PlayerPrefs.GetFloat("health") > 0f)
        {
            SceneManager.LoadScene(5);
        }
        else
        {
            NewGame();
        }
    }
    public void DifficultyChange(int value)
    {
        mode = value;
        
        if (mode == 3) 
        {
            difficulty.text = "Difficulty: Hard";
            PlayerPrefs.SetInt("difficulty", 3);
        }
        else if (mode == 2)
        {
            difficulty.text = "Difficulty: Medium";
            PlayerPrefs.SetInt("difficulty", 2);
        }
        else
        {
            mode = 1;
            difficulty.text = "Difficulty: Easy";
            PlayerPrefs.SetInt("difficulty", 1);
        }
    }
}
