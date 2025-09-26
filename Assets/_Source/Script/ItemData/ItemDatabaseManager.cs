using NUnit.Framework.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

[Serializable]
public class RWPData
{
    // 원거리 장비
    public string Id, Type, Name, Rarity, Description;
    public int MaxHP, MaxMP, STR, INT, DEX, ATK, DEF;
}
[Serializable]
public class MWPData
{
    // 근거리 장비
    public string Id, Type, Name, Rarity, Description;
    public int MaxHP, MaxMP, STR, INT, DEX, ATK, DEF;
}

[Serializable]
public class PartsData
{
    // 방어구및 파츠
    public string Id, Type, Name, Rarity, Description;
    public int MaxHP, MaxMP, STR, INT, DEX, ATK , DEF;
}

[Serializable]
public class CONData
{
    // 소비 아이템
    public string Id, Type, Name, Rarity, Description;
    public int Delay, BuffTime, MaxHP, MaxMP, HP, MP, STR, INT, DEX, ATK, DEF;
}

[Serializable]
public class MATData
{
    // 재료및 잡템
    public string Id, Name, Rarity, Description;
}

[Serializable]
public class VALData
{
    // 재료및 잡템
    public string Id, Name, Rarity, Description;
}

[Serializable]
public class RWPDatabaseWrapper
{ public List<RWPData> RWPs; }

[Serializable]
public class MWPDatabaseWrapper
{ public List<MWPData> MWPs; }

[Serializable]
public class PartsDatabaseWrapper
{ public List<PartsData> Parts; }

[Serializable]
public class CONDatabaseWrapper
{ public List<CONData> CONs; }

[Serializable]
public class MATDatabaseWrapper
{ public List<MATData> MATs; }

[Serializable]
public class VALDatabaseWrapper
{ public List<VALData> VALs; }


//public static class ItemDatabaseManager
//{
//    // 전역 변수로 데이터 저장
//    public static ItemDatabaseWrapper db;

//    // ---------------- AES 암호화/복호화 ----------------
//    private static byte[] Encrypt(string plainText)
//    {
//        using Aes aes = Aes.Create();
//        aes.Key = ItemList_KeyMgr.GetUserKey();
//        aes.GenerateIV();
//        aes.Padding = PaddingMode.PKCS7;

//        ICryptoTransform encryptor = aes.CreateEncryptor();
//        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
//        byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

//        // [IV(16)] + [Cipher]
//        return aes.IV.Concat(cipherBytes).ToArray();
//    }

//    private static string Decrypt(byte[] data)
//    {
//        using Aes aes = Aes.Create();
//        aes.Key = ItemList_KeyMgr.GetUserKey();
//        aes.Padding = PaddingMode.PKCS7;

//        byte[] iv = data.Take(16).ToArray();
//        byte[] cipherBytes = data.Skip(16).ToArray();
//        aes.IV = iv;

//        ICryptoTransform decryptor = aes.CreateDecryptor();
//        byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
//        return Encoding.UTF8.GetString(plainBytes);
//    }

//    // ---------------- 공개 메서드 ----------------
//    public static void LoadDatabase()
//    {
//        string path = Path.Combine(Application.streamingAssetsPath, "ItemData/items.bytes");

//        if (!File.Exists(path))
//        {
//            Debug.LogError("Encrypted item file not found: " + path);
//            db = new ItemDatabaseWrapper { Items = new List<ItemData>() };
//            return;
//        }

//        byte[] encrypted = File.ReadAllBytes(path);
//        string json = Decrypt(encrypted);

//        if (string.IsNullOrEmpty(json))
//        {
//            db = new ItemDatabaseWrapper { Items = new List<ItemData>() };
//            return;
//        }

//        db = JsonUtility.FromJson<ItemDatabaseWrapper>(json);
//        if (db.Items == null) db.Items = new List<ItemData>();

//        Debug.Log($"Item database loaded. Total items: {db.Items.Count}");
//    }

//    public static ItemData GetItemById(string id)
//    {
//        if (db == null || db.Items == null) return null;
//        return db.Items.Find(item => item.Id == id);
//    }

