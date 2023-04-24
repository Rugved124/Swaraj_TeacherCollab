using Capabilities;
using Checks;
using Enums;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Character
{
    [RequireComponent(typeof(Jump))]
    [RequireComponent(typeof(Move))]
    [RequireComponent(typeof(Ground))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PC : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private PlayerInput _pi;
        
        /*
         * Input actions
         */
        private InputAction _move;
        private InputAction _climb;
        private InputAction _sprint;
        private InputAction _jump;
        private InputAction _crouch;
        private InputAction _shoot;
        private InputAction _aim;
        private InputAction _aimStickX;
        private InputAction _aimStickY;
        private InputAction _normal;
        private InputAction _paranoid;
        private InputAction _blind;

        // Gets the current state of the player
        [SerializeField]
        private EMovementState currentMovementState;
        // Gets the current state of the player
        private EPlayerState currentPlayerState;

        private Ground groundScript;
        private Move moveScript;
        private Jump jumpScript;

        private void Awake()
        {
            // Initialise components
            groundScript = GetComponent<Ground>();
            moveScript = GetComponent<Move>();
            jumpScript = GetComponent<Jump>();
            _pi = GetComponent<PlayerInput>();
            _rb = GetComponent<Rigidbody2D>();
            
            // Initialise input
            _move = _pi.actions["Move"];
            _climb = _pi.actions["Climb"];
            _sprint = _pi.actions["Sprint"];
            _jump = _pi.actions["Jump"];
            _crouch = _pi.actions["Crouch"];
            _shoot = _pi.actions["Shoot"];
            _aim = _pi.actions["Aim"];
            _aimStickX = _pi.actions["AimStickX"];
            _aimStickY = _pi.actions["AimStickY"];
            _normal = _pi.actions["NormalArrow"];
            _paranoid = _pi.actions["ParanoidArrow"];
            _blind = _pi.actions["BlindArrow"];
        }

        private void Update()
        {
            if (groundScript.GetOnGround())
            {
                if (moveScript.desiredSprint)
                {
                    SetMovementState(EMovementState.Sprinting);
                }
                else
                {
                    SetMovementState(_rb.velocity.magnitude != 0 ? EMovementState.Walking : EMovementState.Idle);
                }
            }
            else
            {
                SetMovementState(EMovementState.Jumping);
            }
            UpdateDirection();
        }

        private void UpdateDirection()
        {
            switch (GetInput(EInputAction.Move).ReadValue<float>())
            {
                case > 0:
                    transform.rotation = Quaternion.Euler(0f, 0f, 0f);
                    break;
                case < 0:
                    transform.rotation = Quaternion.Euler(0f, 180f, 0f);
                    break;
                case 0:
                    break;
            }
        }

        public InputAction GetInput(EInputAction inputAction)
        {
            return inputAction switch
            {
                EInputAction.Move => _move,
                EInputAction.Climb => _climb,
                EInputAction.Sprint => _sprint,
                EInputAction.Jump => _jump,
                EInputAction.Crouch => _crouch,
                EInputAction.Shoot => _shoot,
                EInputAction.Aim => _aim,
                EInputAction.AimStickX => _aimStickX,
                EInputAction.AimStickY => _aimStickY,
                EInputAction.Normal => _normal,
                EInputAction.Paranoid => _paranoid,
                EInputAction.Blind => _blind,
                _ => null
            };
        }

        public EPlayerState GetPlayerState()
        {
            return currentPlayerState;
        }

        public void SetPlayerState(EPlayerState newState)
        {
            currentPlayerState = newState;
        }
        
        public EMovementState GetMovementState()
        {
            return currentMovementState;
        }

        public void SetMovementState(EMovementState newState)
        {
            currentMovementState = newState;
        }
    }
}
