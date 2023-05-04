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

    public bool Alerted()
    {
        if (transform.localScale.x <= 0f)
        {
            return false;
        }
        else if (transform.localScale.x > 0f && transform.localScale.x < 1f)
        {
            return true;
        }

        return false;
    }

    public bool Alarmed()
    {
        if (transform.localScale.x == 1f)
        {
            return true;
        }

        return false;
    }
}
