using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollAutoFocus : MonoBehaviour
{
    public ScrollRect scrollRect; // �⺻ UI ScrollRect (��: �κ��丮, �޴� ��)

    private void FixedUpdate()
    {
        GameObject selected = EventSystem.current.currentSelectedGameObject;
        if (selected == null) return;

        RectTransform target = selected.GetComponent<RectTransform>();
        if (target == null) return;

        // 1) �Ϲ� ScrollRect ��
        if (scrollRect != null && target.transform.IsChildOf(scrollRect.content))
        {
            EnsureVisible(scrollRect, target);
            return;
        }

        // 2) TMP_Dropdown�� Dropdown List �� (�������� ������)
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

        // �Ʒ������� ���
        if (tgCorners[0].y < vpCorners[0].y)
            scroll.content.localPosition += new Vector3(0, vpCorners[0].y - tgCorners[0].y);

        // �������� ���
        if (tgCorners[1].y > vpCorners[1].y)
            scroll.content.localPosition -= new Vector3(0, tgCorners[1].y - vpCorners[1].y);
    }
}