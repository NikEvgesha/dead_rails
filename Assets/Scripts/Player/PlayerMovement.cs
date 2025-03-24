using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(GravityChecker))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject _camera;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _damping = 0.995f;
    [SerializeField] private float _jumpPower = 5;
    [SerializeField] private float _gravity = 9.8f;
    [SerializeField] private float _fallSpeed = 1;
    [SerializeField] private float _spaceSpeedMultiplier = 2f;
    [SerializeField] private float _verticalSpaceSpeed = 2f;

    [SerializeField] private float _YRotationLimitMax = 80f;
    [SerializeField] private float _YRotationLimitMin = -80f;

    [SerializeField] private bool _onPlatform;
    [SerializeField] private bool _isGrounded;

    private PlayerInput _input;

    private CharacterController _controller;

    private Vector3 _velocity;
    private Vector3 _moveDirection;
    private float _currentXRotation = 0f;
    private float _currentYRotation = 0f;
    private ControlUI _controlUI;
    private int _gravitySourceCounter = 0;
    private GravityChecker _gravityChecker;

    private HashSet<Collider> _gravityPlatforms = new HashSet<Collider>();

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _input = GetComponent<PlayerInput>();
        _gravityChecker = GetComponent<GravityChecker>();
        _controlUI = FindAnyObjectByType<ControlUI>();
        _controlUI.UseMobileSetup(_input.UseTouchControl);

        _gravityChecker.GravityChanged += OnGravityChanged;
    }


    private void OnDisable()
    {
        _gravityChecker.GravityChanged -= OnGravityChanged;
    }

    void Update()
    {
        _isGrounded = _controller.isGrounded;
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
            _velocity.y = _verticalSpaceSpeed;
        }
        else if (_input.SpaceDown)
        {
            _velocity.y = -_verticalSpaceSpeed;
        }

        Vector3 cameraForward = _camera.transform.forward;
        Vector3 cameraRight = _camera.transform.right;


        Vector3 horizontalMovement = (cameraForward * movement.z + cameraRight * movement.x).normalized;


        if (movement.magnitude > 0f)
        {
            Vector3 acceleration = horizontalMovement * _moveSpeed * _spaceSpeedMultiplier;
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
        //moveDirection = moveDirection.normalized;

        Vector3 horizontalMovement = moveDirection * _moveSpeed;


        if (_controller.isGrounded)
        {
            _velocity.y = -0.5f;

            if (_input.JumpTriggered)
            {
                Debug.Log("Jump triggered");
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
        Vector2 rotationInput = _input.Rotation * _rotationSpeed * Time.deltaTime;

        _currentYRotation += rotationInput.x;

        _currentXRotation -= rotationInput.y;
        _currentXRotation = Mathf.Clamp(_currentXRotation, _YRotationLimitMin, _YRotationLimitMax);
        transform.rotation = Quaternion.Euler(0f, _currentYRotation, 0f);
        _camera.transform.localRotation = Quaternion.Euler(_currentXRotation, 0f, 0f);
    }

    private void OnGravityChanged(bool inGravitySource)
    {
        _onPlatform = inGravitySource;
        _controlUI.SwitchPlatformControls(_onPlatform);
    }

}
