using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;


// Class to handle nav mesh movement for a game entity
[RequireComponent(typeof(NavMeshAgent))]
public class GNavMovement : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    
    [SerializeField, ReadOnly]
    private bool _isMoving;
    
    public void MoveTo(Vector3 targetPosition)
    {
        if (_navMeshAgent.SetDestination(targetPosition)) {
            _isMoving = true;
            Debug.Log($"{gameObject.name} Moving to position: " + targetPosition);
        } else {
            Debug.LogWarning($"Failed to set destination to {targetPosition} in {gameObject.name}");
        }
    }
    
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    private void Update()
    {
        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !_navMeshAgent.pathPending && _isMoving)  
        {
            Debug.Log($"{gameObject.name} Reached destination");
            _isMoving = false;
        }
    }
}
