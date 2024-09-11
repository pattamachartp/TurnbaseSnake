using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DPadManager : MonoBehaviour
{
    private Gamepad gamepad;

    private void Update()
    {

        gamepad = Gamepad.current;
        if (gamepad == null)
            return;

        if (gamepad.dpad.up.wasPressedThisFrame)
        {
            OnDPadPressed(Direction.UP);
        }
        else if (gamepad.dpad.down.wasPressedThisFrame)
        {
            OnDPadPressed(Direction.DOWN);
        }
        else if (gamepad.dpad.left.wasPressedThisFrame)
        {
            OnDPadPressed(Direction.LEFT);
        }
        else if (gamepad.dpad.right.wasPressedThisFrame)
        {
            OnDPadPressed(Direction.RIGHT);
        }
    }

    private void OnDPadPressed(Direction direction)
    {
        Hero mainPlayer = StageManager._instance.mainPlayer;

        int currentX = mainPlayer.currentGrid.gridPosX;
        int currentY = mainPlayer.currentGrid.gridPosY;

        switch (direction)
        {
            case Direction.UP:
                currentY += 1;
                break;
            case Direction.DOWN:
                currentY -= 1;
                break;
            case Direction.LEFT:
                currentX -= 1;
                break;
            case Direction.RIGHT:
                currentX += 1;
                break;
        }
        StageManager._instance.OnMainPlayerMove(currentX, currentY);
    }
}
