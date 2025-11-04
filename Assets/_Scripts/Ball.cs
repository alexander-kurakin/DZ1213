using System;
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
    [SerializeField] private Vector3 _particleSystemPositionOffset;

    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _jumpSpeed;
    [SerializeField] private float _jumpSoundRate;

    private Rigidbody _rigidBody;
    private AudioSource _audioSource;
    private KeyCode _jumpButton = KeyCode.Space;
    
    private Vector3 _input;
    private Vector3 _normalizedInput;
    private Vector3 _startPosition;
    private Wallet _wallet;

    private bool _isJumpKeyPressed = false;
    private bool _isGrounded = false;
    private bool _isMovementEnabled = true;
    private bool _isPhysicsEnabled = true;

    public bool MovementEnabled() => _isMovementEnabled;
    public bool PhysicsEnabled() => _isPhysicsEnabled;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _wallet = GetComponent<Wallet>();
    }

    private void Start()
    {
        SaveStartPosition();
    }

    private void Update()
    {
        if (_isMovementEnabled)
            HandlePhysicalMovement();
    }

    private void SaveStartPosition()
    {
        _startPosition = transform.position;
    }

    public void MoveToStartPosition()
    {
        transform.position = _startPosition;
    }

    public void EnableMovement()
    {
        _isMovementEnabled = true;
    }

    public void DisableMovement()
    {
        _isMovementEnabled = false;
    }

    public void EnablePhysics()
    {
        _rigidBody.isKinematic = false;
        _isPhysicsEnabled = true;
    }

    public void DisablePhysics()
    {
        _rigidBody.isKinematic = true;
        _isPhysicsEnabled = false;
    }

    public int GetBalance() 
    {
        return _wallet.Coins;    
    }

    public void ResetBalance()
    {
        _wallet.ResetCoins();
    }

    private void HandlePhysicalMovement()
    {
        if (Input.GetKeyDown(_jumpButton) && _isGrounded)
            _isJumpKeyPressed = true;

        _input = new Vector3(Input.GetAxis(HorizontalAxis), 0, Input.GetAxis(VerticalAxis));
        _normalizedInput = _input.normalized;
    }

    private void FixedUpdate()
    {
        if (_isMovementEnabled)
        {
          _rigidBody.AddForce(_normalizedInput * _rollSpeed);

            if (_isJumpKeyPressed && _isGrounded)
            {
                _rigidBody.AddForce(Vector3.up * _jumpSpeed, ForceMode.Impulse);
                _audioSource.PlayOneShot(_jumpEffectAudio);
                _isJumpKeyPressed = false;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Floor floor) == true)
        {
            PlayJumpEffectOnCollision(collision);
            _isGrounded = true;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Floor floor) == true)
            _isGrounded = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Floor floor) == true)
            _isGrounded = false;
    }

    private void PlayJumpEffectOnCollision(Collision collision)
    {
        _jumpParticleSystem.transform.position = collision.contacts[0].point + _particleSystemPositionOffset;
        _jumpParticleSystemTwo.transform.position = collision.contacts[0].point + _particleSystemPositionOffset;

        _jumpParticleSystem.Play();
        _jumpParticleSystemTwo.Play();
    }
}
