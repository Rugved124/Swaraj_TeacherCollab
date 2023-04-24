using Character;
using Checks;
using Enums;
using UnityEngine;
using UnityEngine.Serialization;

namespace Capabilities
{
    [RequireComponent(typeof(Ground))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(PC))]
    public class Move : MonoBehaviour
    {
        [SerializeField,Range(0,100)] 
        private float maxWalkSpeed = 100;
        
        [SerializeField,Range(0,100)] 
        private float maxSprintSpeed = 8;
        
        [SerializeField,Range(0,100)] 
        private float maxAcceleration = 35;
        
        [SerializeField,Range(0,100)] 
        private float maxAirAcceleration = 20;

        private Vector2 _direction;
        private Vector2 _desiredVelocity;
        private Vector2 _velocity;
        private Rigidbody2D _rb;
        private Ground _ground;
        private PC _pc;

        private float _maxSpeedChange;
        [SerializeField]
        public bool desiredSprint;
        private float _acceleration;
        private bool _onGround;

        private void Awake()
        {
            _pc = GetComponent<PC>();
            _ground = GetComponent<Ground>();
            _rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_pc.GetPlayerState() == EPlayerState.Normal)
            {
                _direction.x = _pc.GetInput(EInputAction.Move).ReadValue<float>();
                desiredSprint = _pc.GetInput(EInputAction.Sprint).ReadValue<float>() > 0;
                _desiredVelocity = new Vector2(_direction.x, 0f) * Mathf.Max((desiredSprint ? maxSprintSpeed : maxWalkSpeed) - _ground.GetFriction(), 0f);
            }
        }

        private void FixedUpdate()
        {
            if (_pc.GetPlayerState() == EPlayerState.Normal)
            {
                _onGround = _ground.GetOnGround();
                _velocity = _rb.velocity;
                _acceleration = _onGround ? maxAcceleration : maxAirAcceleration;
                _maxSpeedChange = _acceleration * Time.deltaTime;
                _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _maxSpeedChange);
                _rb.velocity = _velocity;
            }
            else
            {
                _rb.velocity = Vector2.zero;
                _desiredVelocity = Vector2.zero;
            }
        }
    }
}
