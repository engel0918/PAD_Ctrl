using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credit : MonoBehaviour
{
    [SerializeField] Tit_Ctrl Tit;
    
    public ScrollRect scroll;
    public float scrollSpeed = 20f;

    [SerializeField] RectTransform Contents;
    bool Scroll;

    private void OnEnable()
    {
        Scroll = false;

        // 시작 시 맨 위에서 시작하도록 (위 > 아래)
        scroll.verticalNormalizedPosition = 1f;

        StartCoroutine(HeightSet_Load());
    }

    private void FixedUpdate()
    {
        if (Scroll)
        {
            // 끝에 도달하면 멈추거나 씬 전환
            if (scroll.verticalNormalizedPosition <= 0f)
            {
                Debug.Log("크래딧 종료");

                if (Tit != null) { Tit.SetReturn(); }

                Scroll = false;
            }
            else
            {
                // 반대 방향 (위 > 아래)
                scroll.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime / 100f;
            }
        }
    }

    IEnumerator HeightSet_Load()
    {
        float timer = 0f;

        while (timer < 1f) // 1초 동안 Height_Set 반복
        {
            Height_Set();
            timer += Time.deltaTime;
            yield return null;
        }

        // 1초 후 스크롤 시작
        Scroll = true;
    }

    void Height_Set()
    {
        if (Contents.childCount > 0)
        {
            Vector2 Std = Contents.sizeDelta;
            Contents.sizeDelta = new Vector2(Std.x, 0);

            float Set_Y = 0;
            for (int i = 0; i < Contents.childCount; i++)
            {
                RectTransform childRect = Contents.GetChild(i).GetComponent<RectTransform>();
                if (childRect != null)
                {
                    Set_Y += childRect.sizeDelta.y;
                }
            }

            Contents.sizeDelta = new Vector2(Std.x, Set_Y);
        }
    }
}