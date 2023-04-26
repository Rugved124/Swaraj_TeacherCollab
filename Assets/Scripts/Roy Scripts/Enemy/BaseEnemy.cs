using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public FiniteStateMachine enemyFSM;

    public CharacterData characterData;

    public int facingDirection;

    protected Rigidbody2D enemyRb;

    public Animator enemyAnim;

    protected SpriteRenderer enemySpriteRenderer;

    private Vector2 updatedVelocity;

    public bool isWayPointBased;

    [SerializeField]
    private Transform wallCheck;

    [SerializeField]
    private Transform ledgeCheck;



    public virtual void Start()
    {
        facingDirection = 1;
        enemySpriteRenderer = transform.Find("Visuals").GetComponent<SpriteRenderer>();
        enemyAnim = transform.Find("Visuals").GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody2D>();

        enemyFSM = new FiniteStateMachine();
    }

    public virtual void Update()
    {
        enemyFSM.currentState.UpdateState();
    }

    public virtual void SetVelocity(float velocity)
    {
        updatedVelocity.Set(facingDirection * velocity, enemyRb.velocity.y);
        enemyRb.velocity = updatedVelocity;
    }

    public virtual bool CheckWall()
    {
        return Physics2D.Raycast(wallCheck.position, transform.right, characterData.wallCheckDistance, characterData.whatIsGround);
    }

    public virtual bool CheckGround()
    {
        return Physics2D.Raycast(ledgeCheck.position, Vector2.down, characterData.ledgeCheckDistance, characterData.whatIsGround);
    }

    public virtual void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * characterData.wallCheckDistance));
        Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * characterData.ledgeCheckDistance));
    }
}
