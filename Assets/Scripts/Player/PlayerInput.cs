using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private bool _useTouchControls = false;

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

    private void Update()
    {
        SpaceUp = Input.GetKey(KeyCode.Space);
        SpaceDown = Input.GetKey(KeyCode.LeftControl);
        JumpTriggered = Input.GetKeyDown(KeyCode.Space);

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
        IsCursorVisible = visible;
        Cursor.visible = visible;
        Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
