using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

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
		CrouchAiming,
		Death
	}

	[SerializeField] float jumpForce = 15f;
	[SerializeField] float walkSpeed = 5f, runSpeed = 10f, airborneSpeed = 6.5f, climbingSpeed = 5f;
	[SerializeField] private float pauseTimeAfterShoot = 0.25f;
	[SerializeField] private float jumpMulitiplierForLadder = 1 / 3f;
	[SerializeField] GameObject bloodVFX;

	private InputManager inputManager;
	private BottomDeathLine bottomDeathLine;
	private Rigidbody2D rb;
	private AimController aimController;
	private ShootManager shootManager;
	private PCCollisionManager collisionManager;
	private PCVisualManager visualManager;

	private State currentState;
	private State previousState;
	private float maxSpeedX;
	private float dirX;
	private float dirY;
	private Vector2 moveVector;
	private bool isAlreadyAiming;
	private bool canBeGrounded, canGoClimbing;

	[Header("Audio")]
	[SerializeField] AudioSource bowDrawSfx;
	[SerializeField] AudioSource jumpSfx, runSfx;
	

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>(); //because the Rigidbody2D is already on the same GameObject which is the PC
		aimController = GetComponentInChildren<AimController>();
		shootManager = GetComponentInChildren<ShootManager>();
		collisionManager = GetComponent<PCCollisionManager>();
		visualManager = GetComponent<PCVisualManager>();
	}

	void Start()
	{
		inputManager = FindObjectOfType<InputManager>();
		bottomDeathLine = FindObjectOfType<BottomDeathLine>();
		aimController.Init(this, inputManager);
		UpdateState(State.Idle);
		isAlreadyAiming = false;
		canBeGrounded = true;
		canGoClimbing = true;
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

	public void Die()
	{
		if (currentState != State.Death)
			UpdateState(State.Death);

		Instantiate(bloodVFX, transform.position, Quaternion.identity);
	}

	void Update()
	{
		if (currentState == State.Death) return;

		if (transform.position.y < bottomDeathLine.transform.position.y)
		{
			Die();
			return;
		}

		if (canGoClimbing && collisionManager.LadderCollision && (
			(currentState == State.Idle || currentState == State.Crouch || currentState == State.Walk || currentState == State.Run) && inputManager.IsVerticalInput()
			|| currentState == State.Airborne)
			)
		{
			UpdateState(State.Climbing);
		}

		if ((currentState == State.Idle || currentState == State.Run || currentState == State.Walk) && !collisionManager.IsGrounded)
			UpdateState(State.Airborne);

		if (collisionManager.IsGrounded && inputManager.IsCrouchInput)
		{
			if (currentState == State.Crouch)
			{
				UpdateState(State.Idle);
			}
			else if (currentState == State.CrouchAiming)
			{
				UpdateState(State.Aiming);
			}
			else if (currentState == State.Aiming)
			{
				UpdateState(State.CrouchAiming);
			}
			else
			{
				UpdateState(State.Crouch);
			}
		}

		if ((currentState == State.Aiming || currentState == State.CrouchAiming) && collisionManager.IsGrounded) return;

		if (inputManager.IsAiming() && collisionManager.IsGrounded)
		{
			UpdateState(currentState == State.Crouch ? State.CrouchAiming : State.Aiming);
			return;
		}

		if (inputManager.IsJumping() && collisionManager.IsGrounded)
		{
			UpdateState(State.Jump);
			UpdateState(State.Airborne);
			return;
		}

		if (currentState == State.Airborne)
		{
			if (collisionManager.IsGrounded && canBeGrounded)
			{
				UpdateState(State.Idle);
			}
			else
			{
				HorizontalMove();
			}

			if (collisionManager.LadderCollision && collisionManager.IsGrounded)
			{
				UpdateState(State.Climbing);
			}
		}
		else
		{
			if (currentState == State.Climbing)
			{
				DoWhileClimbing();
				return;
			}
			else
			{
				if (currentState != State.Crouch)
				{
					if (inputManager.IsWalking())
					{
						UpdateState(State.Walk);
					}
					else if (inputManager.IsRunning())
					{
						UpdateState(State.Run);
					}
					else if (!inputManager.IsJumping())
					{
						UpdateState(State.Idle);
					}
				}
				HorizontalMove();
			}
		}
	}

	void UpdateState(State newState)
	{
		previousState = currentState;
		if (newState == currentState)
			return;

		switch (newState)
		{
			case State.Idle:
				currentState = State.Idle;
				maxSpeedX = 0f;
				rb.gravityScale = 1f;
				visualManager.UpdateAnimState(PCVisualManager.AnimState.Idle);
				break;

			case State.Walk:
				currentState = State.Walk;
				maxSpeedX = walkSpeed;
				rb.gravityScale = 1f;
				visualManager.UpdateAnimState(PCVisualManager.AnimState.Walk);
				break;

			case State.Run:
				currentState = State.Run;
				maxSpeedX = runSpeed;
				rb.gravityScale = 1f;
				visualManager.UpdateAnimState(PCVisualManager.AnimState.Run);
				break;

			case State.Jump:
				currentState = State.Jump;
				maxSpeedX = airborneSpeed;
				rb.gravityScale = 1f;
				visualManager.UpdateAnimState(PCVisualManager.AnimState.Jump);
				Jump(jumpForce * (previousState == State.Climbing ? jumpMulitiplierForLadder : 1f));
				canBeGrounded = false;
				StartCoroutine(CoNoGroundedOnJump());
				jumpSfx.Play();
				break;

			case State.Airborne:
				currentState = State.Airborne;
				maxSpeedX = airborneSpeed;
				rb.gravityScale = 1f;
				if (previousState != State.Jump) visualManager.UpdateAnimState(PCVisualManager.AnimState.Airborne);
				break;

			case State.Crouch:
				currentState = State.Crouch;
				maxSpeedX = 0f;
				rb.gravityScale = 1f;
				visualManager.UpdateAnimState(PCVisualManager.AnimState.Crouch);
				break;

			case State.Climbing:
				currentState = State.Climbing;
				maxSpeedX = 0f;
				rb.gravityScale = 0f;
				visualManager.UpdateAnimState(PCVisualManager.AnimState.Climbing);
				StartClimbing();
				break;

			case State.Aiming:
				currentState = State.Aiming;
				maxSpeedX = 0f;
				rb.gravityScale = 1f;
				visualManager.UpdateAnimState(PCVisualManager.AnimState.Aim);
				StartAiming();
				bowDrawSfx.Play();
				break;

			case State.CrouchAiming:
				currentState = State.CrouchAiming;
				maxSpeedX = 0f;
				rb.gravityScale = 1f;
				visualManager.UpdateAnimState(PCVisualManager.AnimState.CrouchAim);
				StartAiming();
				bowDrawSfx.Play();
				break;

			case State.Death:
				currentState = State.Death;
				maxSpeedX = 0f;
				rb.gravityScale = 1f;
				visualManager.UpdateAnimState(PCVisualManager.AnimState.Death);
				StartDeath();
				break;
		}
	}
	private void DoWhileClimbing()
	{
		if (collisionManager.IsGrounded && (inputManager.GetVerticalInput() < 0f))
		{
			UpdateState(State.Idle);
			return;
		}
		else if (!collisionManager.LadderCollision || inputManager.IsJumping())
		{
			dirX = inputManager.GetHorizontalInput();
			if (dirX > 0f) FaceDirection(true);
			else if (dirX < 0f) FaceDirection(false);

			UpdateState(State.Jump);
			canGoClimbing = false;
			StartCoroutine(CoNoClimbingOnJumpOffLadder());
			UpdateState(State.Airborne);
			return;
		}

		VerticalMove();
	}

	private void VerticalMove()
	{
		dirY = inputManager.GetVerticalInput();
		moveVector = new Vector2(0f, dirY * climbingSpeed);
		rb.velocity = moveVector;
	}

	private void HorizontalMove()
	{
		dirX = inputManager.GetHorizontalInput();
		moveVector = new Vector2(dirX * maxSpeedX, rb.velocity.y);
		rb.velocity = moveVector;

		if (dirX > 0f) FaceDirection(true);
		else if (dirX < 0f) FaceDirection(false);
	}

	private void FaceDirection(bool isFacingRight)
	{
		transform.rotation = Quaternion.Euler(0f, isFacingRight ? 0f : 180f, 0f);
	}

	void Jump(float jumpHeight)
	{
		rb.AddForce(Vector3.up * jumpHeight, ForceMode2D.Impulse);
	}

	void StartAiming()
	{
		if (isAlreadyAiming) return;

		rb.velocity = Vector3.zero;
		StartCoroutine(coAimingAndShooting());
	}

	private IEnumerator coAimingAndShooting()
	{
		isAlreadyAiming = true;
		aimController.Activate(true);
		aimController.ResetAiming();
		visualManager.InitAiming();
		//Aiming
		do
		{
			FaceDirection(!(Mathf.Abs(aimController.Alpha) > Mathf.PI * 0.5f));
			visualManager.Aim(aimController.Alpha);
			yield return null;
		} while (inputManager.IsAiming());

		//Shooting or cancel
		if (aimController.ReadyToShoot)
		{
			shootManager.Shoot(aimController.Alpha, aimController.V0, aimController.G);
			visualManager.UpdateAnimState(PCVisualManager.AnimState.Shoot);
			aimController.Activate(false);
			yield return new WaitForSeconds(pauseTimeAfterShoot);
		}
		else
		{
			aimController.Activate(false);
		}
		visualManager.EndAiming();

		isAlreadyAiming = false;
		UpdateState(State.Idle);
	}

	void StartClimbing()
	{
		rb.position = new Vector3(collisionManager.LadderPosition.x, rb.position.y);
	}

	void StartDeath()
	{
		StartCoroutine(CoDeath());
	}

	private IEnumerator CoDeath()
	{
		float endTime = Time.time + 1.5f;
		do
		{
			print("coDeath with state " + currentState);//TEST
			yield return null;
		}
		while (Time.time < endTime);

		FindObjectOfType<GameManager>().GameOver();
	}

	private IEnumerator CoNoGroundedOnJump()
	{
		yield return new WaitForSeconds(0.1f);
		canBeGrounded = true;
	}

	private IEnumerator CoNoClimbingOnJumpOffLadder()
	{
		yield return new WaitForSeconds(0.4f);
		canGoClimbing = true;
	}

	public void PlayStepSFX()
	{
		runSfx.Play();
	}
}
