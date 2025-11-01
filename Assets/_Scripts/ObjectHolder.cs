using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHolder : MonoBehaviour
{
    private Coin[] _coins;

    private void Awake()
    {
        _coins = GetComponentsInChildren<Coin>();
    }

    public int TotalCoins() => _coins.Length;

    public int ActiveCoins() 
    {
        int _activeCoinsCounter = 0;
        
        foreach (Coin coin in _coins)
            if (coin.gameObject.activeSelf == true)
                _activeCoinsCounter++;

        return _activeCoinsCounter;
    }
}
