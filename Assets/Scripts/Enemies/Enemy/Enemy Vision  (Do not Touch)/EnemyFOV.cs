using System.Collections.Generic;
using Unity.VisualScripting;
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
    public bool sawCorpse;
    public bool sawKill;
    bool isHitting;

    List<RaycastHit2D> hitList;

    public int facingDirection;

    public Vector3 playerPos { get; private set; }
    public PCController PC { get; private set; }

    public GameObject visionLinePrefab;

    public Material collidingMaterial;
    public Material nonCollidingMaterial;

    public List<LineRenderer> existingLineRenderer;

    // Start is called before the first frame update
    void Start()
    {
        halfAngle = visionAngle / 2f;

    }

    // Update is called once per frame
    void Update()
    {
        isSeeing = EnemySight(transform.right, visionDistance, halfAngle, raycastCount, obstructionLayers, out hitList, out bool[] hitSomethingArr, transform.parent.rotation);
    }

    public void VisionInit(float visionEnemyAngle, float visionEnemyDistance, int raycastEnemyCount, int facingDir)
    {
        visionAngle = visionEnemyAngle;
        visionDistance = visionEnemyDistance;
        raycastCount = raycastEnemyCount;
        SetFacingDirection(facingDir);
        SpawnLineRenderers(raycastCount);
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
        sawCorpse = false;
        PC = null;

        for (int i = 0; i < numRays; i++)
        {
            Vector2 rotatedDirection = RotateVector2(direction, currentAngle);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rotatedDirection, distance, layerMasks);

            if (hit.collider != null)
            {
                didHit = true;
                hitList.Add(hit);
                hitSomethingArr[i] = true;

                AdjustLineRenderer(i, collidingMaterial, hit.point);

                if ((LayerMask.LayerToName(hit.collider.gameObject.layer) == "PC") && !sawKill)
                {
                    playerPos = hit.collider.gameObject.transform.position;

                    if (PC != null)
                    {
                        PC = hit.collider.gameObject.GetComponent<PCController>();
                    }
                    else
                    {
                        PC = GameObject.FindObjectOfType<PCController>();
                    }

                    sawPlayer = true;

                }

                if (LayerMask.LayerToName(hit.collider.gameObject.layer) == "Corpse")
                {
                    if (hit.collider.gameObject.GetComponent<BaseEnemy>().isDying)
                    {
                        sawKill = true;
                    }
                }


            }
            else
            {
                hitSomethingArr[i] = false;

                AdjustLineRenderer(i, nonCollidingMaterial, transform.position + new Vector3(rotatedDirection.x, rotatedDirection.y, 0f) * distance);

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



    void SpawnLineRenderers(int lineCount)
    {
        for (int i = 0; i < lineCount; i++)
        {
            GameObject lineGO = Instantiate(visionLinePrefab, transform);
            LineRenderer line = lineGO.GetComponent<LineRenderer>();
            line.material = nonCollidingMaterial;
            line.SetPosition(0, transform.position);
            line.SetPosition(1, transform.position + transform.right);
            existingLineRenderer.Add(line);
        }
    }


    void AdjustLineRenderer(int lineIndex, Material mat, Vector2 endPoint)
    {
        if (existingLineRenderer != null)
        {
            existingLineRenderer[lineIndex].material = mat;
            existingLineRenderer[lineIndex].SetPosition(0, transform.position);
            existingLineRenderer[lineIndex].SetPosition(1, endPoint);
        }

    }

    public void SwitchOffLines()
    {
        foreach (LineRenderer line in existingLineRenderer)
        {
            line.enabled = false; 
        }
    }
}
