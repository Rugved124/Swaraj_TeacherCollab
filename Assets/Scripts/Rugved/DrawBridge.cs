using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBridge : ActivableObjects
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
        base.Activate();
		Debug.Log("Khul Ja Sim Sim");

		torquePoint.transform.position = new Vector3(0f, 13f, 0f);

		// Calculate a direction for the force
		Vector3 direction = Vector3.right;

		Vector3 position = torquePoint.transform.position;

		// Calculate the force vector
		Vector3 force = direction * platformFallingForce;

		// Apply the force at the specified position
		rb.AddForceAtPosition(force, position);

		// Get the position of the game object
		Vector3 currentPosition = torquePoint.transform.position;
		Debug.Log("The current position of the game object is: " + currentPosition);


	}
}
