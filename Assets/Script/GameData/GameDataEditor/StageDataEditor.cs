using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataEditor : MonoBehaviour
{
    public List<StageData> stageDataList;

    private void Start()
    {
        foreach (var data in stageDataList)
        {
            GameDataManager._instance.stageData.Add(data.id, data);
        }
    }
}
