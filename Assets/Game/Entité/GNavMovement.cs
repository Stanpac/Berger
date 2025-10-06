using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;


// Class to handle nav mesh movement for a game entity
[RequireComponent(typeof(NavMeshAgent))]
public class GNavMovement : MonoBehaviour
{
    private NavMeshAgent _navMeshAgent;
    private GEntity _entity;
    private IEnumerator _movingEnum;
    
    public void MoveTo(Vector3 targetPosition, Action onComplete = null)
    {
        if (_navMeshAgent.SetDestination(targetPosition)) {
            if (_movingEnum != null)
            {
                StopCoroutine(_movingEnum);
            }
            _movingEnum = MovementCoroutine(onComplete);
            StartCoroutine(_movingEnum);
            Debug.Log($"{gameObject.name} Moving to position: " + targetPosition);
        } else {
            Debug.LogWarning($"Failed to set destination to {targetPosition} in {gameObject.name}");
        }
    }
    
    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _entity = GetComponent<GEntity>();
    }

    IEnumerator MovementCoroutine(Action onComplete = null)
    {
        yield return new WaitUntil(() => _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance && !_navMeshAgent.pathPending);
        Debug.Log($"{gameObject.name} Reached destination");
        _entity.ChangeState(GEntity.EEntityState.Passive);
        onComplete?.Invoke();
        _movingEnum = null;
    }
}
