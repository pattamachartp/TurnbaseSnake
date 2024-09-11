using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class GridDataEditor : MonoBehaviour
{
    public List<GridData> gridDataList;

    private void Start()
    {
        foreach (var data in gridDataList)
        {
            GameDataManager._instance.gridData.Add(data.id, data);
        }
    }
}
