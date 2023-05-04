using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBridge : InteractiveObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDrawBridge() 
    {
    }

    public override void OnHitByArrow()
    {
        base.OnHitByArrow();
		Debug.Log("Khul Ja Sim Sim");

	}
}
