using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _damping = 0.995f;
    [SerializeField] private float _jumpPower = 5;
    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private float _fallSpeed = 1;

    [SerializeField] private bool _onPlatform;

    private PlayerInput _input;

    private CharacterController _controller;

    private Vector3 _velocity;
    private Vector3 _moveDirection;



    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInput>();
    }

    void Update()
    {
        if (_onPlatform)
        {
            PlatformMove();
        } else
        {
            SpaceMove();
        }
        
        CameraRotation();
    }

    private void SpaceMove()
    {
        Vector3 movement = _input.Movement;

        if (_input.SpaceUp)
        {
            movement.y = 1f;
        }
        else if (_input.SpaceDown)
        {
            movement.y = -1f;
        }

        if (movement.magnitude > 0f)
        {
            Vector3 acceleration = transform.TransformDirection(movement) * _moveSpeed;
            _velocity += acceleration * Time.deltaTime;
        }

        _velocity *= _damping;

        if (_velocity.magnitude > _maxSpeed)
        {
            _velocity = _velocity.normalized * _maxSpeed;
        }

        _controller.Move(_velocity * Time.deltaTime);
    }


    private void PlatformMove()
    {
        Vector3 movement = _input.Movement;
        Vector3 moveDirection = transform.TransformDirection(movement);
        moveDirection.y = 0f;
        moveDirection = moveDirection.normalized;

        Vector3 horizontalMovement = moveDirection * _moveSpeed;

        float verticalMovement = 0f;

        if (_controller.isGrounded)
        {
            _velocity.y = -0.1f;

            if (_input.SpaceUp)
            {
                _velocity.y = _jumpPower;
            }
        }
        else
        {
            _velocity.y -= _gravity * Time.deltaTime;
        }
        Vector3 finalMovement = horizontalMovement + new Vector3(0f, _velocity.y, 0f);
        _controller.Move(finalMovement * Time.deltaTime);
    }

    private void CameraRotation()
    {
        Vector2 rotation = _input.Rotation * _rotationSpeed * Time.deltaTime;

        transform.Rotate(0f, rotation.x, 0f, Space.World);
        transform.Rotate(-rotation.y, 0f, 0f, Space.Self);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "GravityPlatform")
        {
            Debug.Log("Gravity enter");
            _onPlatform = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "GravityPlatform")
        {
            Debug.Log("Gravity exit");
            _onPlatform = false;
        }
    }

}