//    public static List<ItemData> GetAllItems()
//    {
//        if (db == null || db.Items == null) return new List<ItemData>();
//        return db.Items;
//    }

//    public static void SaveEncrypted(ItemDatabaseWrapper dbToSave, string path)
//    {
//        string json = JsonUtility.ToJson(dbToSave, true);
//        byte[] encrypted = Encrypt(json);
//        Directory.CreateDirectory(Path.GetDirectoryName(path));
//        File.WriteAllBytes(path, encrypted);
//        Debug.Log($"Encrypted item data saved to: {path}");
//    }
//}

public static class RWPDatabaseManager
{
    // 전역 변수로 데이터 저장
    public static RWPDatabaseWrapper db;

    // ---------------- AES 암호화/복호화 ----------------
    private static byte[] Encrypt(string plainText)
    {
        using Aes aes = Aes.Create();
        aes.Key = ItemList_KeyMgr.GetUserKey();
        aes.GenerateIV();
        aes.Padding = PaddingMode.PKCS7;

        ICryptoTransform encryptor = aes.CreateEncryptor();
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // [IV(16)] + [Cipher]
        return aes.IV.Concat(cipherBytes).ToArray();
    }

    private static string Decrypt(byte[] data)
    {
        using Aes aes = Aes.Create();
        aes.Key = ItemList_KeyMgr.GetUserKey();
        aes.Padding = PaddingMode.PKCS7;

        byte[] iv = data.Take(16).ToArray();
        byte[] cipherBytes = data.Skip(16).ToArray();
        aes.IV = iv;

        ICryptoTransform decryptor = aes.CreateDecryptor();
        byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }

    // ---------------- 공개 메서드 ----------------
    public static void LoadDatabase()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "ItemData/items.bytes");

        if (!File.Exists(path))
        {
            Debug.LogError("Encrypted item file not found: " + path);
            db = new RWPDatabaseWrapper { RWPs = new List<RWPData>() };
            return;
        }

        byte[] encrypted = File.ReadAllBytes(path);
        string json = Decrypt(encrypted);

        if (string.IsNullOrEmpty(json))
        {
            db = new RWPDatabaseWrapper { RWPs = new List<RWPData>() };
            return;
        }

        db = JsonUtility.FromJson<RWPDatabaseWrapper>(json);
        if (db.RWPs == null) db.RWPs = new List<RWPData>();

        Debug.Log($"Item database loaded. Total items: {db.RWPs.Count}");
    }

    public static RWPData GetItemById(string id)
    {
        if (db == null || db.RWPs == null) return null;
        return db.RWPs.Find(item => item.Id == id);
    }

    public static List<RWPData> GetAllItems()
    {
        if (db == null || db.RWPs == null) return new List<RWPData>();
        return db.RWPs;
    }

    public static void SaveEncrypted(RWPDatabaseWrapper dbToSave, string path)
    {
        string json = JsonUtility.ToJson(dbToSave, true);
        byte[] encrypted = Encrypt(json);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllBytes(path, encrypted);
        Debug.Log($"Encrypted item data saved to: {path}");
    }
}
public static class MWPDatabaseManager
{
    // 전역 변수로 데이터 저장
    public static MWPDatabaseWrapper db;

    // ---------------- AES 암호화/복호화 ----------------
    private static byte[] Encrypt(string plainText)
    {
        using Aes aes = Aes.Create();
        aes.Key = ItemList_KeyMgr.GetUserKey();
        aes.GenerateIV();
        aes.Padding = PaddingMode.PKCS7;

        ICryptoTransform encryptor = aes.CreateEncryptor();
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // [IV(16)] + [Cipher]
        return aes.IV.Concat(cipherBytes).ToArray();
    }

    private static string Decrypt(byte[] data)
    {
        using Aes aes = Aes.Create();
        aes.Key = ItemList_KeyMgr.GetUserKey();
        aes.Padding = PaddingMode.PKCS7;

        byte[] iv = data.Take(16).ToArray();
        byte[] cipherBytes = data.Skip(16).ToArray();
        aes.IV = iv;

        ICryptoTransform decryptor = aes.CreateDecryptor();
        byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }

