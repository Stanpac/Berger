using System;
using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class GPillarLogic : MonoBehaviour
{
    [SerializeField] private float _navMeshUpdateFrequency;
    private NavMeshSurface _navMesh;
    private IEnumerator Start()
    {
        _navMesh = GameObject.FindFirstObjectByType<NavMeshSurface>();
        float timer = 0;
        while (timer < 1)
        {
            timer += _navMeshUpdateFrequency;
            _navMesh.BuildNavMesh();
            yield return new WaitForSeconds(_navMeshUpdateFrequency);
        }
    }


}