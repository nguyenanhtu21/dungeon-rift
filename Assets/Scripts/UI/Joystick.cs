using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    private RectTransform bg;
    private RectTransform handle;
    private Vector2 inputVector;

    void Start()
    {
        bg = transform as RectTransform;
        handle = transform.GetChild(0) as RectTransform; // JoystickHandle
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bg, eventData.position, eventData.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bg.sizeDelta.x);
            pos.y = (pos.y / bg.sizeDelta.y);

            inputVector = new Vector2(pos.x * 2, pos.y * 2);
            inputVector = (inputVector.magnitude > 1) ? inputVector.normalized : inputVector;

            // di chuyển handle theo input
            handle.anchoredPosition = new Vector2(
                inputVector.x * (bg.sizeDelta.x / 2),
                inputVector.y * (bg.sizeDelta.y / 2)
            );
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        inputVector = Vector2.zero;
        handle.anchoredPosition = Vector2.zero;
    }

    public float Horizontal() { return inputVector.x; }
    public float Vertical() { return inputVector.y; }
    public Vector2 Direction() { return inputVector; }
}
