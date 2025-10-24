using System.IO;
using UnityEngine;

public class SteamCloudTest : MonoBehaviour
{
    string fileName = "steamcloud_test.txt";
    public string testString = "진열의 Steam Cloud 테스트입니다!";
    string filePath;

    void Start()
    {
        // Steam Cloud가 감지할 수 있는 경로
        filePath = Path.Combine(Application.persistentDataPath, fileName);

        // 저장 테스트
        //SaveToSteamCloud(testString);

        // 불러오기 테스트
        string loaded = LoadFromSteamCloud();
        Debug.Log("Steam Cloud에서 불러온 내용: " + loaded);
    }

    void SaveToSteamCloud(string content)
    {
        File.WriteAllText(filePath, content);
        Debug.Log("Steam Cloud에 저장 완료: " + filePath);
    }

    string LoadFromSteamCloud()
    {
        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }
        else
        {
            Debug.LogWarning("Steam Cloud 파일이 존재하지 않습니다.");
            return "";
        }
    }
}
