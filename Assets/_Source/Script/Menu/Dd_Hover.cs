using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dd_Hover : MonoBehaviour, ISelectHandler
{
    public void OnSelect(BaseEventData eventData)
    {
        // 부모 ScrollRect 찾기
        ScrollRect scrollRect = GetComponentInParent<ScrollRect>();
        if (scrollRect == null) return;

        RectTransform item = GetComponent<RectTransform>();
        RectTransform viewport = scrollRect.viewport;

        // 아이템과 뷰포트의 월드 좌표 비교
        Vector3[] itemCorners = new Vector3[4];
        Vector3[] viewportCorners = new Vector3[4];
        item.GetWorldCorners(itemCorners);
        viewport.GetWorldCorners(viewportCorners);

        // 아래쪽 벗어남
        if (itemCorners[0].y < viewportCorners[0].y)
        {
            float offset = viewportCorners[0].y - itemCorners[0].y;
            scrollRect.content.anchoredPosition += new Vector2(0, offset);
        }
        // 위쪽 벗어남
        else if (itemCorners[1].y > viewportCorners[1].y)
        {
            float offset = itemCorners[1].y - viewportCorners[1].y;
            scrollRect.content.anchoredPosition -= new Vector2(0, offset);
        }
    }
}