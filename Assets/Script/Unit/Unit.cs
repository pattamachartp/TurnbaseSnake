using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : RPGBase
{
    public TextMeshProUGUI hpText;
    public TextMeshProUGUI atkText;
    public TextMeshProUGUI defText;

    public UnitType unitType;

    public Grid currentGrid { get; private set; }
    public Grid previousGrid { get; private set; }
    public Direction facingDirection { get; private set; }

    private Vector3 targetPosition;
    private bool isMoving = false;
    public float moveSpeed = 5f;

    private Coroutine flashCoroutine;
    private SpriteRenderer displaySprite;

    public void Initialize(Grid grid, UnitType type)
    {
        displaySprite = transform.Find("display").GetComponent<SpriteRenderer>();

        currentGrid = grid;
        facingDirection = Direction.DOWN;
        unitType = type;
        grid.unitInGrid.Add(this);

        // init stat
        var currentStageData = GameDataManager._instance.stageData.GetSafe(1);
        if (currentStageData == null)
        {
            Debug.LogError("StageData not found Initialize Unit");
            return;
        }
        SerializableDictionary<UnitStat, float> initStats = new SerializableDictionary<UnitStat, float>();
        if (unitType == UnitType.HERO || unitType == UnitType.ALLY)
        {
            initStats.SetSafe(UnitStat.maxHp, Random.Range(currentStageData.heroStartHp.x, currentStageData.heroStartHp.y));
            initStats.SetSafe(UnitStat.attack, Random.Range(currentStageData.heroStartAttack.x, currentStageData.heroStartAttack.y));
            initStats.SetSafe(UnitStat.defend, Random.Range(currentStageData.heroStartDefend.x, currentStageData.heroStartDefend.y));
        }
        else if (unitType == UnitType.ENEMY)
        {
            initStats.SetSafe(UnitStat.maxHp, Random.Range(currentStageData.enemyStartHp.x, currentStageData.enemyStartHp.y));
            initStats.SetSafe(UnitStat.attack, Random.Range(currentStageData.enemyStartAttack.x, currentStageData.enemyStartAttack.y));
            initStats.SetSafe(UnitStat.defend, Random.Range(currentStageData.enemyStartDefend.x, currentStageData.enemyStartDefend.y));
        }
        InitStat(initStats);
        UpdateStatText();
    }

    public void UpdateStatText()
    {
        UpdateHpText();
        atkText.text =  GetStat(UnitStat.attack).ToInit() +"";
        defText.text = GetStat(UnitStat.defend).ToInit() +"";
    }

    public void UpdateHpText()
    {
        hpText.text = curHp + "/" + GetStat(UnitStat.maxHp).ToInit();
    }

    private Direction GetDirectionTo(int x, int y)
    {
        if (x > currentGrid.gridPosX) return Direction.RIGHT;
        if (x < currentGrid.gridPosX) return Direction.LEFT;
        if (y > currentGrid.gridPosY) return Direction.UP;
        if (y < currentGrid.gridPosY) return Direction.DOWN;
        return facingDirection; 
    }

    private bool CanMoveInDirection(Direction moveDirection)
    {
        switch (facingDirection)
        {
            case Direction.UP:
                return moveDirection != Direction.DOWN;
            case Direction.DOWN:
                return moveDirection != Direction.UP;
            case Direction.LEFT:
                return moveDirection != Direction.RIGHT;
            case Direction.RIGHT:
                return moveDirection != Direction.LEFT;
        }
        return true; 
    }

    public bool Move(int gridX, int gridY, bool isIgnoreDirection = false)
    {
        if (isMoving) return false;
        Direction moveDirection = GetDirectionTo(gridX, gridY);
        if (!isIgnoreDirection && !CanMoveInDirection(moveDirection)) return false;
        facingDirection = moveDirection;

        if (currentGrid != null)
        {
            currentGrid.unitInGrid.Remove(this);
        }

        Grid newGrid = GridManager._instance.GetGrid(gridX, gridY);
        if (newGrid != null)
        {
            targetPosition = newGrid.GetWorldPosition();
            newGrid.unitInGrid.Add(this);
            previousGrid = currentGrid;
            currentGrid = newGrid;
            StartCoroutine(SmoothMove(moveSpeed));
        }
        return true;
    }

    public void ForceMove(int gridX, int gridY, float mutiSpeed = 1f)
    {
        if (currentGrid != null)
        {
            currentGrid.unitInGrid.Remove(this);
        }

        Grid newGrid = GridManager._instance.GetGrid(gridX, gridY);
        if (newGrid != null)
        {
            targetPosition = newGrid.GetWorldPosition();
            newGrid.unitInGrid.Add(this);
            previousGrid = currentGrid;
            currentGrid = newGrid;
            StartCoroutine(SmoothMove(moveSpeed* mutiSpeed));
        }
    }

    private IEnumerator SmoothMove(float speed)
    {
        isMoving = true;
        while (Vector3.Distance(transform.position, targetPosition) > 0.05f) 
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null; 
        }
        transform.position = targetPosition;    
        isMoving = false;
    }

    public void GetAttack(int attackVaule)
    {
        int damage = attackVaule - GetStat(UnitStat.defend).ToInit();
        curHp -= damage;
        if (curHp <= 0)
        {
            Die();
        }
        FlashWhenHit();
        UpdateHpText();
    }

    private void FlashWhenHit()
    {
        displaySprite.material = AssetMananger._instance.flashMaterial;
        if (flashCoroutine != null)
            MainService.StopDelegate(flashCoroutine);
        flashCoroutine = MainService.InvokeDelegate(0.1f, () => { 
            if(displaySprite !=null)
               displaySprite.material = AssetMananger._instance.normalMaterial;
        });
    }

    public bool IsDead()
    {
        return curHp <= 0;
    }

    public virtual void Die()
    {
        if (StageManager._instance.mainPlayer == this)
            StageManager._instance.SetNewMainPlayer();
        currentGrid.unitInGrid.Remove(this);
        Destroy(gameObject); 
    }

}

public enum Direction
{ 
  LEFT,
  RIGHT,
  UP,
  DOWN
}

public enum UnitType
{
    ALLY,
    HERO,
    ENEMY
}
