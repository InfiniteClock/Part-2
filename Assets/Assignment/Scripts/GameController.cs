using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject player;
    public GameObject[] Obstacles;
    public TextMeshProUGUI highScore;
    public TextMeshProUGUI currentScore;

    public int score;
    public int record;

    public float spawnSpeed;
    private float timer;

    private void Start()
    {
        Instantiate(player);
        record = PlayerPrefs.GetInt("highscore", 0);
        score = PlayerPrefs.GetInt("score", 0);
        highScore.text = record.ToString();
        currentScore.text = score.ToString();
        if (PlayerPrefs.GetInt("difficulty") == 3)
        {
            spawnSpeed = 0.5f;
        }
        else if (PlayerPrefs.GetInt("difficulty") == 2)
        {
            spawnSpeed = 1;
        }
        else
        {
            spawnSpeed = 2;
        }

    }
    private void Update()
    {
        score = PlayerPrefs.GetInt("score");
        if (timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = spawnSpeed;
            Instantiate(Obstacles[Random.Range(0, Obstacles.Length)], new Vector3(-10,-10,0), transform.rotation);
        }
        currentScore.text = score.ToString();

        if (score > record)
        {
            record = score;
            highScore.text = record.ToString();
            PlayerPrefs.SetInt("highscore", record);
        }
    }
    public void Pause()
    {
        if (PlayerPrefs.GetFloat("health") > 0f)
        {
            SceneManager.LoadScene(4);
        }
    }
}
