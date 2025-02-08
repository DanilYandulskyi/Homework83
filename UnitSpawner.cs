using UnityEngine;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Unit _unit;
    [SerializeField] private BaseSpawner _baseSpawner;

    public Unit SpawnUnit(Vector3 position)
    {
        return Instantiate(_unit, position, Quaternion.identity).Initialize(_baseSpawner);
    }
}
