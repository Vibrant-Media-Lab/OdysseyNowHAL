using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibrationScreenOverlay : MonoBehaviour
{

    public SpriteRenderer pSpriteRenderer;

    public void SetSpriteOpacity(float o)
    {
        var color = pSpriteRenderer.color;
        color.a = o;
        pSpriteRenderer.color = color;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
