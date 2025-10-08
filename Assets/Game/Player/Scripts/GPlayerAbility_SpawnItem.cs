using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Player Spawn Ability", menuName = "Ability/Spawn Structure")]
public class GPlayerAbility_SpawnItem : GPlayerAbility
{
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private Vector3 _spawnPositionOffset;
    private GameObject _itemInstance;

    protected override void OnAbilityStarted()
    {
        base.OnAbilityStarted();
        _itemInstance = GameObject.Instantiate(_itemPrefab, _inventory.transform.position +_inventory.transform.rotation * _spawnPositionOffset , _inventory.transform.rotation);
        GameObject.FindFirstObjectByType<NavMeshSurface>().BuildNavMesh();
    }

    protected override void ProcessEntities()
    {
        _itemInstance.GetComponent<GEntityContainer>().BufferEntities(_inventory.agents.GetRange(0, _entityCost));
        base.ProcessEntities();
    }
}