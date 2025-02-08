using UnityEngine;
using System.Collections;
using System;

public class UnitMover : MonoBehaviour
{
    private const float DistanceToStop = 0.2f;

    [SerializeField] private float _speed;

    private float _initialSpeed;
    private Transform _transform;
    
    public event Action BaseReached;
    public event Action GoldReached;
    public event Action FlagReached;
    
    private void Awake()
    {
        _transform = transform;
        _initialSpeed = _speed;
    }

    public void Stop()
    {
        _speed = 0;
    }

    public void MoveToTarget(IUnitTarget unitTarget)
    {
        StartCoroutine(StartMoveToTarget(unitTarget));
    }

    private void Move(Vector3 direction)
    {
        _speed = _initialSpeed;
        Vector3 offset = direction.normalized * (_speed * Time.deltaTime);

        _transform.Translate(offset);
    }

    private IEnumerator StartMoveToTarget(IUnitTarget unitTarget)
    {
        Vector3 target = unitTarget.Transform.position;

        Vector3 direction = (target - transform.position);

        while (Vector2.SqrMagnitude(transform.position - target) > DistanceToStop)
        {
            Move(direction);
            yield return new WaitForEndOfFrame();
        }

        if (unitTarget is Base)
        {
            BaseReached?.Invoke();
        }
        else if (unitTarget is Gold)
        {
            GoldReached?.Invoke();
        }
        else if (unitTarget is Flag)
        {
            FlagReached?.Invoke();
        }
    }
}
