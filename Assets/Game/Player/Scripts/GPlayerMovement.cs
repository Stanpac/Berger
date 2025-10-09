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
    [FormerlySerializedAs("_groundAngleLimit")] [SerializeField] private float _groundCheckSize;
    [SerializeField, ReadOnly] private bool _isGrounded;
    private Vector3 _currentMovementDirection;
    private float _currentAcceleration;
    [SerializeField, ReadOnly] private Vector3 _currentVelocity;
    private CharacterController _characterController;

    private Transform _currentPlatform;
    private Vector3 _lastPlatformPosition;
    
    public void Jump()
    {
        if (_isGrounded)
        {
            // Set upward velocity instead of moving directly
            _currentVelocity.y = _jumpStrength;
        }
    }
    
    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(transform.position - Vector3.up * transform.localScale.y, _groundCheckSize, LayerMask.GetMask("Ground"), QueryTriggerInteraction.Ignore);
        ComputeHorizontalMovement();
        HandlePlatformMovement();
        ComputeGravity();
        ApplyMovement();
        Rotate();
    }

    private void ComputeGravity()
    {
        if (_isGrounded && _currentVelocity.y < 0)
        {
            // Small downward force to keep grounded
            _currentVelocity.y = -2f;
        }
        else
        {
            // Apply gravity
            _currentVelocity.y += _gravityMultiplier * Physics.gravity.y * Time.fixedDeltaTime;
        }
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
        
        // Don't overwrite velocity.y here
        Vector3 horizontalVelocity = _currentMovementDirection * (_speed * _currentAcceleration * Time.fixedDeltaTime);
        _currentVelocity.x = horizontalVelocity.x;
        _currentVelocity.z = horizontalVelocity.z;
    }

    private void ApplyMovement()
    {
        _characterController.Move(_currentVelocity);
    }

    void HandlePlatformMovement()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _characterController.height / 2 + 0.3f, LayerMask.GetMask("Ground")))
        {
            Transform hitTransform = hit.collider.transform;
            
            if (_currentPlatform != hitTransform)
            {
                _currentPlatform = hitTransform;
                if (_currentPlatform != null)
                {
                    _lastPlatformPosition = _currentPlatform.position;
                }
            }
        }
        else
        {
            _currentPlatform = null;
        }
        
        if (_currentPlatform != null && _isGrounded)
        {
            Vector3 platformDelta = _currentPlatform.position - _lastPlatformPosition;
            _characterController.Move(platformDelta);
            _lastPlatformPosition = _currentPlatform.position;
        }
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
        Gizmos.DrawSphere(transform.position - Vector3.up * transform.localScale.y, _groundCheckSize);
    }
}