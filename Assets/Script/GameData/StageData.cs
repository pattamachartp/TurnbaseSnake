using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class StageData 
{
    public int id;
    public int startHeroNumber;
    public int startEnemyNumber;
    public Vector2 heroStartHp;
    public Vector2 heroStartAttack;
    public Vector2 heroStartDefend;
    public Vector2 enemyStartHp;
    public Vector2 enemyStartAttack;
    public Vector2 enemyStartDefend;
}
