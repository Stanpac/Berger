using System;
using System.Collections.Generic;
using UnityEngine;

public class GEntityContainer : MonoBehaviour
{
    private List<GEntity> _bufferedEntities;

    public void BufferEntities(List<GEntity> bufferedEntities)
    {
        _bufferedEntities = bufferedEntities;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        foreach (var entity in _bufferedEntities)
        {
            entity.transform.position = transform.position;
            entity.gameObject.SetActive(true);
            entity.ChangeState(GEntity.EEntityState.Passive);
        }
        Destroy(gameObject);
    }
}
