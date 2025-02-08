using UnityEngine;

public class FlagHandler : MonoBehaviour
{
    [SerializeField] private BaseSpawner _baseSpawner;
    [SerializeField] private FlagSpawner _flagSpawner;

    private Flag _spawnedFlag;

    public bool IsFlagSet { get; private set; } = false;
    public Flag Flag => _spawnedFlag;

    private void Awake()
    {
        _spawnedFlag.OnDisable += RemoveFlag;
    }

    private void OnDestroy()
    {
        _spawnedFlag.OnDisable -= RemoveFlag;
    }

    public void SetFlag(Vector3 position)
    {
        if (IsFlagSet)
        {
            SetFlagPosition(position);
        }
        else
        {
            IsFlagSet = true;
            _spawnedFlag = _flagSpawner.SpawnFlag(position);
        }
    }

    public void SetFlagPosition(Vector3 position)
    {
        _spawnedFlag.gameObject.SetActive(true);
        _spawnedFlag.transform.position = position;
    }

    private void RemoveFlag()
    {
        IsFlagSet = false;
    }
}
