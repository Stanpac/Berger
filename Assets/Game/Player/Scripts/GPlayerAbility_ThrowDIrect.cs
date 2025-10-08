using UnityEngine;

[CreateAssetMenu(fileName = "Player Throw Ability", menuName = "Ability/ThrowDirectAbility")]
public class GPlayerAbility_ThrowDirect : GPlayerAbility
{
    
    [SerializeField] 
    protected float strength = 10f;
    
    [SerializeField] 
    protected float Offset = 1.5f;
    
    [SerializeField] 
    protected GameObject _projectilePrefab;
    
    
    private Rigidbody _projectileRigidBody;
    
    
    protected override void OnAbilityStarted()
    {
        base.OnAbilityStarted();
        Quaternion cameraRotation = Camera.main.transform.rotation;
        // Get spawn point in front of player in camera direction 
        _projectileRigidBody = GameObject.Instantiate(
            _projectilePrefab, _inventory.transform.position + new Vector3(0, 2, 0) + cameraRotation * Vector3.forward * Offset, cameraRotation, _inventory.transform).GetComponent<Rigidbody>();
        _projectileRigidBody.transform.parent = null;
        _projectileRigidBody.isKinematic = false;
        _projectileRigidBody.GetComponent<Collider>().isTrigger = false;
        Vector3 force = cameraRotation * Vector3.forward * strength;
        _projectileRigidBody.AddForce(force, ForceMode.Impulse);
    }
    
    protected override void ProcessEntities()
    {
        _projectileRigidBody.GetComponent<GEntityContainer>().BufferEntities(_inventory.agents.GetRange(0, _entityCost));
        base.ProcessEntities();
    }
}
