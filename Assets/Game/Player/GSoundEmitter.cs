using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class GSoundEmitter : MonoBehaviour
{
    private const float _gizmosOffsetHeight = .01f;
    
    [SerializeField] private float _bellRange;
    [SerializeField] private LayerMask _bellDetectionMask;
    [SerializeField] private bool _isGizmosActive = true;
    [SerializeField] private int _gizmosCircleResolution;
    [FormerlySerializedAs("_gizmosAnimDuration")] [SerializeField] private float _bellTickDuration;
    [SerializeField] private AnimationCurve _gizmosAnimCurve;
    private float _gizmosAnimRadius;
    private Vector3[] _gizmosCircleArraySize;
    private IEnumerator _bellEnum;
    private bool _isBellActive;
    
    public void OnBellStarted()
    {
        if (_bellEnum != null)
        {
            StopCoroutine(_bellEnum);
        }
        _bellEnum = BellGizmosCoroutine();
        StartCoroutine(_bellEnum);
    }

    public void BellTick()
    {
        GSoundListener[] _listeners = GEntityManager.Instance.listeningAgents.Where(a=>
            (Vector3.Distance(transform.position, a.transform.position)) < _bellRange).ToArray();
        
        for (int i = 0; i < _listeners.Length; i++)
        {
            _listeners[i].HearSound(transform.position);
        }
    }

    public void OnBellReleased()
    {
        _isBellActive = false;
    }

    private void Start()
    {
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

        if (_isGizmosActive && _gizmosCircleArraySize != null && _bellEnum != null && _gizmosCircleArraySize.Length > 0)
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
        _isBellActive = true;
        while (_isBellActive)
        {
            BellTick();
            float i = 0;
            while (i < 1)
            {
                i += Time.deltaTime / _bellTickDuration;
                _gizmosAnimRadius = _bellRange * _gizmosAnimCurve.Evaluate(i);
                yield return null;
            }

            yield return null;
        }
        _bellEnum = null;
    }
}
