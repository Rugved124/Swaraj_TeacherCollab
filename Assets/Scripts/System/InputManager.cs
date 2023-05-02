using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	[SerializeField] private float walkRunThreshold = 0.9f;
	
	float horizontalInputValue, verticalInputValue;
	bool jumpInput;
	bool canWalk;
	bool canJump;
	bool canRun;
	bool aimInput;
	private bool canAim;

	void Update()
	{
		horizontalInputValue = Input.GetAxis("Horizontal");
		verticalInputValue = Input.GetAxis("Vertical");
		jumpInput = Input.GetButtonDown("Jump");
		aimInput = Input.GetButton("Fire1");

		CheckJumpInput();
		CheckHorizontalInput();
		CheckShootInput();
	}

	void CheckHorizontalInput()
	{
		if (horizontalInputValue != 0f)
		{
			if (Mathf.Abs(horizontalInputValue) >= walkRunThreshold)
			{
				canRun = true;
				canWalk = false;
			}

			else if (Mathf.Abs(horizontalInputValue) < walkRunThreshold)
			{
				canRun = false;
				canWalk = true;
			}
		}
	}

	public void DisableInput()
	{
		horizontalInputValue = 0f;
		verticalInputValue = 0f;
        jumpInput = false;
        canWalk = false;
        canJump = false;
        canRun = false;
        aimInput = false;
		this.enabled = false;
    }

	public void EnableInput()
	{
		this.enabled = true;
	}

	void CheckJumpInput()
	{
		if (jumpInput)
		{
			canJump = true;
		}

		else if (!jumpInput)
		{
			canJump = false;
		}
	}

	public void CheckShootInput()
	{
		if (aimInput)
		{
			canAim = true;
		}

		if (!aimInput)
		{
			canAim = false;
		}
	}

	public float GetHorizontalInput()
	{
		return horizontalInputValue;
	}

	public float GetVerticalInput()
	{
		return verticalInputValue;
	}

	public bool IsWalking()
	{
		return canWalk;
	}

	public bool IsJumping()
	{
		return canJump;
	}

	public bool IsRunning()
	{
		return canRun;
	}

	public bool IsAiming()
	{
		return canAim;
	}

	internal bool IsCrouch()
	{
		
	}
}
