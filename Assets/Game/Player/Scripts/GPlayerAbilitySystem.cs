using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GPlayerAbilitySystem : MonoBehaviour
{
    [SerializeField] private List<GPlayerAbility> _abilities;

    public void StartAbility(int abilityIndex)
    {
        if (_abilities.Count <= abilityIndex)
        {
            Debug.LogWarning("Ability " + abilityIndex + " is not present in player abilities");
            return;
        }
        _abilities[abilityIndex].StartAbility();
    }

    public void CancelAbility(int abilityIndex)
    {        
        if (_abilities.Count <= abilityIndex)
        {
            return;
        }
        _abilities[abilityIndex].CancelAbility();
    }

    private void Start()
    {
        foreach (var ability in _abilities)
        {
            ability.OnStart(this);
        }
    }
}
