using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private const string HorizontalAxis = "Horizontal";
    private const string VerticalAxis = "Vertical";

    [SerializeField] private ParticleSystem _jumpParticleSystem;
    [SerializeField] private ParticleSystem _jumpParticleSystemTwo;
    [SerializeField] private AudioClip _jumpEffectAudio;
    [SerializeField] private AudioClip _coinPickupAudio;
    [SerializeField] private AudioClip _winAudio;
    [SerializeField] private AudioClip _loseAudio;
    [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
    [SerializeField] private Vector3 _particleSystemPositionOffset;
    [SerializeField] private Coin[] _coinsGameObjects;
    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _targetTime;

    private Rigidbody _rigidBody;
    private AudioSource _audioSource;
    private KeyCode _jumpButton = KeyCode.Space;
    private KeyCode _restartKey = KeyCode.R;
    private Vector3 _input;
    private Vector3 _normalizedInput;
    private Vector3 _startPosition;

    private bool _isJumpKeyPressed = false;
    private bool _isGrounded = false;
    private bool _isSoundPlaying = false;
    private bool _isStillPlaying = true;
    private float _currentTime;
    private int _currentCoins;
    private int _totalCoins;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _totalCoins = FindObjectsOfType<Coin>().Length;
    }

    private void Start()
    {
        ResetGameplayTimer();
        _startPosition = transform.position;
        DisplayCurrentTime();
    }

    private void Update()
    {

        if (_isStillPlaying == false && Input.GetKeyDown(_restartKey))
        {
            Restart();
            return;
        }

        if (_currentTime <= 0)
        {
            Lose();
            return;
        }

        if (_currentCoins == _totalCoins)
        {
            Win();
            return;
        }

        if (_isStillPlaying)
        {
            _currentTime -= Time.deltaTime;
            DisplayCurrentTime();

            HandlePhysicalMovement();
        }
    }

    private void ResetGameplayTimer()
    {
        _currentTime = _targetTime;
    }

    private void ResetCoins()
    {
        foreach (Coin coin in _coinsGameObjects) 
        {
            coin.gameObject.SetActive(true);
        }
            
    }

    private void HandlePhysicalMovement()
    {
        if (Input.GetKeyDown(_jumpButton) && _isGrounded)
            _isJumpKeyPressed = true;

        _input = new Vector3(Input.GetAxis(HorizontalAxis), 0, Input.GetAxis(VerticalAxis));
        _normalizedInput = _input.normalized;
    }

    private void Win()
    {
        StopGameplay(_winAudio, $"Отличная работа! Жмите {_restartKey} если хотите повторить!");
    }

    private void Lose()
    {
        StopGameplay(_loseAudio, $"Вы не успели! Жмите {_restartKey} и погнали снова!");
    }

    private void StopGameplay(AudioClip clipToPlay, String textToShow) 
    {
        Time.timeScale = 0.0f;
        PlaySingleTime(clipToPlay);
        _isStillPlaying = false;

        Debug.Log(textToShow);
    }

    private void Restart()
    {
        _isSoundPlaying = false;

        _rigidBody.isKinematic = true;

        ResetGameplayTimer();
        _currentCoins = 0;
        ResetCoins();

        transform.position = _startPosition;
        Time.timeScale = 1.0f;

        _rigidBody.isKinematic = false;

        _isStillPlaying = true;

        
    }
    private void DisplayCurrentTime()
    {
        _textMeshProUGUI.text = _currentTime.ToString("0.00");
    }

    private void FixedUpdate()
    {
        if (_isStillPlaying)
        {
          _rigidBody.AddForce(_normalizedInput * _rollSpeed);

            if (_isJumpKeyPressed && _isGrounded)
            {
                _rigidBody.AddForce(Vector3.up * _jumpSpeed, ForceMode.Impulse);
                _isJumpKeyPressed = false;
            }
        }
    }

    public void AddCoin()
    {
        _currentCoins++;
        _audioSource.PlayOneShot(_coinPickupAudio);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Floor floor = collision.gameObject.GetComponent<Floor>();

        if (floor != null)
        {
            PlayJumpEffectOnCollision(collision);
            _isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Floor floor = collision.gameObject.GetComponent<Floor>();
        
        if (floor != null)
            _isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        Floor floor = collision.gameObject.GetComponent<Floor>();

        if (floor != null)
            _isGrounded = false;
    }

    private void PlayJumpEffectOnCollision(Collision collision)
    {
        _jumpParticleSystem.transform.position = collision.contacts[0].point + _particleSystemPositionOffset;
        _jumpParticleSystemTwo.transform.position = collision.contacts[0].point + _particleSystemPositionOffset;

        _jumpParticleSystem.Play();
        _jumpParticleSystemTwo.Play();

        _audioSource.PlayOneShot(_jumpEffectAudio);
    }

    private void PlaySingleTime(AudioClip audioClip)
    {
        if (_isSoundPlaying == false)
        {
            _audioSource.PlayOneShot(audioClip);
            _isSoundPlaying = true;
        }
    }
}
