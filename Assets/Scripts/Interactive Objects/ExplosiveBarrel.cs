using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : InteractiveObject
{
	public float radius = 10.0f;
	public LayerMask Enemies;

	public GameObject Explosion;
	public GameObject explosionSoundPrefab; // Prefab of the explosion sound AudioSource

	private void Start()
	{
		// Instantiate the explosion sound AudioSource
		GameObject soundObject = Instantiate(explosionSoundPrefab, transform.position, Quaternion.identity);
		AudioSource explosionSound = soundObject.GetComponent<AudioSource>();
		explosionSound.Play();

		// Destroy the explosion sound GameObject after the sound finishes playing
		Destroy(soundObject, explosionSound.clip.length);
	}
	public override void OnHitByArrow()
	{
		base.OnHitByArrow();
		Destroy(gameObject);
		Instantiate(Explosion, transform.position, Quaternion.identity);

		// Check for colliders within the specified circle
		Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);

		

		// Iterate over all colliders within the circle
		foreach (Collider2D collider in colliders)
		{
			// Check if the collider is on the enemy layer
			if (Enemies == (Enemies | (1 << collider.gameObject.layer)))
			{
				// Get the parent GameObject of the collider
				GameObject enemy = collider.gameObject;
				if (enemy.transform.parent != null)
				{
					enemy = enemy.transform.parent.gameObject;
				}


				// Call the OnHitByHazard() function of the BaseEnemy script
				BaseEnemy baseEnemy = enemy.GetComponent<BaseEnemy>();
				if (baseEnemy != null)
				{
					baseEnemy.OnHitByHazard();
				}

				// Destroy the parent GameObject
				Destroy(enemy);
			}
		}

	}
}

