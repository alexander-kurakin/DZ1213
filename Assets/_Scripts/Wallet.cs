using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    public int Coins {  get; private set; }

    public void AddCoin() => Coins++;

}
