using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	[SerializeField] private float walkRunThreshold = 0.9f;
	
	float horizontalInputValue, verticalInputValue;
	bool jumpInput, canWalk, canJump, canRun, canClimb;
	bool aimInput;
	private bool canAim;

	public bool IsCrouchInput { get; private set; }

	void Update()
	{
		horizontalInputValue = Input.GetAxis("Horizontal");
		verticalInputValue = Input.GetAxis("Vertical");
		jumpInput = Input.GetButtonDown("Jump");
		aimInput = Input.GetButton("Fire1");
		IsCrouchInput = Input.GetButtonDown("Crouch");

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
		else
		{
			canRun = false;
			canWalk = false;
		}
	}

    public bool IsVerticalInput()
    {
        if (verticalInputValue != 0f)
        {
            canClimb = true;
        }
        else
        {
			canClimb = false;
        }

		return canClimb;
    }

    public void DisableInput()
	{
		horizontalInputValue = 0f;
		verticalInputValue = 0f;
        jumpInput = false;
        canWalk = false;
        canJump = false;
        canRun = false;
		canClimb = false;
        aimInput = false;
		IsCrouchInput = false;
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
}
