using UnityEngine;

public class GInteractable : MonoBehaviour
{
    [SerializeField] private GPlayerAbility _ability;

    public GPlayerAbility GetAbility() => _ability;
}
