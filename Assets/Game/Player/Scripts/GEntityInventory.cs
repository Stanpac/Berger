using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GEntityInventory : MonoBehaviour
{
    [field: SerializeField, ReadOnly] public List<GEntity> agents { get; private set; }
    
    
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
    }

    private void FixedUpdate()
    {
        agents.ForEach(agent => agent.DesignateTarget(transform.position));
    }
}
