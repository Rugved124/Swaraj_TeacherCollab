using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertMeter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetScaleX(float alertFillAmount)
    {
        Vector3 newScale = transform.localScale;
        newScale.x = alertFillAmount;
        transform.localScale = newScale;
    }
}
