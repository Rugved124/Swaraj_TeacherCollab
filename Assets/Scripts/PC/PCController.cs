using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PCController : MonoBehaviour
{
	public enum State
	{
		Idle,
		Walk,
		Run,
		Jump,
		Airborne,
		CrouchIdle,
		CrouchWalk,
		Aiming,
		Death
	}

	public State currentState;
	public bool isOnPlatform = true;

	[SerializeField] float jumpForce = 15f;
	[SerializeField] float walkSpeed = 5f, runSpeed = 10f, crouchWalkSpeed = 3f;
	[SerializeField] private float pauseTimeAfterShoot = 0.25f;

	private InputManager inputManager;
	private BottomDeathLine bottomDeathLine;
	private Rigidbody2D rb;
	private AimController aimController;
	private ShootManager shootManager;
	private PCCollisionManager collisionManager;
	private float maxSpeedX;
	private float dirX;
	private Vector2 moveVector;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>(); //because the Rigidbody2D is already on the same GameObject which is the PC
		aimController = GetComponentInChildren<AimController>();
		shootManager = GetComponentInChildren<ShootManager>();
		collisionManager = GetComponent<PCCollisionManager>();
	}

	void Start()
	{
		inputManager = FindObjectOfType<InputManager>();
		bottomDeathLine = FindObjectOfType<BottomDeathLine>();
		aimController.Init(this, inputManager);
		ChangeState(State.Idle);
	}

	void Update()
	{
		if (currentState == State.Death)
			return;

		if (transform.position.y < bottomDeathLine.transform.position.y)
		{
			ChangeState(State.Death);
			return;
		}

		if (inputManager.IsAiming() && collisionManager.IsGrounded)
		{
			if (currentState != State.Aiming)
				ChangeState(State.Aiming);
			return;
		}

		if (inputManager.IsJumping() && collisionManager.IsGrounded)
		{
			if (currentState != State.Jump)
			{
				ChangeState(State.Jump);
			}
		}

		if (currentState == State.Jump)
		{
			ChangeState(State.Airborne);
		}
		else if (currentState == State.Airborne)
		{
			if (collisionManager.IsGrounded)
			{
				ChangeState(State.Idle);
			}
			else if (inputManager.IsWalking())
			{
				HorizontalMove();
			}
		}
		else
		{
			if (inputManager.IsWalking() && !inputManager.IsRunning())
			{
				if (currentState != State.Walk)
				{
					ChangeState(State.Walk);
				}
				HorizontalMove();
			}
			else if (!inputManager.IsWalking() && inputManager.IsRunning())
			{
				if (currentState != State.Run)
				{
					ChangeState(State.Run);
				}
				HorizontalMove();
			}
			else if (!inputManager.IsWalking() && !inputManager.IsJumping() && !inputManager.IsRunning())
			{
				if (currentState != State.Idle)
				{
					ChangeState(State.Idle);
				}
				HorizontalMove();
			}
		}
	}

	private void HorizontalMove()
	{
		dirX = inputManager.GetHorizontalInput();
		moveVector = new Vector2(dirX * maxSpeedX, rb.velocity.y);
		rb.velocity = moveVector;

		if (dirX > 0)
		{
			transform.rotation = Quaternion.Euler(0, 0, 0);
		}
		else if (dirX < 0)
		{
			transform.rotation = Quaternion.Euler(0, 180, 0);
		}
	}

	void ChangeState(State newState)
	{
		switch (newState)
		{
			case State.Idle:
				currentState = State.Idle;
				maxSpeedX = 0f;
				break;

			case State.Walk:
				currentState = State.Walk;
				maxSpeedX = walkSpeed;
				break;

			case State.Run:
				currentState = State.Run;
				maxSpeedX = runSpeed;
				break;

			case State.Jump:
				currentState = State.Jump;
				maxSpeedX = runSpeed;
				Jump();
				break;

			case State.Airborne:
				currentState = State.Airborne;
				break;

			case State.Aiming:
				currentState = State.Aiming;
				StartAiming();
				break;

			case State.Death:
				currentState = State.Death;
				Destroy(gameObject);
				Debug.Log("Dead");
				break;
		}
	}

	//bool IsGrounded()
	//{
	//	float extraDepth = 0.5f;
	//	RaycastHit2D groundHit = Physics2D.BoxCast(pcCollider.bounds.center, pcCollider.bounds.size, 0f, Vector2.down, extraDepth, platformLayer);

	//	Color rayColor;
	//	if (groundHit.collider != null)
	//	{
	//		rayColor = Color.green;
	//	}
	//	else
	//	{
	//		rayColor = Color.red;
	//	}

	//	Debug.DrawRay(pcCollider.bounds.center + new Vector3(pcCollider.bounds.extents.x, 0f), Vector2.down * (pcCollider.bounds.extents.y + extraDepth), rayColor);
	//	Debug.DrawRay(pcCollider.bounds.center - new Vector3(pcCollider.bounds.extents.x, 0f), Vector2.down * (pcCollider.bounds.extents.y + extraDepth), rayColor);
	//	Debug.DrawRay(pcCollider.bounds.center - new Vector3(pcCollider.bounds.extents.x, pcCollider.bounds.extents.y), Vector2.right * (pcCollider.bounds.extents.x), rayColor);
	//	return groundHit.collider != null;
	//}

	void Jump()
	{
		rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
	}

	void StartAiming()
	{
		rb.velocity = Vector3.zero;
		StartCoroutine(coAimingAndShooting());
	}

	private IEnumerator coAimingAndShooting()
	{
		aimController.Activate(true);
		aimController.ResetAiming();
		//Aiming
		do
		{
			yield return null;
		} while (inputManager.IsAiming());

		//Shooting or cancel
		if (aimController.ReadyToShoot)
		{
			shootManager.Shoot(aimController.Alpha, aimController.V0, aimController.G);
		}

		aimController.Activate(false);
		yield return new WaitForSeconds(pauseTimeAfterShoot);
		ChangeState(State.Idle);
	}
}
