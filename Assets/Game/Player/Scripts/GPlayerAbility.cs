using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Ability", menuName = "Ability")]
public class GPlayerAbility : ScriptableObject
{
    protected GEntityInventory _inventory;
    [SerializeField] protected int _entityCost;
    [SerializeField] protected bool _isCancellable;
    protected IEnumerator _abilityEnum;
    protected bool _isAbilityActive;

    public virtual void OnStart(GPlayerAbilitySystem player)
    {
        _inventory = player.GetComponent<GEntityInventory>();
    }
    
    public bool StartAbility()
    {
        if (_entityCost <= _inventory.agents.Count && (_isCancellable || _abilityEnum == null))
        {
            if (_abilityEnum != null)
            {
                _inventory.StopCoroutine(_abilityEnum);
            }

            _abilityEnum = AbilityCoroutine();
            _inventory.StartCoroutine(_abilityEnum);

            return true;
        }

        return false;
    }

    public void CancelAbility()
    {
        _isAbilityActive = false;
    }

    protected virtual void ProcessEntities()
    {
        if (_entityCost > 0)
        {
            _inventory.DeleteEntities(_entityCost);
        }
    }
    
    protected virtual void OnAbilityStarted()
    {
        
    }
    
    protected virtual void OnAbilityTick()
    {
        
    }


    protected virtual void OnAbilityEnded()
    {
        
    }

    protected IEnumerator AbilityCoroutine()
    {
        _isAbilityActive = true;
        OnAbilityStarted();
        ProcessEntities();

        while (_isAbilityActive)
        {
            OnAbilityTick();
            yield return new WaitForFixedUpdate();
        }
        OnAbilityEnded();
        _abilityEnum = null;
    }
    
}