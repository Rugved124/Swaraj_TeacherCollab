using UnityEngine;

public class BottomDeathLine : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine(new Vector3(transform.position.x - 100f, transform.position.y, transform.position.z),
			new Vector3(transform.position.x + 100f, transform.position.y, transform.position.z));
	}
}
