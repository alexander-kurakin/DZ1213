using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private string HorizontalAxis = "Horizontal";
    private KeyCode _jumpButton = KeyCode.Space;
    private Rigidbody _rigidBody;

    [SerializeField] private float _force;
    [SerializeField] private float _rotationForce;

    private float _xInput;
    private bool _isJumping;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        _xInput = Input.GetAxis(HorizontalAxis);
        _isJumping = Input.GetKey(_jumpButton);
    }

    private void FixedUpdate()
    {
        if (_isJumping)
            _rigidBody.AddForce(transform.up * _force);
    }
}
