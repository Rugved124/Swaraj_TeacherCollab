using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCCollisionManager : MonoBehaviour
{
	[SerializeField] private Transform sensorBottomLeft, sensorBottomRight;
	[SerializeField] private LayerMask collisionLayerMask;

	public bool IsGrounded { get; private set; }
    public bool ladderCollision { get; private set; }

    public Vector3 ladderPosition { get; private set; }

    void Update()
    {
		if (Physics2D.OverlapPoint(sensorBottomLeft.position , collisionLayerMask)
			|| Physics2D.OverlapPoint(sensorBottomRight.position , collisionLayerMask))
		{
			IsGrounded = true;
		}
		else
		{
			IsGrounded = false;
		}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
		if (LayerMask.LayerToName(collision.gameObject.layer) == "Ladder")
		{
            ladderCollision = true;
            ladderPosition = collision.gameObject.transform.position;
		}
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == "Ladder")
        {
            ladderCollision = false;
            ladderPosition = Vector3.zero;
        }

    }
}
