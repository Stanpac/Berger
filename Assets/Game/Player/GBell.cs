using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class GBell : MonoBehaviour
{
    private const float _gizmosOffsetHeight = .5f;
    
    [SerializeField] private float _bellRange;
    [SerializeField] private LayerMask _bellDetectionMask;
    [SerializeField] private bool _isGizmosActive = true;
    [SerializeField] private int _gizmosCircleResolution;
    [SerializeField] private float _gizmosAnimDuration;
    [SerializeField] private AnimationCurve _gizmosAnimCurve;
    private float _gizmosAnimRadius;
    private Vector3[] _gizmosCircleArraySize;
    private Collider[] _bellDetectionArray;
    private const int _bellDetectionArraySize = 100;
    private IEnumerator _bellGizmosEnum;
    
    public void OnTriggerBell()
    {
        if (_bellGizmosEnum != null)
        {
            StopCoroutine(_bellGizmosEnum);
        }
        _bellGizmosEnum = BellGizmosCoroutine();
        StartCoroutine(_bellGizmosEnum);
        
         int numOfDetected = Physics.OverlapSphereNonAlloc(transform.position, _bellRange, _bellDetectionArray, _bellDetectionMask, QueryTriggerInteraction.Ignore);
         for (int i = 0; i < numOfDetected; i++)
         {
             if (_bellDetectionArray[i].TryGetComponent<GBell>(out GBell bell))
             {
                 // Call Callback

             }
         }
    }

    private void Start()
    {
        _bellDetectionArray = new Collider[_bellDetectionArraySize];
        _gizmosCircleArraySize = new Vector3[_gizmosCircleResolution];
    }

    private void OnDrawGizmos()
    {
        if (_isGizmosActive && _gizmosCircleArraySize != null && _gizmosCircleArraySize.Length > 0)
        {
            float angleStep = 360f / (_gizmosCircleResolution);
            for (int i = 0; i < _gizmosCircleResolution; i++)
            {
                Vector3 center = transform.position + Vector3.up * _gizmosOffsetHeight;
                float angle = i * angleStep * Mathf.Deg2Rad;
                float x = center.x + _bellRange * Mathf.Cos(angle);
                float z = center.z + _bellRange * Mathf.Sin(angle);
                _gizmosCircleArraySize[i] = new Vector3(x, center.y, z);
            }
            Gizmos.DrawLineStrip(_gizmosCircleArraySize, true);
        }

        if (_isGizmosActive && _gizmosCircleArraySize != null && _bellGizmosEnum != null && _gizmosCircleArraySize.Length > 0)
        {
            float angleStep = 360f / (_gizmosCircleResolution);
            for (int i = 0; i < _gizmosCircleResolution; i++)
            {
                Vector3 center = transform.position + Vector3.up * _gizmosOffsetHeight;
                float angle = i * angleStep * Mathf.Deg2Rad;
                float x = center.x + _gizmosAnimRadius * Mathf.Cos(angle);
                float z = center.z + _gizmosAnimRadius * Mathf.Sin(angle);
                _gizmosCircleArraySize[i] = new Vector3(x, center.y, z);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawLineStrip(_gizmosCircleArraySize, true);
        }
    }

    IEnumerator BellGizmosCoroutine()
    {
        float i = 0;
        while (i < 1)
        {
            i += Time.deltaTime / _gizmosAnimDuration;
            _gizmosAnimRadius = _bellRange * _gizmosAnimCurve.Evaluate(i);
            yield return null;
        }

        _bellGizmosEnum = null;
    }
}
