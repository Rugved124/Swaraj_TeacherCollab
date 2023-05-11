using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Arrow : MonoBehaviour
{
	public enum ArrowType
	{
		Normal,
		Blinding,
		Paranoid
	}

	static public Action<Arrow> OnArrowHit;

	public ArrowType type;
	public GameObject ArrowBlood;

	private Rigidbody2D rb;
	private Collider2D coll;
	bool isAlive;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		coll = GetComponent<Collider2D>();
	}

	internal void Init(float v0, float g)
	{
		rb.velocity = transform.TransformDirection(Vector3.right) * v0;
		rb.gravityScale = g / Physics2D.gravity.y;

		isAlive = true;
	}

	void Update()
	{
		if (isAlive)
		{
			Reorient();
		}
	}

	private void Reorient()
	{
		float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
		rb.rotation = angle;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (!isAlive) return;

		Collider2D otherObjColl = collision.collider;

		if (otherObjColl.TryGetComponent(out BaseEnemy enemy))
		{
			Instantiate(ArrowBlood, transform.position, Quaternion.identity);
			enemy.OnHitByArrow(this);
		}
		else if (otherObjColl.TryGetComponent(out InteractiveObject obj))
		{
			obj.OnHitByArrow();
		}
		else
		{
			rb.gravityScale = 0f;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = 0f;
		}

		isAlive = false;
		OnArrowHit?.Invoke(this);
		StartCoroutine(coDying());
	}

	private IEnumerator coDying()
	{
		yield return new WaitForSeconds(1.5f);
		Destroy(gameObject);
	}

	
}
