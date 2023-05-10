using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKillZone : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D otherCollider)
	{
		print("Die!");
        otherCollider.GetComponent<BaseEnemy>().OnHitByHazard();

	}
}
