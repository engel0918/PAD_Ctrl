using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-10)] // 다른 스크립트보다 먼저 실행되게
public class Navi_Finder : MonoBehaviour
{
    [Header("Navi UI - Collect")]
    public List<Button> Btns = new List<Button>();
    public List<ScrollRect> Scrolls = new List<ScrollRect>();

    void Awake()
    {
        Btns.Clear();
        Scrolls.Clear();

        // 버튼, 스크롤 자동 수집
        Btns.AddRange(GetComponentsInChildren<Button>(true));
        Scrolls.AddRange(GetComponentsInChildren<ScrollRect>(true));
    }

    public List<Button> GetButtons() => Btns;
    public List<ScrollRect> GetScrollRects() => Scrolls;
}