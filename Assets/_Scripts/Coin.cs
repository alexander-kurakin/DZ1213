using UnityEngine;

public class Coin : MonoBehaviour
{
    public void Collect() 
    { 
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        gameObject.SetActive(true);
    }
}
