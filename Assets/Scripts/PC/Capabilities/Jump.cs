using System;
using Character;
using Checks;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Capabilities
{
    [RequireComponent(typeof(Ground))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PC))]
    public class Jump : MonoBehaviour
    {
        [SerializeField, Range(0, 10)] 
        private float jumpHeight = 3;

        [SerializeField, Range(0, 5)] 
        private float downwardMovementMultiplier = 3;
        
        [SerializeField, Range(0, 5)] 
        private float upwardMovementMultiplier = 1.7f;

        private Rigidbody2D _rb;
        private Ground _ground;
        private Vector2 _velocity;

        private float _defaultGravityScale;

        private bool _desiredJump;
        private bool _onGround;
        private PC _pc;
        private EMovementState prevMovementState;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _ground = GetComponent<Ground>();
            _pc = GetComponent<PC>();
            _defaultGravityScale = 1f;
        }

        private void Start()
        {
            _pc.GetInput(EInputAction.Jump).performed += JumpAction;
        }

        private void OnDisable()
        {
            _pc.GetInput(EInputAction.Jump).performed -= JumpAction;
        }

        private void FixedUpdate()
        {
            _onGround = _ground.GetOnGround();
            if (_pc.GetMovementState() != EMovementState.Jumping)
            {
                prevMovementState = _pc.GetMovementState();
            }
            _pc.SetMovementState(!_onGround ? EMovementState.Jumping : prevMovementState);
            
            _velocity = _rb.velocity;

            switch (_rb.velocity.y)
            {
                case > 0:
                    _rb.gravityScale = upwardMovementMultiplier;
                    break;
                case < 0:
                    _rb.gravityScale = downwardMovementMultiplier;
                    break;
                case 0:
                    _rb.gravityScale = _defaultGravityScale;
                    break;
            }

            _rb.velocity = _velocity;
        }

        private void JumpAction(InputAction.CallbackContext cc)
        {
            if (_onGround && _pc.GetPlayerState() == EPlayerState.Normal)
            {
                _rb.AddForce(Vector2.up * 100 * jumpHeight);
            }
        }
    }
}
