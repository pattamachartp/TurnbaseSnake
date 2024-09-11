using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetMananger : MonoBehaviour
{
    public static AssetMananger _instance;
    [Header("Unit Flash Effects")]
    public Material normalMaterial;
    public Material flashMaterial;

    public void Awake()
    {
        _instance = this;
    }

    public void Init()
    {

    }
}