using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TopUI_Ctrl : MonoBehaviour
{
    public List<RectTransform> MovBtns;
    public List<TMP_Text> BtnTxts;

    [SerializeField] int BtnSpace;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetBtns();


    }

    void SetBtns()
    {
        for(int i = 0; i <= (MovBtns.Count-1); i++)
        {
            Vector2 Std = MovBtns[i].sizeDelta;
            float txtX = BtnTxts[i].GetComponent<RectTransform>().sizeDelta.x;

            MovBtns[i].sizeDelta = new Vector2(BtnSpace + txtX, Std.y);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
