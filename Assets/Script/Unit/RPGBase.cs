using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGBase : MonoBehaviour
{
    [Header("Base Stat")]
    [HideInInspector] public int level;
    [HideInInspector] public int curHp;
    protected SerializableDictionary<UnitStat, float> stats = new SerializableDictionary<UnitStat, float>();
    protected SerializableDictionary<UnitStat, float> addStats = new SerializableDictionary<UnitStat, float>();

    [HideInInspector] public UnitClass unitClass;

    public void Start()
    {
  
    }

    public void InitStat(SerializableDictionary<UnitStat, float> stats)
    {
        this.stats = stats;
        curHp = (int)GetStat(UnitStat.maxHp);
    }

    public float GetStat(UnitStat stat)
    {
        float getStat = stats.ContainsKey(stat) ? stats[stat] : 0;
        if (addStats.ContainsKey(stat)) getStat += addStats[stat];
      //  if (mutiStats.ContainsKey(stat)) getStat *= mutiStats[stat];
        return getStat;
    }
}



[Serializable]
public class StartUnitStat
{
    public UnitStat stat;
    public float value;
}


[Serializable]
public enum UnitStat
{
    maxHp,
    attack,
    defend
}

[Serializable]
public enum UnitClass
{
    Warrior,
    Rogue,
    Wizard
}


