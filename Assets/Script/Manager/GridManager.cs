using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager _instance;

    public Dictionary<Vector2Int, Grid> gridContainer = new Dictionary<Vector2Int, Grid>();

    private void Awake()
    {
        _instance = this;
    }

    public void CreateGrid()
    {
        GridData data = GameDataManager._instance.gridData.GetSafe(1);
        if (data == null)
        {
            Debug.LogError("GridData not found");
            return;
        }

        float halfGridSize = data.gridSizePixels / 2f; 

        for (int x = 0; x < data.totalGridX; x++)
        {
            for (int y = 0; y < data.totalGridY; y++)
            {
                float worldPosX = (x * data.gridSizePixels) + halfGridSize;
                float worldPosY = (y * data.gridSizePixels) + halfGridSize;

                Grid newGrid = new Grid(x, y, worldPosX, worldPosY, data.gridSizePixels);

                Vector2Int gridPos = new Vector2Int(x, y);
                gridContainer.SetSafe(gridPos, newGrid);

                Debug.DrawLine(new Vector3(x * data.gridSizePixels, y * data.gridSizePixels, 0),
                               new Vector3(x * data.gridSizePixels, (y + 1) * data.gridSizePixels, 0), Color.white, 100f);
                Debug.DrawLine(new Vector3(x * data.gridSizePixels, y * data.gridSizePixels, 0),
                               new Vector3((x + 1) * data.gridSizePixels, y * data.gridSizePixels, 0), Color.white, 100f);
            }
        }
    }

    public Vector3 GetGridWorldPosition(int gridX, int gridY)
    {
        Vector2Int gridKey = new Vector2Int(gridX, gridY);
        if (gridContainer.ContainsKey(gridKey))
        {
            Grid grid = gridContainer[gridKey];
            return new Vector3(grid.worldPosX, grid.worldPosY, 0); 
        }
        return Vector3.zero;
    }

    public Grid GetGrid(int x, int y)
    {
        Vector2Int gridKey = new Vector2Int(x, y);
        if (gridContainer.ContainsKey(gridKey))
        {
            return gridContainer[gridKey];
        }
        return null;
    }
}

public class Grid
{
    public int gridPosX { get; private set; }
    public int gridPosY { get; private set; }
    public float worldPosX { get; private set; }
    public float worldPosY { get; private set; }
    public float gridSizePixels { get; private set; }

    public List<Unit> unitInGrid = new List<Unit>();

    public Grid(int x, int y, float worldPosX, float worldPosY, float gridSizePixels)
    {
        gridPosX = x;
        gridPosY = y;
        this.worldPosX = worldPosX;
        this.worldPosY = worldPosY;
        this.gridSizePixels = gridSizePixels;
    }

    public Vector3 GetWorldPosition()
    {
        return new Vector3(worldPosX, worldPosY, 0); // Already centered in world space
    }

    public bool IsOccupied()
    {
        return unitInGrid.Count > 0;
    }
}
