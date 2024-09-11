using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainManager : MonoBehaviour
{
    public static MainManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        InitMainGame();
    }

    public void InitMainGame()
    {
        GridManager._instance.CreateGrid();
        StageManager._instance.CreateStage(1);
    }


    public void GameOver()
    {
        Debug.Log("--------------GameOver--------------");
        UIManager._instance.gameOverPanel.OpenPanel();
    }

    public void Restart()
    {
        // for fast 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}