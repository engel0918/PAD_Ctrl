using UnityEngine;

[System.Serializable]

public class SettingsData
{
    public int GQ, AA, GM, RES;
    public float Mv, BGMv, SFXv;
}

public class Data_Mgr : MonoBehaviour
{
    private const string Setting = "settings.json";


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
