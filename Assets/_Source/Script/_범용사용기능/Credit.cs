using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Credit : MonoBehaviour
{
    [SerializeField] Tit_Ctrl Tit;

    public ScrollRect scroll;
    public float scrollSpeed = 20f;
    public float UpSpd = 3f;

    [SerializeField] RectTransform Contents;
    bool Scroll;

    // 내부 입력 상태 저장용
    float InpSpdMtp = 1f;

    private void OnEnable()
    {
        Scroll = false;
        scroll.verticalNormalizedPosition = 1f;
        StartCoroutine(HeightSet_Load());
    }

    private void Update()
    {

        if (!Scroll)
            return;

        float verticalInput = Input.GetAxis("Vertical");
        InpSpdMtp = 1f;

        // ↑ / Space / 스틱 위 = 가속
        if (verticalInput > 0.5f || Input.GetKey(KeyCode.Space))
            InpSpdMtp = UpSpd;

        // ↓ / 스틱 아래 = 감속
        if (verticalInput < -0.5f)
            InpSpdMtp = 0.5f;

    }

    private void FixedUpdate()
    {
        // 실제 스크롤 이동
        scroll.verticalNormalizedPosition -= (scrollSpeed * InpSpdMtp) * Time.deltaTime / 100f;

        // 끝 도달 시 자동 종료
        if (scroll.verticalNormalizedPosition <= 0f)
        {
            scroll.verticalNormalizedPosition = 0f;
            SkipCredit();
        }
    }

    private void SkipCredit()
    {
        Scroll = false;
        Debug.Log("크래딧 종료");
        if (Tit != null) Tit.SetReturn();
    }

    IEnumerator HeightSet_Load()
    {
        float timer = 0f;

        while (timer < 1f)
        {
            Height_Set();
            timer += Time.deltaTime;
            yield return null;
        }

        Scroll = true;
    }

    void Height_Set()
    {
        if (Contents.childCount <= 0)
            return;

        Vector2 Std = Contents.sizeDelta;
        Contents.sizeDelta = new Vector2(Std.x, 0);

        float Set_Y = 0;
        for (int i = 0; i < Contents.childCount; i++)
        {
            RectTransform childRect = Contents.GetChild(i).GetComponent<RectTransform>();
            if (childRect != null)
                Set_Y += childRect.sizeDelta.y;
        }

        Contents.sizeDelta = new Vector2(Std.x, Set_Y);
    }
}
