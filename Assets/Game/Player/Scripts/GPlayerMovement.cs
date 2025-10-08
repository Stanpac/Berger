using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

public class GPlayerMovement : MonoBehaviour
{
    public Vector2 inputMovementDirection { private get; set; }
    [SerializeField] private float _speed;
    [SerializeField] private float _acceleration, _deceleration;
    [SerializeField] private float _gravityMultiplier;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _jumpStrength;
    [SerializeField] private float _groundAngleLimit;
    [SerializeField, ReadOnly] private bool _isGrounded;
    private Vector3 _currentMovementDirection;
    private float _currentAcceleration;
    [SerializeField, ReadOnly] private Vector3 _currentVelocity;
    private CharacterController _characterController;
    
    // cinemachine Needs these hooks to work properly
    public Action PreUpdate;
    public Action<Vector3, float> PostUpdate;

    public void Jump()
    {
        if (_isGrounded)
        {
            _characterController.Move(Vector3.up * _jumpStrength);
        }
    }
    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(transform.position - Vector3.up * transform.localScale.y, .2f, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);
        ComputeHorizontalMovement();
        ComputeGravity();
        ApplyMovement();
        Rotate();
    }

    private void ComputeGravity()
    {
        float velocityY = _isGrounded ? 0 : _currentVelocity.y + _gravityMultiplier * Physics.gravity.y * Time.fixedDeltaTime;
        _currentVelocity.y = velocityY;
    }

    private void ComputeHorizontalMovement()
    {
        bool isDecelerating = inputMovementDirection == Vector2.zero;
        _currentMovementDirection = isDecelerating ? 
            _currentMovementDirection : 
            new Vector3(inputMovementDirection.x, 0, inputMovementDirection.y);
        
        float accelerationMult = isDecelerating ? -1 : 1;
        float accelerationMagnitude = isDecelerating ?
            _deceleration :
            _acceleration;
        
        _currentAcceleration = Mathf.Clamp(_currentAcceleration + accelerationMult / accelerationMagnitude * Time.fixedDeltaTime, 0, 1);
        float velocityY = _currentVelocity.y;
        _currentVelocity = _currentMovementDirection * (_speed * _currentAcceleration * Time.fixedDeltaTime);
        _currentVelocity.y = velocityY;
    }

    private void ApplyMovement()
    {
        _characterController.Move(_currentVelocity);
    }

    private void Rotate()
    {
        _currentMovementDirection.y = 0;
        if (_currentMovementDirection == Vector3.zero) return;
        Quaternion toRotation = Quaternion.LookRotation(_currentMovementDirection, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, _rotationSpeed * Time.fixedDeltaTime);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawSphere(transform.position - Vector3.up * transform.localScale.y, .2f);
    }
}
