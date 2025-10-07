using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "Player Throw Ability", menuName = "Ability/ThrowAbility")]
public class GPlayerAbility_Throw : GPlayerAbility
{
    [SerializeField] protected float _throwStrength;
    [SerializeField] protected float _timeToReachMaxThrowDistance;
    [SerializeField] protected float _throwAngle;
    [SerializeField] protected GameObject _projectilePrefab;
    [SerializeField] protected Vector3 _projectileSpawnPositionOffset;
    [SerializeField] protected LayerMask _projectileLayer;
    [SerializeField] protected int _lineResolution;
    private Rigidbody _projectileRigidBody;
    private PlayerInput _playerInput;
    private LineRenderer _lineRenderer;
    

    public override void OnStart(GPlayerAbilitySystem player)
    {
        base.OnStart(player);
        _lineRenderer = player.GetComponent<LineRenderer>();
        _playerInput = player.GetComponent<PlayerInput>();
    }
    
    protected override void OnAbilityStarted()
    {
        base.OnAbilityStarted();
        _projectileRigidBody = GameObject.Instantiate(
            _projectilePrefab, _inventory.transform.position + _projectileSpawnPositionOffset, _inventory.transform.rotation).GetComponent<Rigidbody>();
    }

    protected override void ProcessEntities()
    {
        _projectileRigidBody.GetComponent<GEntityContainer>().BufferEntities(_inventory.agents.GetRange(0, _entityCost));
        base.ProcessEntities();
    }
    
    protected override void OnAbilityTick()
    {
        base.OnAbilityTick();
        AbilityPreview();
    }

    protected override void OnAbilityEnded()
    {
        base.OnAbilityEnded();
        ApplyForce();
    }

    protected virtual void AbilityPreview()
    {
        Vector3 initialVelocity = GetThrowVector(out Vector3 hozizontalDir);

        float objectMass = _projectileRigidBody.mass;
        Vector3 velocity = initialVelocity / objectMass;

        List<Vector3> points = new List<Vector3>();
        Vector3 startPos = _projectileRigidBody.transform.position;
        points.Add(startPos); // Add starting position
        Vector3 currentPosition = startPos;
        float deltaTime = Time.fixedDeltaTime / _lineResolution;

        for (int i = 1; i < 1000; i++)
        {
            velocity += Physics.gravity * deltaTime;
            velocity *= 1.0f / (1.0f + deltaTime * _projectileRigidBody.linearDamping);
            Vector3 newPosition = currentPosition + velocity * deltaTime;
            Vector3 direction = (newPosition - currentPosition).normalized;
            float distance = Vector3.Distance(newPosition, currentPosition);

            RaycastHit[] hits = Physics.RaycastAll(points[i - 1], direction, distance, _projectileLayer,
                QueryTriggerInteraction.UseGlobal);

            points.Add(newPosition);
            currentPosition = newPosition;
            if (points.Count > 1)
            {
                Debug.DrawLine(points.Last(), points[points.Count - 2], Color.black, 2);
            }
        }
        _lineRenderer.positionCount = points.Count;
        _lineRenderer.SetPositions(points.ToArray());
    }

    protected Vector3 GetThrowVector(out Vector3 direction)
    {
        direction = _projectileRigidBody.transform.forward;
        float angleInRadians = _throwAngle * Mathf.Deg2Rad;

        if (_playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
            if (!Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _projectileLayer, QueryTriggerInteraction.UseGlobal))
            {
                Debug.LogError("Mouse raycast hit nothing");
                return Vector3.zero;
            }
            Vector3 worldPos = hitInfo.point;
            worldPos.y = 0;
            Vector3 playerPos = _inventory.transform.position;
            playerPos.y = 0;
            direction = (worldPos - playerPos).normalized;
        }

        float horizontalForce = _throwStrength * Mathf.Cos(angleInRadians);
        float verticalForce = _throwStrength * Mathf.Sin(angleInRadians);
        return (direction * horizontalForce) + (Vector3.up * verticalForce);
    }
    
    protected virtual void ApplyForce()
    {
        _lineRenderer.positionCount = 0;
        _projectileRigidBody.isKinematic = false;
        _projectileRigidBody.GetComponent<Collider>().isTrigger = false;
         Vector3 force = GetThrowVector(out var vel);
         _projectileRigidBody.AddForce(force, ForceMode.Impulse);
    }
}