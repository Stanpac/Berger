using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GEntityInventory : MonoBehaviour
{
    [field: SerializeField, ReadOnly] 
    public List<GEntity> agents { get; private set; }
    
    [SerializeField, ReadOnly]
    private List<Vector3> _lastPositions;
    
    public int lastPositionMaxCount = 10;
    public float minDistanceToRecord = 0.1f;
    public float timebetweenRecords = 0.5f;
    
    [SerializeField, ReadOnly]
    private float _timeSinceLastRecord = 0f;
    
    public void RegisterEntity(GEntity agent)
    {
        if (agents.Contains(agent))
        {
            Debug.LogError("Trying to register an entity that is already registered", agent);
            return;
        }
        agents.Add(agent);
    }

    public void UnregisterEntity(GEntity agent)
    {
        if (!agents.Contains(agent))
        {
            Debug.LogError("Trying to unregister an entity that is not registered", agent);
            return;
        }
        agents.Remove(agent);
    }

    public void DeleteEntities(int entityCount)
    {
        for (int i = entityCount - 1; i >= 0; i--)
        {
            var agent = agents[i];
            UnregisterEntity(agents[i]);
            agent.gameObject.SetActive(false);
        }
    }

    private void Start()                                                       
    {
        agents = new List<GEntity>();
        _lastPositions = new List<Vector3>();
        
        _lastPositions.Add(transform.position);
    }

    private void FixedUpdate()
    {
        _timeSinceLastRecord += Time.fixedDeltaTime;
        if (_timeSinceLastRecord >= timebetweenRecords)
        {
            _timeSinceLastRecord = 0f;
            if (Vector3.Distance(transform.position, _lastPositions[^1]) >= minDistanceToRecord)
            {
                _lastPositions.Add(transform.position);
                if (_lastPositions.Count > lastPositionMaxCount)
                {
                    _lastPositions.RemoveAt(0);
                }
            }
        }
        
        // assign last position Register to first agent 
        int index = Math.Max(0, _lastPositions.Count - 2);
        foreach (GEntity agent in agents) 
        {
            agent.DesignateTarget(_lastPositions[index]);
            index = Math.Max(0, index - 1);
        }
    }
}
