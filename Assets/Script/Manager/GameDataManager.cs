using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager : MonoBehaviour
{
    public static GameDataManager _instance;

    public Dictionary<int, StageData> stageData = new Dictionary<int, StageData>();
    public Dictionary<int, GridData> gridData = new Dictionary<int, GridData>();

    public void Awake()
    {
        _instance = this;
    }

    public void Init()
    {

    }
}
