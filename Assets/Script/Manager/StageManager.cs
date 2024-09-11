using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager _instance;
    private StageData currentStageData;

    public GameObject heroPrefab;
    public GameObject enemyPrefab;
    public GameObject bgPrefab;

    [HideInInspector] public Hero mainPlayer;
    private List<Hero> ally = new List<Hero>();

    private void Awake()
    {
        _instance = this;
    }

    public void CreateStage(int id)
    {
        currentStageData = GameDataManager._instance.stageData.GetSafe(id);
        if (currentStageData == null)
        {
            Debug.LogError("StageData not found id:" + id);
            return;
        }

        CreateBG();
        SpawnMainPlayer();
        SpawnHeroFirstTime();
        SpawnEnemyFirstTime();
    }

    public Grid RandomAvailableGrid()
    {
        List<Grid> availableGrids = new List<Grid>();

        foreach (var gridPair in GridManager._instance.gridContainer)
        {
            Grid grid = gridPair.Value;
            if (!grid.IsOccupied())
                availableGrids.Add(grid);
        }
        if (availableGrids.Count > 0)
        {
            int randomIndex = Random.Range(0, availableGrids.Count);
            return availableGrids[randomIndex];
        }
        return null;
    }

    public void CreateBG()
    {
        GridData gridData = GameDataManager._instance.gridData.GetSafe(1);
        if (gridData == null)
        {
            Debug.LogError("GridData not found");
            return;
        }

        float totalGridWidth = gridData.totalGridX * gridData.gridSizePixels;
        float totalGridHeight = gridData.totalGridY * gridData.gridSizePixels;
        float centerX = totalGridWidth / 2f;
        float centerY = totalGridHeight / 2f;

        GameObject bg = Instantiate(bgPrefab);
        bg.transform.position = new Vector3(centerX, centerY, 10); 

        SpriteRenderer bgSpriteRenderer = bg.GetComponent<SpriteRenderer>();
        if (bgSpriteRenderer != null && bgSpriteRenderer.drawMode == SpriteDrawMode.Tiled)     
            bgSpriteRenderer.size = new Vector2(totalGridWidth, totalGridHeight);

    }

    public void SpawnMainPlayer()
    {
        Grid randomGrid = RandomAvailableGrid();
        if (randomGrid != null)
        {
            Vector3 spawnPosition = randomGrid.GetWorldPosition();
            GameObject playerObject = Instantiate(heroPrefab, spawnPosition, Quaternion.identity);
            mainPlayer = playerObject.GetComponent<Hero>();
            mainPlayer.Initialize(randomGrid,UnitType.ALLY);

            CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
            if (cameraFollow != null)
                cameraFollow.target = playerObject.transform;

            ally.Add(mainPlayer);
        }
    }

    public void SpawnNewHero()
    {
        Grid randomGrid = RandomAvailableGrid();
        if (randomGrid != null)
        {
            Vector3 spawnPosition = randomGrid.GetWorldPosition();
            GameObject collectibleHero = Instantiate(heroPrefab, spawnPosition, Quaternion.identity);
            collectibleHero.GetComponent<Hero>().Initialize(randomGrid, UnitType.HERO);
        }
    }

    public void SpawnHeroFirstTime()
    {
        for (int i = 0; i < currentStageData.startHeroNumber; i++)
        {
            Grid randomGrid = RandomAvailableGrid();
            if (randomGrid != null)
            {
                Vector3 spawnPosition = randomGrid.GetWorldPosition();
                GameObject collectibleHero = Instantiate(heroPrefab, spawnPosition, Quaternion.identity);
                collectibleHero.GetComponent<Hero>().Initialize(randomGrid, UnitType.HERO);
            }
        }
    }

    public void SpawnNewEnemy()
    {
        Grid randomGrid = RandomAvailableGrid();
        if (randomGrid != null)
        {
            Vector3 spawnPosition = randomGrid.GetWorldPosition();
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.GetComponent<Enemy>().Initialize(randomGrid, UnitType.ENEMY);
        }
    }
    public void SpawnEnemyFirstTime()
    {
        for (int i = 0; i < currentStageData.startEnemyNumber; i++)
        {
            Grid randomGrid = RandomAvailableGrid();
            if (randomGrid != null)
            {
                Vector3 spawnPosition = randomGrid.GetWorldPosition();
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
                enemy.GetComponent<Enemy>().Initialize(randomGrid, UnitType.ENEMY);
            }
        }
    }



    public void OnMainPlayerMove(int gridX, int gridY)
    {
        Grid newGrid = GridManager._instance.GetGrid(gridX, gridY);
        if (newGrid == null)
            return;
        //same grid with enemy
        List<Unit> unitInGrid = new List<Unit>(newGrid.unitInGrid);
        foreach (var unit in unitInGrid)
        {
            if (unit is Enemy enemy && unit.unitType == UnitType.ENEMY)
            {

                mainPlayer.GetAttack(enemy.GetStat(UnitStat.attack).ToInit());
                enemy.GetAttack(mainPlayer.GetStat(UnitStat.attack).ToInit());
                return;
            }
        }


        //move main player
        bool successMove = mainPlayer.Move(gridX, gridY);
        if (!successMove)
            return;
        
        //same grid with ally
        foreach (var unit in newGrid.unitInGrid)
        {
            if (unit != mainPlayer && unit is Hero   && unit.unitType == UnitType.ALLY)
            {
                MainManager._instance.GameOver();
                return;
            }
        }

        //move  ally
        for (int i = 0; i < ally.Count; i++)
        {
            var unit = ally[i];
            if (unit == mainPlayer)
                continue;
            Grid target = ally[i - 1].previousGrid;
            unit.Move(target.gridPosX,target.gridPosY,true);
        }

        //same grid with hero
        Hero heroToChange = null;
        foreach (var unit in newGrid.unitInGrid)
        {
            if (unit is Hero hero && hero.unitType == UnitType.HERO)
            {
                heroToChange = hero;
                heroToChange.ChangeToAlly();
                break;
            }
        }
        if (heroToChange != null)
        {
            Hero lastAlly = ally[ally.Count - 1];
            heroToChange.ForceMove(lastAlly.previousGrid.gridPosX, lastAlly.previousGrid.gridPosY, 1 + ally.Count);
            ally.Add(heroToChange);
        }
    }

    public void SetNewMainPlayer()
    {
        ally.Remove(mainPlayer);
        if (ally.Count == 0)
        {
            MainManager._instance.GameOver();
            return;
        }
        mainPlayer = ally[0];

        CameraFollow cameraFollow = Camera.main.GetComponent<CameraFollow>();
        if (cameraFollow != null)
            cameraFollow.target = mainPlayer.transform;
    }
}
