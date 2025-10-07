using System.Collections;
using UnityEngine;

public abstract class GPlayerAbility_Async : GPlayerAbility
{
    [SerializeField] protected float _customTickRate;
    protected IEnumerator _abilityEnum;
    protected bool _isAbilityActive;
    protected WaitUntil _waitUntilAbilityCancelled;
    protected WaitForSeconds _waitForCustomTick;
    protected WaitForFixedUpdate _waitForFixedUpdate;
    
    public override bool StartAbility()
    {
        if (_entityCost <= _inventory.agents.Count && _abilityEnum == null)
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

    public override void OnStart(GPlayerAbilitySystem player)
    {
        base.OnStart(player);
        _waitForCustomTick = new WaitForSeconds(_customTickRate);
        _waitForFixedUpdate = new WaitForFixedUpdate();
        _waitUntilAbilityCancelled = new WaitUntil(() => !_isAbilityActive);
    }
    
    protected virtual bool IsTickEnabled() => false;
    protected virtual bool IsPhysicsTickEnabled() => false;
    protected virtual  bool IsCustomTickEnabled() => false;

    protected virtual void OnAbilityPhysicsTick()
    {
        
    }

    protected virtual void OnAbilityTick()
    {
        
    }
    
    protected virtual void OnAbilityCustomTick()
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
        
        if(IsCustomTickEnabled()) _inventory.StartCoroutine(AbilityCustomTickCoroutine());
        if(IsTickEnabled()) _inventory.StartCoroutine(AbilityTickCoroutine());
        if(IsPhysicsTickEnabled()) _inventory.StartCoroutine(AbilityPhysicsTickCoroutine());
        
        yield return _waitUntilAbilityCancelled;

        OnAbilityEnded();
        _abilityEnum = null;
    }
    
    protected IEnumerator AbilityTickCoroutine()
    {
        while (_isAbilityActive)
        {
            OnAbilityTick();
            yield return null;
        }
    }

    protected IEnumerator AbilityCustomTickCoroutine()
    {
        while (_isAbilityActive)
        {
            OnAbilityCustomTick();
            yield return _waitForCustomTick;
        }
    }

    protected IEnumerator AbilityPhysicsTickCoroutine()
    {
        while (_isAbilityActive)
        {
            OnAbilityPhysicsTick();
            yield return _waitForFixedUpdate;
        }
    }
}