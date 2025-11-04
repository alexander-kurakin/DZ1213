using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    private Coin[] _coins;

    private void Awake()
    {
        _coins = GetComponentsInChildren<Coin>();
    }

    public int TotalCoins() => _coins.Length;

    public void ResetCoinsState()
    {
        foreach (Coin coin in _coins)
        {
            coin.gameObject.SetActive(true);
        }
    }
}
