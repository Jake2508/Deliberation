using UnityEngine;
using UnityEngine.EventSystems;

public class EventClicks : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private GridNode _gridNode;

    private void Awake()
    {
        _gridNode = GetComponent<GridNode>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Show Highlight
        Debug.Log("Click");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Hide Highlight
        Debug.Log("Release");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Show Grid Selected Visual
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Show Grid Hover Visual
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Hide Grid Hover Visual
    }
}
