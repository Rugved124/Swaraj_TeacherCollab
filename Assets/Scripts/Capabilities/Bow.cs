using System;
using Character;
using Enums;
using Lib;
using Projectiles;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Capabilities
{
    [RequireComponent(typeof(TrajectoryLine))]
    [RequireComponent(typeof(PC))]
    public class Bow : MonoBehaviour
    {
        [SerializeField,Range(0,100),Tooltip("Max speed of the arrow on full power")]
        private float maxShotSpeed;
        
        [SerializeField,Range(0,100),Tooltip("Max speed of the arrow on min power")]
        private float minShotSpeed;
        
        [SerializeField,Range(0,100),Tooltip("Max angle of the arrow on full power")]
        private float maxAngle;
        
        [SerializeField,Range(0,100),Tooltip("Min angle of the arrow on min power")]
        private float minAngle;

        [SerializeField,Range(0,100),Tooltip("Max lifetime of an arrow")] 
        private float lifetime;

        [SerializeField,Tooltip("Prefab of the arrow")] 
        private GameObject arrowPrefab;
        
        [SerializeField,Tooltip("Prefab of the cross-hair")] 
        private GameObject crossPrefab;

        [SerializeField,Tooltip("Sensitivity of aiming")]
        private float sensitivity;

        [SerializeField, Tooltip("Max amount of paranoid arrows to be given to the player")]
        private int maxParanoidCount = 10;
        
        [SerializeField, Tooltip("Max amount of paranoid arrows to be given to the player")]
        private int maxBlindArrow = 10;
        
        [SerializeField,Tooltip("Speed of the arrow")] 
        private float speed;
        
        [SerializeField,Tooltip("Parabola Height")]
        private float parabolaHeight;
        
        private EArrowType currentArrow = EArrowType.Normal;

        public BaseArrow arrow;
        private GameObject crosshair;
        private PC _pc;
        
        private int paranoidArrowCount;
        private int blindArrowCount;
        
        private Vector2 _shotPower;
        private TrajectoryLine _trajectoryLine;
        private float aimSmoothInput;
        private float triggerSmoothInput;

        private void Awake()
        {
            _pc = GetComponent<PC>();
            _trajectoryLine = GetComponent<TrajectoryLine>();
        }

        private void Start()
        {
            _trajectoryLine.lineRenderer.enabled = false;
            _pc.GetInput(EInputAction.Shoot).performed += ShootCheck;
            _pc.GetInput(EInputAction.Aim).performed += StartAiming;
            _pc.GetInput(EInputAction.Aim).canceled += StopAiming;
            _pc.GetInput(EInputAction.Normal).performed += SwitchNormalArrow;
            _pc.GetInput(EInputAction.Blind).performed += SwitchBlindArrow;
            _pc.GetInput(EInputAction.Paranoid).performed += SwitchParanoidArrow;
        }

        private void StopAiming(InputAction.CallbackContext obj)
        {
            _pc.SetPlayerState(EPlayerState.Normal);
            Destroy(crosshair);
        }

        private void StartAiming(InputAction.CallbackContext obj)
        {
            _pc.SetPlayerState(EPlayerState.Shooting);
            crosshair = Instantiate(crossPrefab,transform.position,Quaternion.identity);
        }

        private void SwitchBlindArrow(InputAction.CallbackContext obj)
        {
            currentArrow = EArrowType.Blind;
        }

        private void SwitchParanoidArrow(InputAction.CallbackContext obj)
        {
            currentArrow = EArrowType.Paranoid;
        }

        private void SwitchNormalArrow(InputAction.CallbackContext obj)
        {
            currentArrow = EArrowType.Normal;
        }

        private void OnDisable()
        {
            _pc.GetInput(EInputAction.Shoot).performed -= ShootCheck;
            _pc.GetInput(EInputAction.Aim).performed -= StartAiming;
            _pc.GetInput(EInputAction.Aim).canceled -= StopAiming;
            _pc.GetInput(EInputAction.Normal).performed -= SwitchNormalArrow;
            _pc.GetInput(EInputAction.Blind).performed -= SwitchBlindArrow;
            _pc.GetInput(EInputAction.Paranoid).performed -= SwitchParanoidArrow;
        }

        private void Update()
        {
            if (_pc.GetPlayerState() == EPlayerState.Shooting )
            {
                float aimX = _pc.GetInput(EInputAction.AimStickX).ReadValue<float>() * sensitivity;
                float aimY = _pc.GetInput(EInputAction.AimStickY).ReadValue<float>() * sensitivity;
                crosshair.transform.Translate(aimX,aimY,0);
            }
            else
            {
                _trajectoryLine.lineRenderer.enabled = false;
            }
        }
        
        private void ShootCheck(InputAction.CallbackContext cc)
        {
            if (_pc.GetMovementState() != EMovementState.Jumping &&
                _pc.GetPlayerState() == EPlayerState.Shooting)
            {
                Debug.Log("Arrow shot");
                arrow = Instantiate(arrowPrefab,transform.position,Quaternion.identity).GetComponent<BaseArrow>();
                arrow.endMarker = crosshair.transform.position;
                arrow.height = parabolaHeight;
                arrow.targetSpeed = speed;
            }
        }
    }
}