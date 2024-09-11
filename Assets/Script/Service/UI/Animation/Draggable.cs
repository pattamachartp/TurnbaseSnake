using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class Draggable : MonoBehaviour, IDragHandler
{
    private RectTransform rectTransform;

    private float maxX;
    private float maxY;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
      //  Debug.LogError("OnDrag" +eventData.delta);
        rectTransform.anchoredPosition += eventData.delta;

        if (rectTransform.localPosition.x > maxX)
            rectTransform.localPosition = new Vector3(maxX, rectTransform.localPosition.y);
        if (rectTransform.localPosition.x < -maxX)
            rectTransform.localPosition = new Vector3(-maxX, rectTransform.localPosition.y);


        if (rectTransform.localPosition.y > maxY)
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x,maxY);
        if (rectTransform.localPosition.y < -maxY)
            rectTransform.localPosition = new Vector3(rectTransform.localPosition.x, -maxY);
      //  Debug.LogError("OnDrag2");
    }

    public void SetDragMaxRange(float x ,float y)
    {
        maxX = y;
        maxY = x;
    }
}
