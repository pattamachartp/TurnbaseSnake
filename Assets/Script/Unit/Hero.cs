using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Unit
{
    public override void Die()
    {       
        base.Die();
        StageManager._instance.SpawnNewHero();
    }

    public void ChangeToAlly()
    {
        unitType = UnitType.ALLY;
    }
}