using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
public class TimeEditer : MonoBehaviour, IDragHandler, IEndDragHandler
{
    public RectTransform hourHand;
    public RectTransform minuteHand;
    public RectTransform secondHand;
    public TMP_InputField hourInputField;
    public TMP_InputField minuteInputField;
    public TMP_InputField secondInputField;
    [SerializeField] private Button editTimeButton;
    [SerializeField] private Image isEditingVisual;
    [SerializeField] private GameObject tipText;
    [SerializeField] private string[] editButtonText;
    public ClockHandControl clockHandController;

    private RectTransform currentDraggingHand;
    private bool isDragging = false;
    private bool isEditing = false;

    void Start()
    {
        editTimeButton.onClick.AddListener(ToggleEditMode);
        editTimeButton.GetComponentInChildren<TextMeshProUGUI>().text = editButtonText[0];
    }

    private void ToggleEditMode()
    {
        isEditing = !isEditing;
        isEditingVisual.enabled = isEditing;
        editTimeButton.GetComponentInChildren<TextMeshProUGUI>().text = isEditing ? editButtonText[1] : editButtonText[0];
        tipText.SetActive(isEditing);

        SetInputFieldsInteractable(isEditing);
    }

    private void SetInputFieldsInteractable(bool state)
    {
        hourInputField.interactable = state;
        minuteInputField.interactable = state;
        secondInputField.interactable = state;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CheckHandUnderMouse();
        }
    }

    private void CheckHandUnderMouse()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(hourHand, Input.mousePosition))
        {
            currentDraggingHand = hourHand;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(minuteHand, Input.mousePosition))
        {
            currentDraggingHand = minuteHand;
        }
        else if (RectTransformUtility.RectangleContainsScreenPoint(secondHand, Input.mousePosition))
        {
            currentDraggingHand = secondHand;
        }
        else
        {
            currentDraggingHand = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isEditing && currentDraggingHand != null)
        {
            Vector2 direction = (Vector2)Input.mousePosition - (Vector2)currentDraggingHand.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            currentDraggingHand.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isEditing && currentDraggingHand != null)
        {
            isDragging = false;
            UpdateClockTime();
            currentDraggingHand = null;
        }
    }

    private void UpdateClockTime()
    {
        if (currentDraggingHand == hourHand)
        {
            float hoursAngle = hourHand.eulerAngles.z;
            int newHour = Mathf.RoundToInt((360 - hoursAngle) / 30f) % 12;
            clockHandController.SetHour(newHour);
        }
        else if (currentDraggingHand == minuteHand)
        {
            float minutesAngle = minuteHand.eulerAngles.z;
            int newMinute = Mathf.RoundToInt((360 - minutesAngle) / 6f) % 60;
            clockHandController.SetMinute(newMinute);
        }
        else if (currentDraggingHand == secondHand)
        {
            float secondsAngle = secondHand.eulerAngles.z;
            int newSecond = Mathf.RoundToInt((360 - secondsAngle) / 6f) % 60;
            clockHandController.SetSecond(newSecond);
        }
    }

    public void InputFieldsToTime()
    {
        int hour = int.Parse(hourInputField.text);
        int minute = int.Parse(minuteInputField.text);
        int second = int.Parse(secondInputField.text);

        hour = Mathf.Clamp(hour, 0, 23);
        minute = Mathf.Clamp(minute, 0, 59);
        second = Mathf.Clamp(second, 0, 59);

        clockHandController.SetHour(hour);
        clockHandController.SetMinute(minute);
        clockHandController.SetSecond(second);
    }
}
