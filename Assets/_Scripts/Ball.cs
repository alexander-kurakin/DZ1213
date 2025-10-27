using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private string HorizontalAxis = "Horizontal";
    private string VerticalAxis = "Vertical";

    private KeyCode _jumpButton = KeyCode.Space;
    private Rigidbody _rigidBody;

    [SerializeField] private float _rollSpeed;
    [SerializeField] private float _jumpSpeed;

    private bool _isJumpKeyPressed;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_jumpButton))
            _isJumpKeyPressed = true;
    }

    private void FixedUpdate()
    {
        Vector3 input = new Vector3(Input.GetAxis(HorizontalAxis), 0, Input.GetAxis(VerticalAxis));
        Vector3 normalizedInput = input.normalized;

        _rigidBody.AddForce(normalizedInput * _rollSpeed);

        if (_isJumpKeyPressed) 
        {
            _rigidBody.AddForce(Vector3.up * _jumpSpeed, ForceMode.Impulse);
            _isJumpKeyPressed = false;
        }

    }
}
