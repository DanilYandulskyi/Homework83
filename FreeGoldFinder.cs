using UnityEngine;
using System.Collections.Generic;

public class FreeGoldFinder : MonoBehaviour
{
    [SerializeField] private Scanner _scanner;
    
    private List<Gold> _takenGoldList;

    private void Awake()
    {
        _scanner.NewGoldFound += AddNewGold;
    }

    private void OnDestroy()
    {
        _scanner.NewGoldFound -= AddNewGold;
    }

    public Gold GetFreeGold()
    {
        foreach (var gold in _scanner.GoldList)
        {
            if (_takenGoldList.Contains(gold) == false)
            {
                _takenGoldList.Add(gold);
                _scanner.RemoveGold(gold);
            }

            return gold;
        }

        return null;
    }

    private void AddNewGold(Gold gold)
    {
        bool succses = false;
        
        foreach (var _gold in _scanner.GoldList)
        {
            if (_gold != gold)
            {
                succses = true;
                break;
            }
        }

        if (succses)
        {
            _scanner.AddGold(gold);
        }
    }
}
