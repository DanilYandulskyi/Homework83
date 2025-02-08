using System;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnitMover))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Gold _gold;
    [SerializeField] private Flag _flag;
    [SerializeField] private bool _isStanding = true;
    [SerializeField] private BaseSpawner _baseSpawner;

    private IUnitTarget _initialPosition;
    private IUnitTarget _target;
    private UnitMover _mover;

    public event Action<Gold> CollectedResource;

    public bool IsResourceCollected { get; private set; } = false;
    public int Price { get; private set; } = 3;
    public bool IsStanding => _isStanding;

    public Vector3 InitialPosition => _initialPosition.Transform.position;

    private void Awake()
    {
        _mover = GetComponent<UnitMover>();

        _mover.BaseReached += CollectGold;
        _mover.GoldReached += ReachGold;
        _mover.FlagReached += ReachFlag;
    }

    private void OnDisable()
    {
        _mover.BaseReached -= CollectGold;
        _mover.GoldReached -= ReachGold;
        _mover.FlagReached -= ReachFlag;
    }

    private void Update()
    {
        if (_gold != null)
        {
            if (_gold.isActiveAndEnabled == false)
            {
                _target = null;
                _gold = null;
                _isStanding = true;
            }
        }
    }

    public Unit Initialize(BaseSpawner baseSpawner)
    {
        _baseSpawner = baseSpawner;
        _initialPosition = _baseSpawner.InitialUnitPosition;

        return this;
    }

    public void SetTarget(IUnitTarget unitTarget)
    {
        _target = unitTarget;

        _mover.MoveToTarget(_target);
        _isStanding = false;
    }

    public void SetInitialPosition()
    {
        _target = _initialPosition;
    }

    public void Stop(float time)
    {
        StartCoroutine(StopForSeconds(time));
    }

    private void ReachGold()
    {
        _gold = (Gold)_target;

        IsResourceCollected = true;
        _mover.MoveToTarget(_initialPosition);
        _gold.StartFollow(transform);
    }

    private void ReachFlag()
    {
        _flag = (Flag)_target;

        _baseSpawner.SpawnBase(_flag.transform.position).Assign(this);
        _flag.Disable();
        _target = null;
        _flag = null;
        _isStanding = true;
    }

    private IEnumerator StopForSeconds(float time)
    {
        _isStanding = false;

        yield return new WaitForSeconds(time);

        _target = null;
        _gold = null;
        _isStanding = true;
    }

    private void CollectGold()
    {
        CollectedResource?.Invoke(_gold);
        _mover.Stop();
        IsResourceCollected = false;
        _isStanding = true;
        _gold.Disable();
        _gold.StopFollow();
        _gold = null;
        _target = null;
    }
}