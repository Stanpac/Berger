using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GPlayerMovement : MonoBehaviour
{
    public Vector2 inputMovementDirection { private get; set; }
    [SerializeField] private float _speed;
    [SerializeField] private float _acceleration, _deceleration;
    [SerializeField] private float _gravityMultiplier;
    [SerializeField] private float _rotationSpeed;
    private Vector3 _currentMovementDirection;
    private float _currentAcceleration;
    private Vector3 _currentVelocity;
    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        ComputeHorizontalMovement();
        ComputeGravity();
        ApplyMovement();
        Rotate();
    }

    private void ComputeGravity()
    {
        float velocityY = _characterController.isGrounded ? 0 : _currentVelocity.y + _gravityMultiplier * Physics.gravity.y * Time.fixedDeltaTime;
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
}
