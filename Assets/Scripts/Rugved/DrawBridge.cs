using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBridge : ActivableObject
{
	private Rigidbody2D rb;
	[SerializeField]
	private float platformFallingForce = 5.0f;
	DrawBridge torquePoint;

	// Start is called before the first frame update
	void Start()
    {
		// Get a reference to the Rigidbody2D component
		rb = GetComponent<Rigidbody2D>();

	}

	// Update is called once per frame
	void Update()
    {
        
    }

    public void OpenDrawBridge() 
    {
    }

    public override void Activate()
    {
		Debug.Log("Khul Ja Sim Sim");


		// Calculate a direction for the force
		Vector3 direction;
		if(transform.rotation.eulerAngles.y == 0)
		{
			direction = Vector3.right;
		}
		else
		{
			direction = Vector3.left;
		}

		Vector3 position = torquePoint.transform.position;

		// Calculate the force vector
		Vector3 force = direction * platformFallingForce;

		// Apply the force at the specified position
		rb.AddForceAtPosition(force, position);



	}
}
