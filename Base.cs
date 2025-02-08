using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class Base : MonoBehaviour, IUnitTarget
{
    [SerializeField] private List<Unit> _units = new List<Unit>();
    [SerializeField] private List<Gold> _collectedGold = new List<Gold>();

    [SerializeField] private FreeGoldFinder _freeGoldFinder;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private Raycaster _raycaster;
    [SerializeField] private ClickProcessor _clickProcessor;
    [SerializeField] private FlagHandler _flagHandler;

    [SerializeField] private int _price;

    private bool _isSelected;
    private bool _isFlagTaken = false;

    public Transform Transform => transform;

    public event UnityAction<int> GoldAmountChanged;

    private void Start()
    {
        _clickProcessor.OnClicked += SetFlag;

        foreach (Unit unit in _units)
        {
            unit.CollectedResource += CollectResource;
        }
    }

    private void LateUpdate()
    {
        if (CanBuildNewBase())
        {
            TrySendUnitToFlag();
        }

        for (int i = 0; i < _units.Count; i++)
        {
            if (_units[i].IsStanding)
            {
                Gold gold = _freeGoldFinder.GetFreeGold();

                if (gold != null)
                {
                    _units[i].SetTarget(gold);
                    break;
                }
            }
        }
    }

    private void OnDestroy()
    {
        _clickProcessor.OnClicked -= SetFlag;

        foreach (Unit unit in _units)
        {
            unit.CollectedResource -= CollectResource;
        }
    }

    private void OnMouseDown()
    {
        if (_isSelected == false)
        {
            _isSelected = true;
        }
    }

    public Base Initialize(FreeGoldFinder freeGoldFinder, UnitSpawner unitSpawner, FlagHandler flagHandler, Raycaster raycaster, ClickProcessor clickProcessor)
    {
        _freeGoldFinder = freeGoldFinder;
        _unitSpawner = unitSpawner;
        _flagHandler = flagHandler;
        _raycaster = raycaster;
        _clickProcessor = clickProcessor;

        return this;
    }

    public void Assign(Unit unit)
    {
        _units.Add(unit);

        unit.SetInitialPosition();
    }

    public void SetFlag()
    {
        if (_isSelected)
        {
            Vector3 flagSetPosition;

            if (_raycaster.Cast(Input.mousePosition, out flagSetPosition))
            {
                _flagHandler.SetFlag(new Vector3(flagSetPosition.x, flagSetPosition.y, flagSetPosition.z));
                _isSelected = false;
            }
        }
    }

    private void CollectResource(Gold gold)
    {
        _collectedGold.Add(gold);

        TryAddNewUnit();

        GoldAmountChanged?.Invoke(_collectedGold.Count);
    }

    private void TryAddNewUnit()
    {
        if (_collectedGold.Count >= _units[0].Price)
        {   
            if (_units.Count == 1 || _flagHandler.IsFlagSet == false)
            {
                int unitStopTime = 1;
                Vector3 unitSpawningPositionOffset = new Vector3(2, 0, 0);

                Unit unit = _unitSpawner.SpawnUnit(_units[_units.Count - 1].InitialPosition + unitSpawningPositionOffset);

                unit.Stop(unitStopTime);

                _units.Add(unit);

                unit.CollectedResource += CollectResource;

                for (int i = 0; i < _units[0].Price; i++)
                {
                    _collectedGold.Remove(_collectedGold[_collectedGold.Count - 1]);
                }
            }
        }
    }

    private bool CanBuildNewBase()
    {
        int minUnitAmount = 1;

        return _collectedGold.Count >= _price && _flagHandler.IsFlagSet && _isFlagTaken == false && _units.Count > minUnitAmount;
    }

    private void TrySendUnitToFlag()
    {
        for (int i = 0; i < _units.Count; i++)
        {
            Unit unit = _units[i];

            if (unit.IsStanding)
            {
                unit.CollectedResource -= CollectResource;

                unit.SetTarget(_flagHandler.Flag);
                _isFlagTaken = true;

                _units.Remove(unit);

                for (int j = 0; j < _price; j++)
                {
                    _collectedGold.Remove(_collectedGold[_collectedGold.Count - 1]);
                }

                break;
            }
        }
    }
}