    // ---------------- 공개 메서드 ----------------
    public static void LoadDatabase()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "ItemData/items.bytes");

        if (!File.Exists(path))
        {
            Debug.LogError("Encrypted item file not found: " + path);
            db = new MWPDatabaseWrapper { MWPs = new List<MWPData>() };
            return;
        }

        byte[] encrypted = File.ReadAllBytes(path);
        string json = Decrypt(encrypted);

        if (string.IsNullOrEmpty(json))
        {
            db = new MWPDatabaseWrapper { MWPs = new List<MWPData>() };
            return;
        }

        db = JsonUtility.FromJson<MWPDatabaseWrapper>(json);
        if (db.MWPs == null) db.MWPs = new List<MWPData>();

        Debug.Log($"Item database loaded. Total items: {db.MWPs.Count}");
    }

    public static MWPData GetItemById(string id)
    {
        if (db == null || db.MWPs == null) return null;
        return db.MWPs.Find(item => item.Id == id);
    }

    public static List<MWPData> GetAllItems()
    {
        if (db == null || db.MWPs == null) return new List<MWPData>();
        return db.MWPs;
    }

    public static void SaveEncrypted(MWPDatabaseWrapper dbToSave, string path)
    {
        string json = JsonUtility.ToJson(dbToSave, true);
        byte[] encrypted = Encrypt(json);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllBytes(path, encrypted);
        Debug.Log($"Encrypted item data saved to: {path}");
    }
}
public static class PartDatabaseManager
{
    // 전역 변수로 데이터 저장
    public static PartsDatabaseWrapper db;

    // ---------------- AES 암호화/복호화 ----------------
    private static byte[] Encrypt(string plainText)
    {
        using Aes aes = Aes.Create();
        aes.Key = ItemList_KeyMgr.GetUserKey(); // Steam 기반 키
        aes.GenerateIV();
        aes.Padding = PaddingMode.PKCS7;

        ICryptoTransform encryptor = aes.CreateEncryptor();
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // [IV(16)] + [Cipher]
        return aes.IV.Concat(cipherBytes).ToArray();
    }

    private static string Decrypt(byte[] data)
    {
        using Aes aes = Aes.Create();
        aes.Key = ItemList_KeyMgr.GetUserKey();
        aes.Padding = PaddingMode.PKCS7;

        byte[] iv = data.Take(16).ToArray();
        byte[] cipherBytes = data.Skip(16).ToArray();
        aes.IV = iv;

        ICryptoTransform decryptor = aes.CreateDecryptor();
        try
        {
            byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
            return Encoding.UTF8.GetString(plainBytes);
        }
        catch (Exception e)
        {
            Debug.LogError($"[PartDatabase] Decryption failed. Possibly wrong key or corrupted file. Error: {e.Message}");
            return null;
        }
    }

    // ---------------- 공개 메서드 ----------------
    public static void LoadDatabase()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "PartData/parts.bytes");

        if (!File.Exists(path))
        {
            Debug.LogError("Encrypted part file not found: " + path);
            db = new PartsDatabaseWrapper { Parts = new List<PartsData>() };
            return;
        }

        byte[] encrypted = File.ReadAllBytes(path);
        string json = Decrypt(encrypted);

        if (string.IsNullOrEmpty(json))
        {
            db = new PartsDatabaseWrapper { Parts = new List<PartsData>() };
            return;
        }

        db = JsonUtility.FromJson<PartsDatabaseWrapper>(json);
        if (db.Parts == null) db.Parts = new List<PartsData>();

        Debug.Log($"Part database loaded. Total parts: {db.Parts.Count}");
    }

    public static PartsData GetPartById(string id)
    {
        if (db == null || db.Parts == null) return null;
        return db.Parts.Find(part => part.Id == id);
    }

    public static List<PartsData> GetAllParts()
    {
        if (db == null || db.Parts == null) return new List<PartsData>();
        return db.Parts;
    }

    public static void SaveEncrypted(PartsDatabaseWrapper dbToSave, string path)
    {
        string json = JsonUtility.ToJson(dbToSave, true);
        byte[] encrypted = Encrypt(json);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllBytes(path, encrypted);
        Debug.Log($"Encrypted part data saved to: {path}");
    }
}
public static class CONDatabaseManager
{
    // 전역 변수로 데이터 저장
    public static CONDatabaseWrapper db;

