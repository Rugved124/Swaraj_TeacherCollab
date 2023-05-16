using UnityEngine;

/// <summary>
/// Base class for objects that can be interacted with (destroyed or switched) by Arrows
/// </summary>
public abstract class InteractiveObject : MonoBehaviour
{
	public abstract void OnHitByArrow();
}