using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotingPlatform : InteractiveObject
{
	public override void OnHitByArrow()
	{
		Debug.Log("It Broke!");
	}
}
