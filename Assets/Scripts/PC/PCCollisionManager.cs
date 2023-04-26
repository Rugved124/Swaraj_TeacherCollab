using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCCollisionManager : MonoBehaviour
{
	[SerializeField] private Transform sensorBottomLeft, sensorBottomRight;
	//[SerializeField] private LayerMask collisionLayerMask;

	public bool IsGrounded { get; private set; }

    void Update()
    {
		if (Physics2D.OverlapPoint(sensorBottomLeft.position /*, collisionLayerMask*/)
			|| Physics2D.OverlapPoint(sensorBottomRight.position /*, collisionLayerMask)*/))
		{
			IsGrounded = true;
		}
		else
		{
			IsGrounded = false;
		}
    }
}
