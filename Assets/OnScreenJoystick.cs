using UnityEngine;
using UnityEngine.EventSystems;

public class OnScreenJoystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    [SerializeField] private RectTransform background;  // The joystick background
    [SerializeField] private RectTransform knob;        // The movable knob
    [SerializeField] private float maxRadius = 100f;    // Maximum distance knob can move

    private Vector2 inputVector;                       // The final input value
    private Vector2 startPos;                          // Initial position of background
    private bool isDragging = false;

    void Start()
    {
        // Store initial position
        startPos = background.anchoredPosition;
    }

    // Called when pointer/touch presses down
    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
        OnDrag(eventData);
    }

    // Called while dragging
    public void OnDrag(PointerEventData eventData)
    {
        // Convert screen point to local point in rectangle
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            background,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        // Clamp the position within the radius
        localPoint = Vector2.ClampMagnitude(localPoint, maxRadius);

        // Update knob position
        knob.anchoredPosition = localPoint;

        // Calculate input vector (-1 to 1 range)
        inputVector = localPoint / maxRadius;
        Debug.Log("Joystick: " + inputVector);
    }

    // Called when pointer/touch is released
    public void OnPointerUp(PointerEventData eventData)
    {
        isDragging = false;
        inputVector = Vector2.zero;
        knob.anchoredPosition = Vector2.zero;
    }

    // Public methods to get the input values
    public float Horizontal()
    {
        return inputVector.x;
    }

    public float Vertical()
    {
        return inputVector.y;
    }

    // Optional: Get the raw Vector2 input
    public Vector2 GetInputDirection()
    {
        return inputVector;
    }
}
