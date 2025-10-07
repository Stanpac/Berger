using UnityEngine;

[CreateAssetMenu(fileName = "Player Ability", menuName = "Ability")]
public abstract class GPlayerAbility : ScriptableObject
{
    [SerializeField] protected int _entityCost;
    protected GEntityInventory _inventory;

    public virtual void OnStart(GPlayerAbilitySystem player)
    {
        _inventory = player.GetComponent<GEntityInventory>();

    }
    
    public virtual bool StartAbility()
    {
        if (_entityCost <= _inventory.agents.Count)
        {
            ProcessEntities();
            OnAbilityStarted();
            return true;
        }

        return false;
    }

    protected virtual void OnAbilityStarted()
    {
        
    }

    protected virtual void ProcessEntities()
    {
        if (_entityCost > 0)
        {
            _inventory.DeleteEntities(_entityCost);
        }
    }
    
}

public class GPlayerAbility_SpawnItem : GPlayerAbility
{
    
}