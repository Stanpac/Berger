using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GPlayerAbilitySystem : MonoBehaviour
{
    [SerializeField] private List<GPlayerAbility> _abilities;
    [SerializeField, ReadOnly] private List<GInteractable> _interactableAbilities;
    private List<GPlayerAbility> _playerAbilitiesInstances;
    
    public void Interact()
    {
        if (_interactableAbilities.Count > 0)
        {
            
        }
    }
    
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
        if (_abilities.Count <= abilityIndex) return;
        GPlayerAbility_Async asyncAbility = _abilities[abilityIndex] as GPlayerAbility_Async;
        if (asyncAbility != null)
        {
            asyncAbility.CancelAbility();
        }
    }
    
    private void Start()
    {
        for (int i = 0; i < _abilities.Count; i++)
        {
            GPlayerAbility ability = _abilities[i];
            _abilities[i] = ScriptableObject.Instantiate(ability);
            _abilities[i].OnStart(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            var interactable = other.GetComponentInParent<GInteractable>();
            if (interactable != null && !_interactableAbilities.Contains(interactable))
            {
                _interactableAbilities.Add(interactable);
            }
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactable"))
        {
            var interactable = other.GetComponentInParent<GInteractable>();
            if (interactable != null && _interactableAbilities.Contains(interactable))
            {
                _interactableAbilities.Remove(interactable);
            }
        }
    }
}
