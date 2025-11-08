using UnityEngine;

public class Rotator : MonoBehaviour
{
    private int _firstSide = 1;
    private int _secondSide = -1;

    [SerializeField] private float _rotateSpeed = 60;

    private int _currentSide;

    private void Awake()
    {
        _currentSide = DetermineRotationSide();
    }

    private int DetermineRotationSide()
    {
        int chance = Random.Range(0, 2);

        return chance == 0 ? _firstSide : _secondSide;
    }

    private void Update()
    {
        transform.Rotate(Vector3.up * _currentSide * _rotateSpeed * Time.deltaTime);
    }
}
