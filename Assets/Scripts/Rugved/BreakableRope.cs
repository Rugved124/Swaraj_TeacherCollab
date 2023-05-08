using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableRope : InteractiveObject
{
    [SerializeField]
    HingeJoint2D joint2D;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	public override void OnHitByArrow()
	{
        print("Hit by Arrow!");
        joint2D.breakForce = 0;


	}

}
