using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GEntityManager : GSingleton<GEntityManager>
{
    public List<GEntity> agents {get; private set;}

    [SerializeField] private float _agentsDetectionRange;
    
    protected override void Awake()
    {
        base.Awake();
        agents = new List<GEntity>();
    }

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
}
