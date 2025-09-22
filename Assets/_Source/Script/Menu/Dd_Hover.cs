using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dd_Hover : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        // �θ� ScrollRect ã��
        ScrollRect scrollRect = GetComponentInParent<ScrollRect>();
        if (scrollRect == null) return;

        RectTransform item = GetComponent<RectTransform>();
        RectTransform viewport = scrollRect.viewport;

        // �����۰� ����Ʈ�� ���� ��ǥ ��
        Vector3[] itemCorners = new Vector3[4];
        Vector3[] viewportCorners = new Vector3[4];
        item.GetWorldCorners(itemCorners);
        viewport.GetWorldCorners(viewportCorners);

        // �Ʒ��� ���
        if (itemCorners[0].y < viewportCorners[0].y)
        {
            float offset = viewportCorners[0].y - itemCorners[0].y;
            scrollRect.content.anchoredPosition += new Vector2(0, offset);
        }
        // ���� ���
        else if (itemCorners[1].y > viewportCorners[1].y)
        {
            float offset = itemCorners[1].y - viewportCorners[1].y;
            scrollRect.content.anchoredPosition -= new Vector2(0, offset);
        }
    }
}