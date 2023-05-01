using UnityEngine;

[CreateAssetMenu(fileName = "newWayPointData", menuName = "Data/WayPoint Data/Base Data")]
public class EnemyWaypointsData:ScriptableObject
{
    public Vector3 localPosition;
    public float rotateAngle;
    public float rotateTime;
}
