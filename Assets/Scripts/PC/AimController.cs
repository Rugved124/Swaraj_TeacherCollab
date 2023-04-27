using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AimController : MonoBehaviour
{
	[SerializeField] private Transform origin;
	[SerializeField] private float maxInitialVelocity = 11.5f;
	[SerializeField] private float minInitialVelocity = 6f;
	[SerializeField] private float arrowGravity = -9.81f;
	[SerializeField] private float deadZone = 0.05f;
	[SerializeField] private int smoothFrames = 40;
	[SerializeField] private LayerMask collisionLayerMask;
	[SerializeField] private LineRenderer curveRendererPreHit, curveRendererPostHit;
	[SerializeField] private float curveTimeStep = 0.025f;

	public bool ReadyToShoot { get; private set; }
	public float Alpha { get; private set; }
	public float V0 { get; private set; }
	public float G { get; private set; }

	PCController pcController;
	InputManager inputManager;
	Queue<Vector2> smoothedInputs;
	Camera cam;

	public void Init(PCController pc, InputManager inputMngr)
	{
		pcController = pc;
		inputManager = inputMngr;

		Activate(false);
	}

	public void ResetAiming()
	{
		smoothedInputs = new Queue<Vector2>(smoothFrames);
		ReadyToShoot = false;
	}

	public void Activate(bool mustBeActive)
	{
		gameObject.SetActive(mustBeActive);
	}

	// Start is called before the first frame update
	void Start()
	{
		if (smoothFrames < 1) smoothFrames = 1;

		cam = Camera.main;

		ResetAiming();
	}

	// Update is called once per frame
	void Update()
	{
		Vector2 inputs = GetAxesInputs();

		if (Mathf.Abs(inputs.x) > deadZone || Mathf.Abs(inputs.y) > deadZone)
		{
			Alpha = Mathf.Atan2(inputs.y, inputs.x);
			float inputMag = Mathf.Clamp01(inputs.magnitude);
			V0 = inputMag / (1f - deadZone) * (maxInitialVelocity - minInitialVelocity) + minInitialVelocity;
			G = arrowGravity;

			Vector3[] curvePreHit, curvePostHit;
			(curvePreHit, curvePostHit) = GetBallisticCurves(V0, Alpha, G);
			ReadyToShoot = true;

			curveRendererPreHit.enabled = true;
			curveRendererPreHit.positionCount = curvePreHit.Length;
			curveRendererPreHit.SetPositions(curvePreHit);
			curveRendererPostHit.enabled = true;
			curveRendererPostHit.positionCount = curvePostHit.Length;
			curveRendererPostHit.SetPositions(curvePostHit);
		}
		else
		{
			ReadyToShoot = false;
			curveRendererPreHit.enabled = false;
			curveRendererPostHit.enabled = false;
		}
	}

	Vector2 GetAxesInputs()
	{
		smoothedInputs.Enqueue(new Vector2(inputManager.GetHorizontalInput(), inputManager.GetVerticalInput()));
		if (smoothedInputs.Count > smoothFrames)
		{
			smoothedInputs.Dequeue();
		}

		Vector2 ret = new Vector2();
		foreach (Vector2 v in smoothedInputs)
		{
			ret += v;
		}
		ret /= smoothedInputs.Count;
		return ret;
	}

	Rect GetCamBoundaries()
	{
		Vector2 bottomLeft = cam.ViewportToWorldPoint(Vector2.zero);
		Vector2 topRight = cam.ViewportToWorldPoint(Vector2.one);

		return new Rect(bottomLeft, topRight - bottomLeft);
	}

	(Vector3[], Vector3[]) GetBallisticCurves(float v0, float alpha, float g)
	{
		List<Vector3> curvePointsPreHit = new List<Vector3>();
		List<Vector3> curvePointsPostHit = new List<Vector3>();

		bool hasHit = false;

		Rect camBoundaries = GetCamBoundaries();
		float x = camBoundaries.center.x, y = camBoundaries.center.y;

		for (float t = 0f; !(y < camBoundaries.yMin || x < camBoundaries.xMin || x > camBoundaries.xMax); t += curveTimeStep)
		{
			x = v0 * Mathf.Cos(alpha) * t + origin.position.x;
			y = 0.5f * g * t * t + v0 * Mathf.Sin(alpha) * t + origin.position.y;
			Vector2 point = new Vector2(x, y);

			if (!hasHit)
			{
				Collider2D coll = Physics2D.OverlapPoint(point, collisionLayerMask);
				if (coll != null)
				{
					curvePointsPostHit.Add(point);
					hasHit = true;
				}
				curvePointsPreHit.Add(point);
			}
			else
			{
				curvePointsPostHit.Add(point);
			}
		}

		return (curvePointsPreHit.ToArray(), curvePointsPostHit.ToArray());
	}

}
