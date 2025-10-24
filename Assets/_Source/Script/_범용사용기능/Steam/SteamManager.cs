using Steamworks;
using UnityEngine;
using Steamworks;


public class SteamManager : MonoBehaviour
{
    private bool initialized = false;

    void Start()
    {
        try
        {
            if (SteamAPI.RestartAppIfNecessary((AppId_t)3904900))
            {
                Application.Quit();
                return;
            }

            SteamAPI.Init();
            initialized = true;

            Debug.Log("Steam 초기화 성공!");
            Debug.Log("Steam 사용자 이름: " + SteamFriends.GetPersonaName());
        }
        catch (System.Exception e)
        {
            Debug.LogError("Steam 초기화 실패: " + e.Message);
        }
    }

    void Update()
    {
        if (initialized)
            SteamAPI.RunCallbacks();
    }

    void OnApplicationQuit()
    {
        if (initialized)
            Debug.Log("Steam - 종료");
            SteamAPI.Shutdown();
    }
}

