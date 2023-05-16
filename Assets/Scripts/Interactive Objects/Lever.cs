using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractiveObject
{
	[SerializeField] ActivableObject activableTarget;

	private bool activated = false;

	public override void OnHitByArrow()
	{
		if (activated) return;
		
		activableTarget.Activate();

		activated = true;

		GetComponent<Animator>().SetTrigger("activate");

		GetComponent<AudioSource>().Play();
	}

}
