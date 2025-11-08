using UnityEngine;

public class CoinPrefabsHandler : MonoBehaviour
{
    private Coin[] _coinPrefabs;

    private void Awake()
    {
        _coinPrefabs = GetComponentsInChildren<Coin>();
    }

    public int TotalCoins() => _coinPrefabs.Length;

    public void ResetCoinsState()
    {
        foreach (Coin coin in _coinPrefabs)
        {
            coin.Activate();
        }
    }
}
