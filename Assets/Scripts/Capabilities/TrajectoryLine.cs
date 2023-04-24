using Character;
using Enums;
using UnityEngine;

namespace Capabilities
{
    public class TrajectoryLine : MonoBehaviour
    {
        [SerializeField] private float timeStep = 0.1f;
        [SerializeField] private float lineSpacing = 0.1f;
        [SerializeField] private int maxPositions = 100;

        [HideInInspector]
        public Vector3 direction;
        [HideInInspector]
        public float shotPower = 10f;
        [HideInInspector]
        public LineRenderer lineRenderer;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        private void Start()
        {
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
        }

        private void FixedUpdate()
        {
            DrawTrajectory();
        }

        private void DrawTrajectory()
        {
            Vector3 currentPosition = transform.position;
            Vector3 currentVelocity = direction.normalized * shotPower;
            Vector3 gravity = Physics.gravity;

            lineRenderer.positionCount = 1;
            lineRenderer.SetPosition(0, currentPosition);

            int positionIndex = 1;

            for (float t = timeStep; t < maxPositions * timeStep; t += timeStep)
            {
                Vector3 newPosition = currentPosition + currentVelocity * t + gravity * (0.5f * t * t);
                currentVelocity += gravity * t;
                currentPosition = newPosition;

                if (positionIndex % Mathf.RoundToInt(lineSpacing / timeStep) == 0)
                {
                    lineRenderer.positionCount++;
                    lineRenderer.SetPosition(positionIndex, currentPosition);
                }

                positionIndex++;
            }
        }
    }
}