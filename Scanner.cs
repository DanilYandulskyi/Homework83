using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

public class Scanner : MonoBehaviour
{
    [SerializeField] private List<Gold> _goldList = new List<Gold>();
    [SerializeField] private float _scanDelay;
    [SerializeField] private float _scanRadius;
    [SerializeField] private LayerMask _mask;

    public event Action<Gold> NewGoldFound;

    public IReadOnlyCollection<Gold> GoldList => _goldList;

    private void Start()
    {
        StartCoroutine(Scan());
    }

    public void RemoveGold(Gold gold)
    {
        if (_goldList.Contains(gold))
        {
            _goldList.Remove(gold);
        }
    }
    
    public void AddGold(Gold gold)
    {
        _goldList.Add(gold);
    }

    private IEnumerator Scan()
    {
        WaitForSeconds waiter = new WaitForSeconds(_scanDelay);

        while (enabled)
        {
            yield return waiter;

            Collider[] colliders = Physics.OverlapSphere(transform.position, _scanRadius, _mask);

            foreach (var collider in colliders)
            {
                if (collider.TryGetComponent(out Gold gold))
                {
                    NewGoldFound?.Invoke(gold);
                }
            }
        }
    }
}
