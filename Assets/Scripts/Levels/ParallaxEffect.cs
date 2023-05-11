using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
	[SerializeField] Vector3 previousPosition;
	[SerializeField] float parallaxMultiplier = 0.5f;

	// Update is called once per frame
	void Update()
	{
		Vector3 currentPosition = Camera.main.transform.position;

		if (previousPosition != Vector3.zero)
		{
			Vector3 distanceTravelled = currentPosition - previousPosition;
			transform.Translate(distanceTravelled * parallaxMultiplier);
		}

		previousPosition = currentPosition;
	}
}
