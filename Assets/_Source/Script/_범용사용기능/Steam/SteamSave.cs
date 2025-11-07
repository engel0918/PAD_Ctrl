using Steamworks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[System.Serializable]

public class SettingsData
{
    public int GQ, AA, GM, RES;
    public float Mv, BGMv, SFXv;
}

[System.Serializable]
public class MyRank
{
    public static string Name;
    public static Texture2D Pic;
}

public class SteamSave : MonoBehaviour
{
    private const string Setting_File = "settings.json";

    void Start()
    {

        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam 초기화 안됨");
            return;
        }

        // 내 이름, 사진 불러오기
        MyRank.Name = SteamFriends.GetPersonaName();

        LoadMyProfilePicture((Texture2D tex) =>
        {
            if (tex != null)
            { MyRank.Pic = tex; }
        });

        Debug.Log("Cloud 사용 가능?: " + SteamRemoteStorage.IsCloudEnabledForApp());
    }

    public void Steam_Set()
    {
        if (!SteamManager.Initialized)
        {
            Debug.LogError("Steam 초기화 안됨");
            return;
        }

        // 내 이름, 사진 불러오기
        MyRank.Name = SteamFriends.GetPersonaName();

        LoadMyProfilePicture((Texture2D tex) =>
        {
            if (tex != null)
            { MyRank.Pic = tex; }
        });
    }

    public string SetDat_Check()
    {

        Steam_Set();

        // 클라우드 삭제 기능 
        if (SteamRemoteStorage.FileExists(Setting_File))
        {
            bool deleted = SteamRemoteStorage.FileDelete(Setting_File);
            Debug.Log("File Deleted: " + deleted);
        }

        if (!SteamRemoteStorage.FileExists(Setting_File))
        { return "No Data"; }
        else
        { return "Data Available"; }
    }

    public void SaveSettings(List<int> val1, List<float> val2)
    {
        SettingsData data = new SettingsData
        {
            GQ = val1[0],
            AA = val1[1],
            GM = val1[2],
            RES = val1[3],

            Mv = val2[0],
            BGMv = val2[1],
            SFXv = val2[2],
        };

        string json = JsonUtility.ToJson(data);
        byte[] bytes = Encoding.UTF8.GetBytes(json);

        bool result = SteamRemoteStorage.FileWrite(Setting_File, bytes, bytes.Length);

        Debug.Log("Settings Saved to Steam: " + result);
        //Debug.Log(data.Test);
    }

    public void LoadSettings()
    {
        if (!SteamRemoteStorage.FileExists(Setting_File))
        {
            Debug.Log("No settings file found in Steam cloud.");
            return;
        }

        int size = SteamRemoteStorage.GetFileSize(Setting_File);
        byte[] buffer = new byte[size];
        SteamRemoteStorage.FileRead(Setting_File, buffer, size);

        string json = Encoding.UTF8.GetString(buffer);
        SettingsData data = JsonUtility.FromJson<SettingsData>(json);

        Debug.Log("Settings Loaded from Steam.");
        //Debug.Log(data.Test);
    }


    // 내 정보 들고오기
    public void LoadMyProfilePicture(Action<Texture2D> onAvatarReady = null)
    {
        CSteamID mySteamId = SteamUser.GetSteamID();
        int avatarInt = SteamFriends.GetLargeFriendAvatar(mySteamId); // Large: 184x184

        if (avatarInt == -1)
        {
            Debug.LogWarning("아바타 로딩 대기 중...");
            StartCoroutine(WaitForAvatar(mySteamId, onAvatarReady));
            return;
        }

        if (SteamUtils.GetImageSize(avatarInt, out uint width, out uint height))
        {
            byte[] imageData = new byte[width * height * 4];

            if (SteamUtils.GetImageRGBA(avatarInt, imageData, imageData.Length))
            {
                Texture2D avatarTex = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
                avatarTex.LoadRawTextureData(imageData);
                avatarTex.Apply();

                onAvatarReady?.Invoke(avatarTex);
            }
        }
    }

    private IEnumerator WaitForAvatar(CSteamID steamId, Action<Texture2D> callback)
    {
        int avatarInt = -1;
        while (avatarInt == -1)
        {
            yield return new WaitForSeconds(0.1f);
            avatarInt = SteamFriends.GetLargeFriendAvatar(steamId);
        }

        if (SteamUtils.GetImageSize(avatarInt, out uint width, out uint height))
        {
            byte[] imageData = new byte[width * height * 4];

            if (SteamUtils.GetImageRGBA(avatarInt, imageData, imageData.Length))
            {
                Texture2D avatarTex = new Texture2D((int)width, (int)height, TextureFormat.RGBA32, false);
                avatarTex.LoadRawTextureData(imageData);
                avatarTex.Apply();

                callback?.Invoke(avatarTex);
            }
        }
    }


    void Update()
    {
        if (SteamManager.Initialized)
            SteamAPI.RunCallbacks();
    }

}