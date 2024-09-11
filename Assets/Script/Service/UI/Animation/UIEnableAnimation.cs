using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEnableAnimation : MonoBehaviour
{
    [SerializeField] UIEnableAnimationType UIAnimationType;
    [Header("Scale")]
    public Vector3 ScaleFrom;
    public Vector3 postionTo;
    [Header("Move")]
    public Vector3 From;
    public Vector3 To;

    void OnEnable()
    {
        if (UIAnimationType == UIEnableAnimationType.FADE_IN)
        {

        }
        else if (UIAnimationType == UIEnableAnimationType.SCALE_UP)
        { 
        
        }
        else if (UIAnimationType == UIEnableAnimationType.MOVE)
        {

        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
[Serializable]
public enum UIEnableAnimationType
{
    FADE_IN,
    SCALE_UP,
    MOVE
}