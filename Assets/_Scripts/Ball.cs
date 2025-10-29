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
    private bool _isGrounded = false;

    private Vector3 _input;
    private Vector3 _normalizedInput;

    private int _coins;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(_jumpButton))
            _isJumpKeyPressed = true;

        _input = new Vector3(Input.GetAxis(HorizontalAxis), 0, Input.GetAxis(VerticalAxis));
        _normalizedInput = _input.normalized;

        Debug.Log("IsGrounded=" +  _isGrounded);
    }

    private void FixedUpdate()
    {

        _rigidBody.AddForce(_normalizedInput * _rollSpeed);

        if (_isJumpKeyPressed && _isGrounded) 
        {
            _rigidBody.AddForce(Vector3.up * _jumpSpeed, ForceMode.Impulse);
            _isJumpKeyPressed = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Floor>() != null)
            _isGrounded = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<Floor>() != null)
            _isGrounded = true;            
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<Floor>() != null)
            _isGrounded = false;
    }

    public void AddCoins(int value)
    {
        _coins += value;
        Debug.Log(_coins);
    }
}
