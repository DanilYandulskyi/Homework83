using UnityEngine;
using System;

public class Flag : MonoBehaviour, IUnitTarget
{
    public Transform Transform => transform;

    public event Action OnDisable;

    public void Disable()
    {
        OnDisable?.Invoke();
        gameObject.SetActive(false);
    }
}
