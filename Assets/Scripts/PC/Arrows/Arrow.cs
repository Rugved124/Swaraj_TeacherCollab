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

	//static public Action<Arrow> OnArrowHit;

	public ArrowType type;
	public GameObject ArrowBlood;

	private Rigidbody2D rb;
	[SerializeField] private SpriteRenderer spriteRenderer;
	bool isAlive;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
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
		float deathDuration = 1.5f;

		if (otherObjColl.TryGetComponent(out BaseEnemy enemy))
		{
			Instantiate(ArrowBlood, transform.position, Quaternion.identity);
			enemy.OnHitByArrow(this);
			deathDuration = 0f;
		}
		else if (otherObjColl.TryGetComponent(out InteractiveObject obj))
		{
			obj.OnHitByArrow();
			transform.parent = otherObjColl.transform;
			deathDuration = 0f;
		}
		else
		{
			rb.gravityScale = 0f;
			rb.velocity = Vector3.zero;
			rb.angularVelocity = 0f;
		}

		isAlive = false;
		//OnArrowHit?.Invoke(this);
		StartCoroutine(coDying(deathDuration));
	}

	private IEnumerator coDying(float duration)
	{
		float startTime = Time.time;
		float endTime = startTime + duration;
		do
		{
			float alpha = 1f - Mathf.InverseLerp(startTime, endTime, duration);
			spriteRenderer.color = new Color(1f, 1f, 1f, alpha);
			yield return null;
		}
		while (Time.time < endTime);
		Destroy(gameObject);
	}

}
