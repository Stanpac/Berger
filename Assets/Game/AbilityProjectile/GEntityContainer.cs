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

    public void ReleaseContainer()
    {
        if (_bufferedEntities != null && _bufferedEntities.Count > 0)
        {
            foreach (var entity in _bufferedEntities)
            {
                entity.transform.position = transform.position;
                entity.gameObject.SetActive(true);
                entity.ChangeState(GEntity.EEntityState.Passive);
            }
        }
        Destroy(gameObject);
    }

}