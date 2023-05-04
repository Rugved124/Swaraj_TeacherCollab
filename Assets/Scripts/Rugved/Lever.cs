using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractiveObject
{   public DrawBridge bridge;
	// Start is called before the first frame update
	private bool _activated = false;

	public override void OnHitByArrow()
	{
		base.OnHitByArrow();

		Debug.Log("Arrow hit the lever!");

		//Trigger the door or something here
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		Arrow arrow = collision.GetComponent<Arrow>();
		if (arrow != null && !_activated)
		{
			// Arrow hit the lever for the first time
			_activated = true;

			// Trigger the door to open
			bridge.OpenDrawBridge();
			Debug.Log("Khul Ja Sim Sim Sim");
		}
	}
}
