using UnityEngine;

/// <summary>
/// Base class for objects that can be interacted with (destroyed or switched) by Arrows
/// </summary>
public class InteractiveObject : MonoBehaviour
{
	bool isAlive;

	public virtual void OnHitByArrow()
	{
	
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!isAlive) return;

		Collider2D otherObjColl = collision.collider;

		if (otherObjColl.TryGetComponent(out BaseEnemy enemy))
		{
			//enemy.OnHitByHazards(this);
		}
		
	}

}