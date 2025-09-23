using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollAutoFocus : MonoBehaviour
{
    public ScrollRect scrollRect; // 기본 UI ScrollRect (예: 인벤토리, 메뉴 등)

    private void FixedUpdate()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected == null) return;

        RectTransform target = selected.GetComponent<RectTransform>();
        if (target == null) return;

        // 1) 일반 ScrollRect 안
        if (scrollRect != null && target.transform.IsChildOf(scrollRect.content))
        {
            EnsureVisible(scrollRect, target);
            return;
        }

        // 2) TMP_Dropdown의 Dropdown List 안 (동적으로 생성됨)
        ScrollRect dropdownScroll = target.GetComponentInParent<ScrollRect>();
        if (dropdownScroll != null && target.transform.IsChildOf(dropdownScroll.content))
        {
            EnsureVisible(dropdownScroll, target);
        }
    }

    void EnsureVisible(ScrollRect scroll, RectTransform target)
    {
        RectTransform viewport = scroll.viewport;

        Vector3[] vpCorners = new Vector3[4];
        Vector3[] tgCorners = new Vector3[4];
        viewport.GetWorldCorners(vpCorners);
        target.GetWorldCorners(tgCorners);

        // 아래쪽으로 벗어남
        if (tgCorners[0].y < vpCorners[0].y)
            scroll.content.localPosition += new Vector3(0, vpCorners[0].y - tgCorners[0].y);

        // 위쪽으로 벗어남
        if (tgCorners[1].y > vpCorners[1].y)
            scroll.content.localPosition -= new Vector3(0, tgCorners[1].y - vpCorners[1].y);
    }
}