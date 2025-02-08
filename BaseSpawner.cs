using UnityEngine;

public class BaseSpawner : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private FreeGoldFinder _freeGoldFinder;
    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private Raycaster _raycaster;
    [SerializeField] private ClickProcessor _clickProcessor;
    [SerializeField] private FlagHandler _flagHandler;
    [SerializeField] private GoldUIViewSpawner _goldUIViewSpawner;

    public IUnitTarget InitialUnitPosition => _base;

    public Base SpawnBase(Vector3 position)
    {   
        Base @base = Instantiate(_base, position, Quaternion.identity);
        @base.Initialize(_freeGoldFinder, _unitSpawner, _flagHandler, _raycaster, _clickProcessor);

        _goldUIViewSpawner.Spawn(@base);

        return @base;
    }
}
