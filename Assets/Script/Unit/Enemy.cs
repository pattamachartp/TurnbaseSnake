using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit
{
    public override void Die()
    {
        base.Die();
        StageManager._instance.SpawnNewEnemy();
    }

}