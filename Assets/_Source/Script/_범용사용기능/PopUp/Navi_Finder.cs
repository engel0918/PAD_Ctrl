using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-10)] // 다른 스크립트보다 먼저 실행되게
public class Navi_Finder : MonoBehaviour
{
    [Header("Navi UI - Collect")]
    public List<Button> Btns = new List<Button>();
    public List<ScrollRect> Scrolls = new List<ScrollRect>();
    public List<TMP_Dropdown> Dropdowns = new List<TMP_Dropdown>();
    public List<Slider> Sliders = new List<Slider>();

    void Awake()
    {
        CollectUI();
    }

    void CollectUI()
    {
        Btns.Clear();
        Scrolls.Clear();
        Dropdowns.Clear();
        Sliders.Clear();

        string ignore_cd = "Ign_NavFind";

        // 한 번만 탐색 → 타입별로 분류
        foreach (var comp in GetComponentsInChildren<MonoBehaviour>(true))
        {
            //if (comp.CompareTag(tag)) continue;
            // 이름에 특정 문자열 포함되면 무시
            if (comp.name.Contains(ignore_cd))
                continue;

            if (comp is Button b && !Btns.Contains(b))
                Btns.Add(b);

            else if (comp is ScrollRect s && !Scrolls.Contains(s))
                Scrolls.Add(s);

            else if (comp is TMP_Dropdown d && !Dropdowns.Contains(d))
                Dropdowns.Add(d);

            else if (comp is Slider sl && !Sliders.Contains(sl))
                Sliders.Add(sl);
        }
    }

    public List<Button> GetButtons() => Btns;
    public List<ScrollRect> GetScrollRects() => Scrolls;
    public List<TMP_Dropdown> GetDropdowns() => Dropdowns;
    public List<Slider> GetSliders() => Sliders;
}