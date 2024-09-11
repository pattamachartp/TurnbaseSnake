using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    public static KeyboardManager _instance;

    private void Awake()
    {
        _instance = this;
    }

    private void Update()
    {
        if (StageManager._instance.mainPlayer == null) return;

        int newX = StageManager._instance.mainPlayer.currentGrid.gridPosX;
        int newY = StageManager._instance.mainPlayer.currentGrid.gridPosY;

        if (Input.GetKeyDown(KeyCode.W))
        {
            newY += 1; // Move up
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            newY -= 1; // Move down
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            newX -= 1; // Move left
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            newX += 1; // Move right
        }

        if (newX != StageManager._instance.mainPlayer.currentGrid.gridPosX|| newY != StageManager._instance.mainPlayer.currentGrid.gridPosY)
        {
            StageManager._instance.OnMainPlayerMove(newX, newY);
        }
    }
}
