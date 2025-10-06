using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GEntity : SerializedMonoBehaviour
{
    public enum EEntityState { Passive, InInventory, Charmed }
    
    [field: Unity.Collections.ReadOnly, SerializeField] public EEntityState _currentState { get; private set; }

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
    }

    private void OnStateExit()
    {
        
    }
    
    private void Start()
    {
        GEntityManager.Instance.RegisterEntity(this);
        _navMovement = GetComponent<GNavMovement>();
    }
}