    // ---------------- AES 암호화/복호화 ----------------
    private static byte[] Encrypt(string plainText)
    {
        using Aes aes = Aes.Create();
        aes.Key = ItemList_KeyMgr.GetUserKey();
        aes.GenerateIV();
        aes.Padding = PaddingMode.PKCS7;

        ICryptoTransform encryptor = aes.CreateEncryptor();
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // [IV(16)] + [Cipher]
        return aes.IV.Concat(cipherBytes).ToArray();
    }

    private static string Decrypt(byte[] data)
    {
        using Aes aes = Aes.Create();
        aes.Key = ItemList_KeyMgr.GetUserKey();
        aes.Padding = PaddingMode.PKCS7;

        byte[] iv = data.Take(16).ToArray();
        byte[] cipherBytes = data.Skip(16).ToArray();
        aes.IV = iv;

        ICryptoTransform decryptor = aes.CreateDecryptor();
        byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }

    // ---------------- 공개 메서드 ----------------
    public static void LoadDatabase()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "ItemData/items.bytes");

        if (!File.Exists(path))
        {
            Debug.LogError("Encrypted item file not found: " + path);
            db = new CONDatabaseWrapper { CONs = new List<CONData>() };
            return;
        }

        byte[] encrypted = File.ReadAllBytes(path);
        string json = Decrypt(encrypted);

        if (string.IsNullOrEmpty(json))
        {
            db = new CONDatabaseWrapper { CONs = new List<CONData>() };
            return;
        }

        db = JsonUtility.FromJson<CONDatabaseWrapper>(json);
        if (db.CONs == null) db.CONs = new List<CONData>();

        Debug.Log($"Item database loaded. Total items: {db.CONs.Count}");
    }

    public static CONData GetItemById(string id)
    {
        if (db == null || db.CONs == null) return null;
        return db.CONs.Find(item => item.Id == id);
    }

    public static List<CONData> GetAllItems()
    {
        if (db == null || db.CONs == null) return new List<CONData>();
        return db.CONs;
    }

    public static void SaveEncrypted(CONDatabaseWrapper dbToSave, string path)
    {
        string json = JsonUtility.ToJson(dbToSave, true);
        byte[] encrypted = Encrypt(json);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllBytes(path, encrypted);
        Debug.Log($"Encrypted item data saved to: {path}");
    }
}
public static class MATDatabaseManager
{
    // 전역 변수로 데이터 저장
    public static MATDatabaseWrapper db;

    // ---------------- AES 암호화/복호화 ----------------
    private static byte[] Encrypt(string plainText)
    {
        using Aes aes = Aes.Create();
        aes.Key = ItemList_KeyMgr.GetUserKey();
        aes.GenerateIV();
        aes.Padding = PaddingMode.PKCS7;

        ICryptoTransform encryptor = aes.CreateEncryptor();
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // [IV(16)] + [Cipher]
        return aes.IV.Concat(cipherBytes).ToArray();
    }

    private static string Decrypt(byte[] data)
    {
        using Aes aes = Aes.Create();
        aes.Key = ItemList_KeyMgr.GetUserKey();
        aes.Padding = PaddingMode.PKCS7;

        byte[] iv = data.Take(16).ToArray();
        byte[] cipherBytes = data.Skip(16).ToArray();
        aes.IV = iv;

        ICryptoTransform decryptor = aes.CreateDecryptor();
        byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }

