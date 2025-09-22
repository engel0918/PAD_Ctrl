using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageCtrl : MonoBehaviour
{
    public List<Button> PageBtns;

    public List<GameObject> Pages = new List<GameObject>();
    public List<GameObject> PageSet_01 = new List<GameObject>();

    private void Start()
    {
        SetBtn();
    }

    void SetBtn()
    {
        if (PageBtns.Count > 0)
        {
            for (int i = 0; i <= (PageBtns.Count - 1); i++)
            {
                if (PageBtns[i] != null)
                {
                    int index = i;
                    PageBtns[i].onClick.AddListener(() => func_PageCtrl(index));
                }
            }
        }
    }

    public void func_PageCtrl(int i)
    {
        foreach (GameObject page in Pages)
        {
            if (page != null)
            { page.SetActive(false); }
        }

        Pages[i].SetActive(true);

        if (PageSet_01.Count > 0)
        {
            foreach (GameObject page in PageSet_01)
            {
                if (page != null)
                { page.SetActive(false); }
            }

            if (PageSet_01[i] != null)
            { PageSet_01[i].SetActive(true); }
        }
    }

    public void All_Check(bool b)
    {
        foreach (GameObject page in Pages)
        {
            if (page != null)
            { page.SetActive(b); }
        }

        if (PageSet_01.Count > 0)
        {
            foreach (GameObject page in PageSet_01)
            {
                if (page != null)
                { page.SetActive(b); }
            }
        }
    }

    public void True_Page(int i)
    {
        Pages[i].SetActive(true);

        if (PageSet_01.Count > 0)
        {
            if (PageSet_01[i] != null)
            { PageSet_01[i].SetActive(true); }
        }
    }

    public void False_Page(int i)
    {
        Pages[i].SetActive(false);

        if (PageSet_01.Count > 0)
        {
            if (PageSet_01[i] != null)
            { PageSet_01[i].SetActive(false); }
        }
    }
}
