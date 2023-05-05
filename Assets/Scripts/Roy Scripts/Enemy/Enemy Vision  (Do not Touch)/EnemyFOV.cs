using System.Collections.Generic;
using UnityEngine;

public class EnemyFOV : MonoBehaviour
{
    float visionAngle = 25f;
    float visionDistance = 10f;

    public float halfAngle;
    int raycastCount = 10;
    public LayerMask obstructionLayers;

    public bool isSeeing;
    public bool sawPlayer;
    bool isHitting;

    List<RaycastHit2D> hitList;

    public int facingDirection;

    Vector3 playerPos;

    // Start is called before the first frame update
    void Start()
    {
        halfAngle = visionAngle / 2f;
    }

    // Update is called once per frame
    void Update()
    {
        isSeeing = EnemySight(transform.right, visionDistance, halfAngle, raycastCount,obstructionLayers, out hitList, out bool[] hitSomethingArr, transform.parent.rotation);
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


    Vector2 RotateVector2(Vector2 vector, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);
        float x = (cos * vector.x) - (sin * vector.y);
        float y = (sin * vector.x) + (cos * vector.y);
        return new Vector2(x, y);
    }


    
    public bool EnemySight(Vector2 direction, float distance, float halfAngle, int numRays, LayerMask layerMasks, out List<RaycastHit2D> hitList, out bool[] hitSomethingArr, Quaternion rotation)
    {
        float angleStep = (halfAngle * 2) / (numRays - 1);

        float currentAngle = -halfAngle;

        hitList = new List<RaycastHit2D>();
        hitSomethingArr = new bool[numRays]; // changed from raycastCount to numRays

        bool didHit = false;

        sawPlayer = false;

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
                    playerPos = hit.collider.gameObject.transform.position;

                    sawPlayer = true;

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

    public void LookAtPlayer()
    {
        Vector3 direction = playerPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = targetRotation;
       

    }

    void OnDrawGizmos()
    {
        Vector2 direction = transform.right;

        List<RaycastHit2D> hitList;
        bool[] hitSomethingArr;
        bool hitSomething = EnemySight(direction, visionDistance, halfAngle, raycastCount, obstructionLayers, out hitList, out hitSomethingArr, transform.parent.rotation);

        float angleStep = (halfAngle * 2) / (raycastCount - 1);

        // Adjust starting angle based on parent rotation
        float currentAngle;
        
        if (transform.parent.eulerAngles.y == 0)
        {
            currentAngle = -halfAngle;
        }
        else
        {
            currentAngle = halfAngle;
        }

        for (int i = 0; i < raycastCount; i++)
        {
            Vector2 rotatedDirection = RotateVector2(direction, currentAngle);
            Vector2 worldDirection = transform.TransformDirection(rotatedDirection); // Transform direction to world space

            if (hitList.Count > 0 && i < hitList.Count && hitList[i].collider != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(transform.position, hitList[i].point);
            }
                

            currentAngle += angleStep;
        }
    }


}
