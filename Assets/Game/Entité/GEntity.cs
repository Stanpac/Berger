using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class GEntity : SerializedMonoBehaviour
{
    public enum EEntityState { Passive, InInventory, Charmed }
    
    [field: Unity.Collections.ReadOnly, SerializeField] public EEntityState _currentState { get; private set; }

    [SerializeField]
    private float _passiveSpeed = 1.5f;
    
    [SerializeField]
    private float _charmedSpeed = 3.5f;
    
    private NavMeshAgent _navMeshAgent;
    
    [SerializeField] private Dictionary<EEntityState, Color> _entityColors;
    private GNavMovement _navMovement;

    public void ChangeState(EEntityState newState)
    {
        OnStateExit();
        _currentState = newState;
        OnStateEnter();
    }

    public void DesignateTarget(Vector3 position, Action onMovementComplete = null)
    {
        _navMovement.MoveTo(position, onMovementComplete);
    }
    
    private void OnStateEnter()
    {
        var renderers = GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            renderer.material.color = _entityColors[_currentState];
        }
        
        if (_currentState == GEntity.EEntityState.Passive)
        {
            _navMeshAgent.speed = _passiveSpeed;
        }
        
        if (_currentState == GEntity.EEntityState.Charmed)
        {
            _navMeshAgent.speed = _charmedSpeed;
        }
    }

    private void OnStateExit()
    {
        
    }

    private void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        GEntityManager.Instance.RegisterEntity(this);
        _navMovement = GetComponent<GNavMovement>();
        OnStateEnter();
    }

    private void Update()
    {
        /*if (_currentState == EEntityState.Passive && !_navMovement.IsMoving)
        {
            // Add Random Minor movement near
            Vector3 CurrentPosition = transform.position;
            CurrentPosition += new Vector3(UnityEngine.Random.Range(-1f, 1f), 0, UnityEngine.Random.Range(-1f, 1f));
            _navMovement.MoveTo(CurrentPosition);
        }*/
    }
}
