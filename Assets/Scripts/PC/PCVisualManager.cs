using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.IK;

public class PCVisualManager : MonoBehaviour
{
	public enum AnimState
	{
		Idle = 0,
		Walk = 1,
		Run = 2,
		Aim = 3,
		Jump = 4,
		Crouch = 5,
		Death = 6,
		CrouchAim = 7,
		Shoot = 8,
		Climbing = 9,
		Airborne = 10
	}

	[SerializeField] private Animator animator;
	[SerializeField] private LimbSolver2D LArmIKSolver;
	[SerializeField] private Transform aimPivot, aimTarget;
	private Transform defaultLArmTarget;

	private void Awake()
	{
		defaultLArmTarget = LArmIKSolver.GetChain(0).target;
	}

	public void UpdateAnimState(AnimState newState)
	{
		animator.SetInteger("State", (int)newState);
	}

	public void Aim(float angle)
	{
		aimTarget.position = aimPivot.position + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * 1.5f;
	}

	internal void InitAiming()
	{
		LArmIKSolver.GetChain(0).target = aimTarget;
	}

	internal void EndAiming()
	{
		LArmIKSolver.GetChain(0).target = defaultLArmTarget;
	}
}
