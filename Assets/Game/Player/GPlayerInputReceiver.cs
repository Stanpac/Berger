using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GPlayerInputReceiver : MonoBehaviour
{
    private GPlayerMovement _playerMovement;
    private GSoundEmitter _soundEmitter;
    
    public void OnTriggerBell()
    {
        if (_soundEmitter == null) return;
        _soundEmitter.OnTriggerBell();
    }

    public void OnMove(InputValue cbx)
    {
        if (_playerMovement == null) return;
        
        Vector3 movementDirection;
        Vector2 moveInput = cbx.Get<Vector2>();
        
        Vector2 cameraForward = new Vector2(Camera.main.transform.forward.x, Camera.main.transform.forward.z);
        Vector2 cameraRight = new Vector2(Camera.main.transform.right.x, Camera.main.transform.right.z);
            
        // Project vectors to ground plane (zero out Y component for pure horizontal movement)
        cameraForward.Normalize(); // Normalize after zeroing Y
        cameraRight.Normalize();

        // Create movement direction using the horizontal components
        movementDirection = moveInput.x * cameraRight + moveInput.y * cameraForward;
        _playerMovement.inputMovementDirection = movementDirection;
    }

    private void Start()
    {
        _playerMovement = GetComponent<GPlayerMovement>();
        _soundEmitter = GetComponent<GSoundEmitter>();
    }
}
