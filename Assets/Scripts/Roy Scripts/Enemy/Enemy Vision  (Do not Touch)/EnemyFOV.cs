using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class EnemyFOV : MonoBehaviour
{
    float visionAngle = 25f;
    float visionDistance = 10f;

    float halfAngle;
    int raycastCount = 10;
    public LayerMask obstructionLayers;

    public bool isSeeing;
    public bool sawPlayer;
    bool isHitting;

    List<RaycastHit2D> hitList;

    public int facingDirection;

    // Start is called before the first frame update
    void Start()
    {
        halfAngle = visionAngle / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        isSeeing = EnemySight(transform.right, visionDistance, halfAngle, raycastCount,obstructionLayers, out hitList, out bool[] hitSomethingArr);
    }

    public void VisionInit(float visionEnemyAngle,float visionEnemyDistance, int raycastEnemyCount, int facingDir)
    {
        visionAngle = visionEnemyAngle;
        visionDistance = visionEnemyDistance;
        raycastCount = raycastEnemyCount;
        SetFacingDirection(facingDir);
    }

    public void SetFacingDirection(int facingDir)
    {
        facingDirection = facingDir;
    }

    public bool EnemySight(Vector2 direction, float distance, float halfAngle, int numRays, LayerMask layerMasks, out List<RaycastHit2D> hitList, out bool[] hitSomethingArr)
    {
        float angleStep = (halfAngle * 2) / (numRays - 1);
        float currentAngle = -halfAngle;

        hitList = new List<RaycastHit2D>();
        hitSomethingArr = new bool[numRays]; // changed from raycastCount to numRays

        bool didHit = false;

        for (int i = 0; i < numRays; i++)
        {
            Vector2 rotatedDirection = RotateVector2(direction, currentAngle);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rotatedDirection, distance, layerMasks);

            if (hit.collider != null)
            {
                didHit = true;
                hitList.Add(hit);
                hitSomethingArr[i] = true;

                if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "PC")
                {
                    if (!sawPlayer)
                    {
                        sawPlayer = true;
                    }

                }
                else
                {
                    sawPlayer = false;
                }
            }
            else
            {
                hitSomethingArr[i] = false;

            }

            currentAngle += angleStep;
        }

        return didHit;
    }

    Vector2 RotateVector2(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        float x = (cos * vector.x) - (sin * vector.y);
        float y = (sin * vector.x) + (cos * vector.y);
        return new Vector2(x, y);
    }


    void OnDrawGizmos()
    {
        Vector2 direction = transform.right;

        List<RaycastHit2D> hitList;
        bool[] hitSomethingArr;
        bool hitSomething = EnemySight(direction, visionDistance, halfAngle, raycastCount, obstructionLayers, out hitList, out hitSomethingArr);

        float angleStep = (halfAngle * 2) / (raycastCount - 1);

        for (int i = 0; i < raycastCount; i++)
        {
            Vector2 rotatedDirection = RotateVector2(direction, -halfAngle + i * angleStep);

            if (hitList.Count > 0 && i < hitList.Count && hitList[i].collider != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, hitList[i].point);
            }
            else
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, transform.position + (Vector3)rotatedDirection * visionDistance);
            }
        }
    }
}
