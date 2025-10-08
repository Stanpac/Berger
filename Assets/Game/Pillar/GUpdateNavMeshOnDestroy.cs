using Unity.AI.Navigation;
using UnityEngine;

public class GUpdateNavMeshOnDestroy : MonoBehaviour
{
    private void OnDestroy()
    {
        var navMesh = GameObject.FindFirstObjectByType<NavMeshSurface>();
        navMesh.UpdateNavMesh(navMesh.navMeshData);
    }
}