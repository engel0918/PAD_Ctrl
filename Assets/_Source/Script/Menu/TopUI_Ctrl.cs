using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class TopUI_Ctrl : MonoBehaviour
{

    [SerializeField] ScrollRect Scroll;
 
    public List<RectTransform> MovBtns;
    public List<RectTransform> BtnTxts;

    [SerializeField] int BtnSpace;

    [Tooltip("이동 속도 (0이면 즉시 이동)")]
    public float smoothSpeed = 10f; // 0으로 하면 즉시 이동

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        SetBtns();


    }

    void SetBtns()
    {

        for (int i = 0; i <= (MovBtns.Count-1); i++)
        {
            Vector2 Std = MovBtns[i].sizeDelta;
            float txtX = BtnTxts[i].GetComponent<TextMeshProUGUI>().preferredWidth;

            MovBtns[i].sizeDelta = new Vector2(BtnSpace + txtX, Std.y);
        }

        HorizontalLayoutGroup Hor = Scroll.content.gameObject.GetComponent<HorizontalLayoutGroup>();
        RectTransform Con_rect = Scroll.content.GetComponent<RectTransform>();

        Vector2 Std_Con = Con_rect.sizeDelta;

        float Size_X = 0;
        for (int i = 0; i <= (MovBtns.Count - 1); i++)
        {
            float size = Hor.spacing + MovBtns[i].GetComponent<RectTransform>().sizeDelta.x;
            Size_X += size;
        }

        Con_rect.sizeDelta = new Vector2(Size_X + (Hor.spacing * 2), Std_Con.y);
    }

    /// <summary>
    /// 지정된 대상 오브젝트를 스크롤 중앙에 오도록 이동시킴
    /// </summary>

    public void MoveToTarget(RectTransform target)
    {
        RectTransform content = Scroll.content;    // Content

        if (Scroll == null || content == null || target == null)
            return;

        float contentWidth = content.rect.width;
        float viewportWidth = Scroll.viewport.rect.width;

        // target의 Content 내 위치
        float targetX = target.anchoredPosition.x;

        // 중앙 정렬 계산
        float normalizedX = Mathf.Clamp01((targetX - viewportWidth / 2) / (contentWidth - viewportWidth));

        // 즉시 이동
        Scroll.normalizedPosition = new Vector2(normalizedX, Scroll.normalizedPosition.y);
    }
}
