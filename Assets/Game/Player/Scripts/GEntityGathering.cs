using System;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class GEntityGathering : MonoBehaviour
{
    private const float _gizmosOffsetHeight = .01f;
    
    [SerializeField] private float _gatheringRange;
    [SerializeField, BoxGroup("GIZMOS")] private bool _isGizmosActive = true;
    [SerializeField, BoxGroup("GIZMOS")] private int _gizmosCircleResolution;
    [SerializeField, BoxGroup("GIZMOS")] private Transform _gizmosGatherTransform;
    private GEntityInventory _inventory;
    private Vector3[] _gizmosCircleArraySize;
    
    private void Start()
    {
        _inventory = GetComponent<GEntityInventory>();
        _gizmosCircleArraySize = new Vector3[_gizmosCircleResolution];
    }

    private void Update()
    {
        AddEntitiesToInventory();
        if (_isGizmosActive && _gizmosCircleArraySize != null && _gizmosCircleArraySize.Length > 0)
        {
            Gizmos.color = Color.yellow;
            float angleStep = 360f / (_gizmosCircleResolution);
            for (int i = 0; i < _gizmosCircleResolution; i++)
            {
                Vector3 center = transform.position + Vector3.up * _gizmosOffsetHeight;
                float angle = i * angleStep * Mathf.Deg2Rad;
                float x = center.x + _gatheringRange * Mathf.Cos(angle);
                float z = center.z + _gatheringRange * Mathf.Sin(angle);
                _gizmosCircleArraySize[i] = new Vector3(x, center.y, z);
            }
           // Gizmos.DrawLineStrip(_gizmosCircleArraySize, true);
            _gizmosGatherTransform.localScale = Vector3.one * _gatheringRange * 12.5f;
        }
    }

    
    /*
    private void OnDrawGizmos()
    {
        if (_isGizmosActive && _gizmosCircleArraySize != null && _gizmosCircleArraySize.Length > 0)
        {
            Gizmos.color = Color.yellow;
            float angleStep = 360f / (_gizmosCircleResolution);
            for (int i = 0; i < _gizmosCircleResolution; i++)
            {
                Vector3 center = transform.position + Vector3.up * _gizmosOffsetHeight;
                float angle = i * angleStep * Mathf.Deg2Rad;
                float x = center.x + _gatheringRange * Mathf.Cos(angle);
                float z = center.z + _gatheringRange * Mathf.Sin(angle);
                _gizmosCircleArraySize[i] = new Vector3(x, center.y, z);
            }
            Gizmos.DrawLineStrip(_gizmosCircleArraySize, true);
            _gizmosGatherTransform.localScale = Vector3.one * _gatheringRange * 12.5f;
        }
    }
    */

    private void AddEntitiesToInventory()
    {
        foreach (var entity in GetNearEntities())
        {
            entity.ChangeState(GEntity.EEntityState.InInventory);
            _inventory.RegisterEntity(entity);
        }
    }
    
    private GEntity[] GetNearEntities()
    {
        GEntity[] entities = GEntityManager.Instance.agents.Where(a=> 
            a.gameObject.activeInHierarchy &&
            Vector3.Distance(transform.position, a.transform.position) <= _gatheringRange &&
            a._currentState != GEntity.EEntityState.InInventory).ToArray();

        return entities;
    }
    
}
