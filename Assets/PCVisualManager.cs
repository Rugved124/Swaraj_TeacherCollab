using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	}

	[SerializeField] private Animator animator, animatorClimbing;

	// Start is called before the first frame update
	void Start()
    {
        
    }

	public void UpdateAnimState(AnimState newState)
	{
		animator.SetInteger("State", (int)newState);
		animatorClimbing.SetBool("isClimbing", newState == AnimState.Climbing);
	}
}
