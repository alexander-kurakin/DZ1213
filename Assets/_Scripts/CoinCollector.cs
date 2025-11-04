using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField] private Wallet _wallet;

    [SerializeField] private AudioClip[] _coinCollectSounds;
    [SerializeField] private ParticleSystem _coinCollectParticleSystem;
    [SerializeField] private float _particleSystemYminOffset;
    [SerializeField] private float _particleSystemYmaxOffset;

    private AudioSource _ballAudioSource;

    private void Awake()
    {
        _ballAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Coin coin) == false)
            return;

        _wallet.AddCoin();

        PlayCollectSound();
        PlayCollectParticle();

        coin.gameObject.SetActive(false);
    }

    private void PlayCollectSound()
    {
        _ballAudioSource.PlayOneShot(_coinCollectSounds[Random.Range(0, _coinCollectSounds.Length)]);
    }

    private void PlayCollectParticle()
    {
        if (_coinCollectParticleSystem.gameObject.activeSelf == false)
            _coinCollectParticleSystem.gameObject.SetActive(true);

        _coinCollectParticleSystem.transform.position = transform.position + new Vector3(0, Random.Range(_particleSystemYminOffset, _particleSystemYmaxOffset), 0);
        _coinCollectParticleSystem.Play();
    }

}
