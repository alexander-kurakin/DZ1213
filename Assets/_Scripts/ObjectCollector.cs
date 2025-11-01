using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollector : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Coin coin) == false)
            return;

        _wallet.AddCoin();

        Destroy(coin.gameObject);
    }
}
