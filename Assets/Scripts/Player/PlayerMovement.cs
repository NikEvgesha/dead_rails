using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 100f;
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _damping = 0.995f;

    private PlayerInput _input;

    private CharacterController _controller;

    private Vector3 _velocity;

    private bool _onPlatform;


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
        
    }

    private void CameraRotation()
    {
        Vector2 rotation = _input.Rotation * _rotationSpeed * Time.deltaTime;

        transform.Rotate(0f, rotation.x, 0f, Space.World);
        transform.Rotate(-rotation.y, 0f, 0f, Space.Self);
    }

}
