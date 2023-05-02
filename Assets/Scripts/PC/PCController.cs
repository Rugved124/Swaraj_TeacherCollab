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
		Crouch,
		Climbing,
		Aiming,
		Death
	}

	private enum AnimState
	{
		Idle = 0,
		Walk = 1,
		Run = 2,
		Aim = 3
	}

	public State currentState;
	public bool isOnPlatform = true;

	[SerializeField] float jumpForce = 15f;
	[SerializeField] float walkSpeed = 5f, runSpeed = 10f, crouchWalkSpeed = 3f, climbingSpeed = 5f;
	[SerializeField] private float pauseTimeAfterShoot = 0.25f;
	[SerializeField] private Animator animator;

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
		UpdateState(State.Idle);
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
			UpdateState(State.Death);
			return;
		}

		if (inputManager.IsAiming() && collisionManager.IsGrounded)
		{
			UpdateState(State.Aiming);
			return;
		}

		if (inputManager.IsJumping() && collisionManager.IsGrounded)
		{
			UpdateState(State.Jump);
		}

		if (currentState == State.Jump)
		{
			UpdateState(State.Airborne);
		}
		else if (currentState == State.Airborne)
		{
			if (collisionManager.IsGrounded)
			{
				UpdateState(State.Idle);
			}
			else if (inputManager.IsWalking() || inputManager.IsRunning())
			{
				HorizontalMove();
			}
		}
		else
		{
			if (inputManager.IsCrouch() && currentState == State.Crouch)
			{
				DoWhileCrouch();
			}
			else if (currentState == State.Climbing)
			{
				DoWhileClimbing();
			}
			else if (inputManager.IsWalking() && !inputManager.IsRunning())
			{
				UpdateState(State.Walk);
				HorizontalMove();
			}
			else if (!inputManager.IsWalking() && inputManager.IsRunning())
			{
				UpdateState(State.Run);
				HorizontalMove();
			}
			else if (!inputManager.IsWalking() && !inputManager.IsJumping() && !inputManager.IsRunning())
			{
				UpdateState(State.Idle);
				HorizontalMove();
			}
		}
	}

	private void DoWhileClimbing()
	{
		throw new NotImplementedException();
	}

	private void DoWhileCrouch()
	{
		
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

	void UpdateState(State newState)
	{
		if (newState == currentState)
			return;

		switch (newState)
		{
			case State.Idle:
				currentState = State.Idle;
				maxSpeedX = 0f;
				UpdateAnimState(AnimState.Idle);
				break;

			case State.Walk:
				currentState = State.Walk;
				maxSpeedX = walkSpeed;
				UpdateAnimState(AnimState.Walk);
				break;

			case State.Run:
				currentState = State.Run;
				maxSpeedX = runSpeed;
				UpdateAnimState(AnimState.Run);
				break;

			case State.Jump:
				currentState = State.Jump;
				maxSpeedX = runSpeed;
				Jump();
				break;

			case State.Airborne:
				currentState = State.Airborne;
				break;

			case State.Crouch:
				currentState = State.Jump;
				maxSpeedX = 0f;
				break;

			case State.Climbing:
				currentState = State.Jump;
				maxSpeedX = 0f;
				break;

			case State.Aiming:
				currentState = State.Aiming;
				StartAiming();
				UpdateAnimState(AnimState.Aim);
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
		UpdateState(State.Idle);
	}

	private void UpdateAnimState(AnimState newState)
	{
		animator.SetInteger("State", (int)newState);
	}
}
