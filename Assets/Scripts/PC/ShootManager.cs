using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootManager : MonoBehaviour
{
	[SerializeField] Arrow normalArrowPrefab, blindingArrowPrefab, paranoidArrowPrefab;

	internal void Shoot(float alpha, float v0, float g)
	{
		Arrow arrow = Instantiate(GetArrowPrefab(), transform.position, Quaternion.Euler(0f, 0f, alpha * Mathf.Rad2Deg));
		arrow.Init(v0, g);
	}

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}

	private Arrow GetArrowPrefab()
	{
		switch (GetCurrentArrowType())
		{
			case Arrow.ArrowType.Blinding: return blindingArrowPrefab;
			case Arrow.ArrowType.Paranoid: return paranoidArrowPrefab;
			default: return normalArrowPrefab;
		}
	}

	private Arrow.ArrowType GetCurrentArrowType()
	{
		return Arrow.ArrowType.Normal;
	}
}
