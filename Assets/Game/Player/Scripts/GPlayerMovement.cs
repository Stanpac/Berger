using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GPlayerMovement : MonoBehaviour
{
    public Vector2 inputMovementDirection { private get; set; }
    [SerializeField] private float _speed;
    [SerializeField] private float _acceleration, _deceleration;
    private Vector3 _currentMovementDirection;
    private float _currentAcceleration;
    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
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
        _characterController.Move(_currentMovementDirection * _speed * _currentAcceleration * Time.fixedDeltaTime);
    }
}
