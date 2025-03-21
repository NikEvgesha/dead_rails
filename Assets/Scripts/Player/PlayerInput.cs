using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private bool _useTouchControls = false;

    [Header("Touch Controls")]
    [SerializeField] private OnScreenButton _SpaceUpButton;
    [SerializeField] private OnScreenButton _SpaceDownButton;
    [SerializeField] private OnScreenButton _JumpButton;

    [SerializeField] private OnScreenJoystick _MoveJoystick;
    [SerializeField] private CameraTouchController _cameraTouchController;
    //[SerializeField] private OnScreenButton _SpaceButton;

    public bool UseTouchControl { get { return _useTouchControls; } private set { } }
    public bool IsCursorVisible { get; private set; }
    public bool SpaceUp { get; private set; }
    public bool SpaceDown { get; private set; }

    public Vector3 Movement { get; private set; }
    public Vector2 Rotation { get; private set; }

    public bool JumpTriggered
    {
        get
        {
            var tmp = _jump;
            _jump = false;
            return tmp;
        }
        private set { }
    }


    private bool _jump;

/*    private void Awake()
    {
        IsCursorVisible = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }*/


    private void Update()
    {
        if (_useTouchControls)
        {
            SpaceUp = _SpaceUpButton.IsHolded;
            SpaceDown = _SpaceDownButton.IsHolded;
            _jump = _JumpButton.IsTriggered;
        } else
        {
            SpaceUp = Input.GetKey(KeyCode.Space);
            Debug.Log("SpaceUp: " + SpaceUp);
            SpaceDown = Input.GetKey(KeyCode.LeftControl);
            _jump = SpaceUp;
            Debug.Log("Jump: " +  _jump);
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ShowCursor(!IsCursorVisible);
        }

        UpdateMovement();
        UpdateRotation();


    }

    private void UpdateMovement()
    {
        if (_useTouchControls)
        {
            Movement = new Vector3(_MoveJoystick.Horizontal(), 0f, _MoveJoystick.Vertical());
        } else
        {
            Movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        }
    }


    public void UpdateRotation()
    {
        if (_useTouchControls)
        {

        }
        else
        {
            Rotation = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }
    }


    public void ShowCursor(bool visible)
    {
        Debug.Log("Set cursor visibility to: " + visible);
        IsCursorVisible = visible;
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
