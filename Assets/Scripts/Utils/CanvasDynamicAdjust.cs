using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(CanvasScaler))]
public class CanvasDynamicAdjust : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float ratio = Screen.width * 1f / Screen.height;//4:3 match width, 19:9 match height, 16:9 0.5        
        if (ratio > 16 / 9f)
        {
            GetComponent<CanvasScaler>().matchWidthOrHeight = 1;
        }
        else
        {
            GetComponent<CanvasScaler>().matchWidthOrHeight = Mathf.Clamp((ratio - 4f / 3) / (19f / 9 - 4f / 3), 0f, 1f);
        }
    }
}
