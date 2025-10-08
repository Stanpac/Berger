using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

[RequireComponent(typeof(GEntityContainer))]
public class GEntityRelease : MonoBehaviour
{
    public enum EEntityReleaseType {OnCollision, WaitTime, None}
    [SerializeField] private EEntityReleaseType _entityReleaseType;
    [SerializeField, ShowIf("_entityReleaseType", EEntityReleaseType.WaitTime)] private float _timeToRelease;
    private GEntityContainer _entityContainer;

    private void Start()
    {
        _entityContainer = GetComponent<GEntityContainer>();
        if (_entityReleaseType == EEntityReleaseType.WaitTime)
        {
            StartCoroutine(WaitForReleaseCoroutine());
        }
    }

    IEnumerator WaitForReleaseCoroutine()
    {
        yield return new WaitForSeconds(_timeToRelease);
        _entityContainer.ReleaseContainer();
    }
    
    
    private void OnCollisionEnter(Collision other)
    {
        if (_entityReleaseType == EEntityReleaseType.OnCollision)
        {
            _entityContainer.ReleaseContainer();
        }
    }
    
}