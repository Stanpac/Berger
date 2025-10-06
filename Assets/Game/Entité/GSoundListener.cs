using System;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

// Class to handle hearing capabilities for a game entity 
public class GSoundListener : MonoBehaviour
{
    public UnityEvent<Vector3> onSoundHeard;
    
    // To Call For Transmit a Sound 
    public void HearSound(Vector3 soundPosition)
    {
        Debug.Log("Sound Heard at position: " + soundPosition);
        
        onSoundHeard?.Invoke(soundPosition);
    }

    private void Start()
    {
        GEntityManager.Instance.RegisterEntity(this);
    }

    // Debugging method to simulate hearing a sound at a specific position 
    [Button]
    private void DebugSimulateSound(Vector3 soundPosition)
    {
        HearSound(soundPosition);
    }
}



// TODO Editor for put Debug in the End of the scrput ! 
