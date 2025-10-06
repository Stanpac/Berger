using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GEntityManager : GSingleton<GEntityManager>
{
    public List<GSoundListener> listeningAgents {get; private set;}

    protected override void Awake()
    {
        base.Awake();
        listeningAgents = new List<GSoundListener>();
    }

    public void RegisterEntity(GSoundListener soundListenerAgent)
    {
        if (listeningAgents.Contains(soundListenerAgent))
        {
            Debug.LogError("Trying to register an entity that is already registered", soundListenerAgent);
            return;
        }
        listeningAgents.Add(soundListenerAgent);
    }

    public void UnregisterEntity(GSoundListener soundListenerAgent)
    {
        if (!listeningAgents.Contains(soundListenerAgent))
        {
            Debug.LogError("Trying to unregister an entity that is not registered", soundListenerAgent);
            return;
        }
        listeningAgents.Remove(soundListenerAgent);
    }
}
