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
    public Transform stageRef;

    [HideInInspector] public Hero mainPlayer;
    private List<Hero> ally = new List<Hero>();

    [HideInInspector] public List<Hero> heroContainer = new List<Hero>();
    [HideInInspector] public List<Enemy> enemyContainer = new List<Enemy>();

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
            GameObject playerObject = Instantiate(heroPrefab, spawnPosition, Quaternion.identity, stageRef);
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
        StageData stageData = GameDataManager._instance.stageData.GetSafe(1);
        if (Random.Range(0f, 100f) <= stageData.heroSpawnChance || heroContainer.Count < 2)
            {
            int numberToSpawn = Random.Range(1, stageData.maxHeroSpawnCount + 1);
            for (int i = 0; i < numberToSpawn; i++)
            {
                Grid randomGrid = RandomAvailableGrid();
                if (randomGrid != null)
                {
                    Vector3 spawnPosition = randomGrid.GetWorldPosition();
                    GameObject collectibleHero = Instantiate(heroPrefab, spawnPosition, Quaternion.identity, stageRef);
                    Hero hero = collectibleHero.GetComponent<Hero>();
                    hero.Initialize(randomGrid, UnitType.HERO);
                    heroContainer.Add(hero);
                }
            }
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
                GameObject collectibleHero = Instantiate(heroPrefab, spawnPosition, Quaternion.identity, stageRef);
                Hero hero = collectibleHero.GetComponent<Hero>();
                hero.Initialize(randomGrid, UnitType.HERO);
                heroContainer.Add(hero);
            }
        }
    }

    public void SpawnNewEnemy()
    {
        StageData stageData = GameDataManager._instance.stageData.GetSafe(1);
        if (Random.Range(0f, 100f) <= stageData.enemySpawnChance || enemyContainer.Count < 2)
        {
            int numberToSpawn = Random.Range(1, stageData.maxEnemySpawnCount + 1);
            for (int i = 0; i < numberToSpawn; i++)
            {
                Grid randomGrid = RandomAvailableGrid();
                if (randomGrid != null)
                {
                    Vector3 spawnPosition = randomGrid.GetWorldPosition();
                    GameObject enemyObj = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, stageRef);
                    Enemy enemy = enemyObj.GetComponent<Enemy>();
                    enemyObj.GetComponent<Enemy>().Initialize(randomGrid, UnitType.ENEMY);
                    enemyContainer.Add(enemy);
                }
            }
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
                GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, stageRef);
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
                heroContainer.Remove(hero);
                SpawnNewHero();
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
