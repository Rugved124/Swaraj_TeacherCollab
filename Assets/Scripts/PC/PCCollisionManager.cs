using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCCollisionManager : MonoBehaviour
{
	[SerializeField] private Transform sensorBottomLeft, sensorBottomRight;
	[SerializeField] private LayerMask collisionLayerMask;

	public bool IsGrounded { get; private set; }
    public bool LadderCollision { get; private set; }
    public Vector3 LadderPosition { get; private set; }

	public void OnLadderEnter(Vector3 ladderPosition)
	{
		LadderCollision = true;
		LadderPosition = ladderPosition;
	}

	public void OnLadderExit()
	{
		LadderCollision = false;
		LadderPosition = Vector3.zero;
	}

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

}
