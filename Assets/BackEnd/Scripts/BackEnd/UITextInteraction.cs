using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;

// IPointerHandler 마우트 포인터가 특정 영역에 있는지, 클릭했는지 여부를 확인할 때 사용
public class UITextInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [System.Serializable]
    private class OnClickEvent : UnityEvent { }

    [SerializeField]
    private OnClickEvent onClickEvent;

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Bold;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Normal;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickEvent?.Invoke();
    }
}
