using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class BreakableRope : InteractiveObject
{
    [SerializeField]
    HingeJoint2D joint2D;
	[SerializeField]
	private float enemyKillForce = 10f;

	public LayerMask Enemies;

	private Collider2D platformCollider;


	// Start is called before the first frame update
	void Start()
    {
		platformCollider = GetComponent<Collider2D>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }


	public override void OnHitByArrow()
	{
        print("Hit by Arrow!");
        joint2D.breakForce = 0;

		// Disable the platform collider to allow the platform to fall through other colliders
		platformCollider.enabled = false;
	}


	private void OnTriggerEnter2D(Collider2D other)
	{
		// Check if the collider is an enemy and apply force to kill it
		if (IsEnemyLayer(other.gameObject.layer))
		{
			Rigidbody2D enemyRigidbody = other.GetComponent<Rigidbody2D>();
			if (enemyRigidbody != null)
			{
				Vector2 forceDirection = (enemyRigidbody.transform.position - transform.position).normalized;
				enemyRigidbody.AddForce(forceDirection * enemyKillForce, ForceMode2D.Impulse);
				enemyRigidbody.gameObject.SetActive(false);
			}
		}
	}

	private bool IsEnemyLayer(int layer)
	{
		string layerName = LayerMask.LayerToName(layer);
		return layerName.Contains("Enemy");
	}

}
