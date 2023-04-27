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

	/// <summary>
	/// Teleports the PC to the indicated location and orients it as ordered. Public method because PCPositioner calls it.
	/// </summary>
	public void InstantReposition(Vector2 pos, bool mustFaceLeft)
	{
		rb.position = pos;
		transform.position = new Vector3(pos.x, pos.y, transform.position.z);

		if (mustFaceLeft) FaceDirection(false);
	}

	void Update()
	{
		if (currentState == State.Death) return;

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

		if (dirX > 0) FaceDirection(true);
		else if (dirX < 0) FaceDirection(false);
	}

	private void FaceDirection(bool isFacingRight)
	{
		transform.rotation = Quaternion.Euler(0, isFacingRight ? 0 : 180, 0);
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