    // ---------------- 공개 메서드 ----------------
    public static void LoadDatabase()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "ItemData/items.bytes");

        if (!File.Exists(path))
        {
            Debug.LogError("Encrypted item file not found: " + path);
            db = new MATDatabaseWrapper { MATs = new List<MATData>() };
            return;
        }

        byte[] encrypted = File.ReadAllBytes(path);
        string json = Decrypt(encrypted);

        if (string.IsNullOrEmpty(json))
        {
            db = new MATDatabaseWrapper { MATs = new List<MATData>() };
            return;
        }

        db = JsonUtility.FromJson<MATDatabaseWrapper>(json);
        if (db.MATs == null) db.MATs = new List<MATData>();

        Debug.Log($"Item database loaded. Total items: {db.MATs.Count}");
    }

    public static MATData GetItemById(string id)
    {
        if (db == null || db.MATs == null) return null;
        return db.MATs.Find(item => item.Id == id);
    }

    public static List<MATData> GetAllItems()
    {
        if (db == null || db.MATs == null) return new List<MATData>();
        return db.MATs;
    }

    public static void SaveEncrypted(MATDatabaseWrapper dbToSave, string path)
    {
        string json = JsonUtility.ToJson(dbToSave, true);
        byte[] encrypted = Encrypt(json);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllBytes(path, encrypted);
        Debug.Log($"Encrypted item data saved to: {path}");
    }
}
public static class VALDatabaseManager
{
    // 전역 변수로 데이터 저장
    public static VALDatabaseWrapper db;

    // ---------------- AES 암호화/복호화 ----------------
    private static byte[] Encrypt(string plainText)
    {
        using Aes aes = Aes.Create();
        aes.Key = ItemList_KeyMgr.GetUserKey();
        aes.GenerateIV();
        aes.Padding = PaddingMode.PKCS7;

        ICryptoTransform encryptor = aes.CreateEncryptor();
        byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
        byte[] cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

        // [IV(16)] + [Cipher]
        return aes.IV.Concat(cipherBytes).ToArray();
    }

    private static string Decrypt(byte[] data)
    {
        using Aes aes = Aes.Create();
        aes.Key = ItemList_KeyMgr.GetUserKey();
        aes.Padding = PaddingMode.PKCS7;

        byte[] iv = data.Take(16).ToArray();
        byte[] cipherBytes = data.Skip(16).ToArray();
        aes.IV = iv;

        ICryptoTransform decryptor = aes.CreateDecryptor();
        byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
        return Encoding.UTF8.GetString(plainBytes);
    }

    // ---------------- 공개 메서드 ----------------
    public static void LoadDatabase()
    {
        string path = Path.Combine(Application.streamingAssetsPath, "ItemData/items.bytes");

        if (!File.Exists(path))
        {
            Debug.LogError("Encrypted item file not found: " + path);
            db = new VALDatabaseWrapper { VALs = new List<VALData>() };
            return;
        }

        byte[] encrypted = File.ReadAllBytes(path);
        string json = Decrypt(encrypted);

        if (string.IsNullOrEmpty(json))
        {
            db = new VALDatabaseWrapper { VALs = new List<VALData>() };
            return;
        }

        db = JsonUtility.FromJson<VALDatabaseWrapper>(json);
        if (db.VALs == null) db.VALs = new List<VALData>();

        Debug.Log($"Item database loaded. Total items: {db.VALs.Count}");
    }

    public static VALData GetItemById(string id)
    {
        if (db == null || db.VALs == null) return null;
        return db.VALs.Find(item => item.Id == id);
    }

    public static List<VALData> GetAllItems()
    {
        if (db == null || db.VALs == null) return new List<VALData>();
        return db.VALs;
    }

    public static void SaveEncrypted(VALDatabaseWrapper dbToSave, string path)
    {
        string json = JsonUtility.ToJson(dbToSave, true);
        byte[] encrypted = Encrypt(json);
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        File.WriteAllBytes(path, encrypted);
        Debug.Log($"Encrypted item data saved to: {path}");
    }
}



public static class ItemList_KeyMgr
{
    public static byte[] GetUserKey()
    {
        // 고정 키 문자열 (32바이트 이상 권장)
        string fixedKey = "MySuperSecretFixedKey1234567890!@#"; // 원하는 문자열로 변경 가능

        // SHA256 해시 → 32바이트 AES 키로 변환
        using var sha = SHA256.Create();
        byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(fixedKey));
        return hash; // AES 256bit key
    }
}

