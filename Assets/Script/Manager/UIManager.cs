using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager _instance;
    public Transform canvas;
    public Transform canvasPopUp;

    [Header("Panel")]
    public GameOverPanel gameOverPanel;

    [Header("PopUp Prefab")]
    public GameObject statPopUp;


    public void Awake()
    {
        _instance = this;
    }
}
