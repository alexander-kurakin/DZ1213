using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int Coins {  get; private set; }

    public void AddCoin() => Coins++;

    public void ResetCoins() => Coins = 0;
}
