using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SceneBText : MonoBehaviour
{
    public TextMeshProUGUI instructions;
    public float timerMax;
    private float timer;

    private void Start()
    {
        instructions = GetComponent<TextMeshProUGUI>();
        instructions.text = (SceneManager.GetActiveScene().name + "\n Hold the Mouse Button down for \n"+timerMax+"\n seconds to continue.");
        timer = timerMax;
        
    }
    private void Update()
    {
        if (Input.GetMouseButton(0) && timer > 0.1f)
        {
            timer -= (Time.deltaTime);
        }
        else if(timer > 0.1f)
        {
            timer = timerMax;
        }
        else
        {
            timer = 0f;
            LoadNextScene();
        }
        instructions.text = (SceneManager.GetActiveScene().name + "\n Hold the Mouse Button down for \n" + timer + "\n seconds to continue.");
    }

    public void LoadNextScene()
    {
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        int nextScene = (currentScene + 1) % SceneManager.sceneCountInBuildSettings;
        SceneManager.LoadScene(nextScene);
    }
}
