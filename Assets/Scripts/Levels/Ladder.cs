using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
	private PCCollisionManager pcCollisionManager;

	private void Start()
	{
		pcCollisionManager = FindObjectOfType<PCCollisionManager>();
	}

	private void OnTriggerEnter2D(Collider2D otherCollider)
	{
		pcCollisionManager.OnLadderEnter(transform.position);
	}

	private void OnTriggerExit2D(Collider2D otherCollider)
	{
		pcCollisionManager.OnLadderExit();
	}
}
