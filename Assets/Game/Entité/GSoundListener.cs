using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

// Class to handle hearing capabilities for a game entity 
public class GSoundListener : MonoBehaviour
{
    public UnityEvent<Vector3, Action> onSoundHeard;
    private GEntity _entity;
    
    // To Call For Transmit a Sound 
    public void HearSound(Vector3 soundPosition)
    {
        Debug.Log("Sound Heard at position: " + soundPosition);
        onSoundHeard?.Invoke(soundPosition, OnMovementComplete);
        _entity.ChangeState(GEntity.EEntityState.Charmed);
    }

    // Debugging method to simulate hearing a sound at a specific position 
    [Button]
    private void DebugSimulateSound(Vector3 soundPosition)
    {
        HearSound(soundPosition);
    }

    private void OnMovementComplete()
    {
        _entity.ChangeState(GEntity.EEntityState.Passive);
    }
    
    private void Start()
    {
        _entity = GetComponent<GEntity>();
    }
}



// TODO Editor for put Debug in the End of the scrput ! 
