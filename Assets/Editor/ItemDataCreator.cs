#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class ItemDataCreator
{
    [MenuItem("Tools/Encrypt Item Data (from CSV)")]
    public static void SaveFromCSV()
    {
        //string ItData_csvPath = Path.Combine(Application.dataPath, "_Source/Script/ItemData/items.csv");

        string RWPData_csvPath = Path.Combine(Application.dataPath, "_Source/Script/ItemData/RWPs.csv");
        string MWPData_csvPath = Path.Combine(Application.dataPath, "_Source/Script/ItemData/MWPs.csv");

        string PtData_csvPath = Path.Combine(Application.dataPath, "_Source/Script/ItemData/Parts.csv");

        string CONData_csvPath = Path.Combine(Application.dataPath, "_Source/Script/ItemData/CONs.csv");
        string MATData_csvPath = Path.Combine(Application.dataPath, "_Source/Script/ItemData/MATs.csv");
        string VALData_csvPath = Path.Combine(Application.dataPath, "_Source/Script/ItemData/VALs.csv");

        if ( !File.Exists(PtData_csvPath))
        {
            Debug.LogError("CSV file not found: " + PtData_csvPath);
            return;
        }


        //------------------------------------------------------------------------------------------------
        //var It_db = new ItemDatabaseWrapper { Items = new List<ItemData>() };
        //string[] It_lines = File.ReadAllLines(ItData_csvPath);

        //for (int i = 1; i < It_lines.Length; i++) // 첫 줄은 헤더
        //{
        //    string[] cols = It_lines[i].Split(',');
        //    if (cols.Length < 3) continue;

        //    It_db.Items.Add(new ItemData
        //    {
        //        //Id = TryParseInt(cols[0]),
        //        Id = cols[0],
        //        Types = cols[1],
        //        Parts = cols[2],
        //        Name = cols[3],
        //        Description = cols[4],

        //        Delay = TryParseInt(cols[5]),
        //        BuffTime = TryParseInt(cols[6]),
        //        MaxHP = TryParseInt(cols[7]),
        //        MaxMP = TryParseInt(cols[8]),
        //        HP = TryParseInt(cols[9]),
        //        MP = TryParseInt(cols[10]),
        //        STR = TryParseInt(cols[11]),
        //        INT = TryParseInt(cols[12]),
        //        DEX = TryParseInt(cols[13]),
        //        ATK = TryParseInt(cols[14]),
        //        DEF = TryParseInt(cols[15]),
        //        Kg = TryParseInt(cols[16])
        //    });
        //}

        //string It_outDir = Path.Combine(Application.streamingAssetsPath, "ItemData");
        //Directory.CreateDirectory(It_outDir);
        //string It_outPath = Path.Combine(It_outDir, "items.bytes");

        //ItDatabaseManager.SaveEncrypted(It_db, It_outPath);

        //------------------------------------------------------------------------------------------------

        var RWP_db = new RWPDatabaseWrapper { RWPs = new List<RWPData>() };
        string[] RWP_lines = File.ReadAllLines(RWPData_csvPath);

        for (int i = 1; i < RWP_lines.Length; i++) // 첫 줄은 헤더
        {
            string[] cols = RWP_lines[i].Split(',');
            if (cols.Length < 3) continue;

            RWP_db.RWPs.Add(new RWPData
            {
                //Id = TryParseInt(cols[0]),
                Id = cols[0],
                Type = cols[1],
                Name = cols[2],
                Rarity = cols[3],
                Description = cols[4],

                MaxHP = TryParseInt(cols[5]),
                MaxMP = TryParseInt(cols[6]),
                STR = TryParseInt(cols[7]),
                INT = TryParseInt(cols[8]),
                DEX = TryParseInt(cols[9]),
                ATK = TryParseInt(cols[10]),
                DEF = TryParseInt(cols[11]),
            });
        }

        string RWP_outDir = Path.Combine(Application.streamingAssetsPath, "RWPData");
        Directory.CreateDirectory(RWP_outDir);
        string RWP_outPath = Path.Combine(RWP_outDir, "RWPs.bytes");

        RWPDatabaseManager.SaveEncrypted(RWP_db, RWP_outPath);

        //------------------------------------------------------------------------------------------------

        var MWP_db = new MWPDatabaseWrapper { MWPs = new List<MWPData>() };
        string[] MWP_lines = File.ReadAllLines(MWPData_csvPath);

        for (int i = 1; i < MWP_lines.Length; i++) // 첫 줄은 헤더
        {
            string[] cols = MWP_lines[i].Split(',');
            if (cols.Length < 3) continue;

            MWP_db.MWPs.Add(new MWPData
            {
                //Id = TryParseInt(cols[0]),
                Id = cols[0],
                Type = cols[1],
                Name = cols[2],
                Rarity = cols[3],
                Description = cols[4],

                MaxHP = TryParseInt(cols[5]),
                MaxMP = TryParseInt(cols[6]),
                STR = TryParseInt(cols[7]),
                INT = TryParseInt(cols[8]),
                DEX = TryParseInt(cols[9]),
                ATK = TryParseInt(cols[10]),
                DEF = TryParseInt(cols[11]),
            });
        }

        string MWP_outDir = Path.Combine(Application.streamingAssetsPath, "MWPData");
        Directory.CreateDirectory(MWP_outDir);
        string MWP_outPath = Path.Combine(MWP_outDir, "MWPs.bytes");

        MWPDatabaseManager.SaveEncrypted(MWP_db, MWP_outPath);

        //------------------------------------------------------------------------------------------------

        var Pt_db = new PartsDatabaseWrapper { Parts = new List<PartsData>() };
        string[] Pt_lines = File.ReadAllLines(PtData_csvPath);

        for (int i = 1; i < Pt_lines.Length; i++) // 첫 줄은 헤더
        {
            string[] cols = Pt_lines[i].Split(',');
            if (cols.Length < 3) continue;

            Pt_db.Parts.Add(new PartsData
            {
                //Id = TryParseInt(cols[0]),
                Id = cols[0],
                Type = cols[1],
                Name = cols[2],
                Description = cols[3],

                MaxHP = TryParseInt(cols[4]),
                MaxMP = TryParseInt(cols[5]),
                STR = TryParseInt(cols[6]),
                INT = TryParseInt(cols[7]),
                DEX = TryParseInt(cols[8]),
                ATK = TryParseInt(cols[9]),
                DEF = TryParseInt(cols[10]),
            });
        }

        string Pt_outDir = Path.Combine(Application.streamingAssetsPath, "PartData");
        Directory.CreateDirectory(Pt_outDir);
        string Pt_outPath = Path.Combine(Pt_outDir, "Parts.bytes");

        PartDatabaseManager.SaveEncrypted(Pt_db, Pt_outPath);

        //------------------------------------------------------------------------------------------------
        var CON_db = new CONDatabaseWrapper { CONs = new List<CONData>() };
        string[] CON_lines = File.ReadAllLines(CONData_csvPath);

        for (int i = 1; i < CON_lines.Length; i++) // 첫 줄은 헤더
        {
            string[] cols = CON_lines[i].Split(',');
            if (cols.Length < 3) continue;

            CON_db.CONs.Add(new CONData
            {
                //Id = TryParseInt(cols[0]),
                Id = cols[0],
                Type = cols[1],
                Name = cols[2],
                Rarity = cols[3],
                Description = cols[4],

                Delay = TryParseInt(cols[5]),
                BuffTime = TryParseInt(cols[6]),
                MaxHP = TryParseInt(cols[7]),
                MaxMP = TryParseInt(cols[8]),
                HP = TryParseInt(cols[9]),
                MP = TryParseInt(cols[10]),
                STR = TryParseInt(cols[11]),
                INT = TryParseInt(cols[12]),
                DEX = TryParseInt(cols[13]),
                ATK = TryParseInt(cols[14]),
                DEF = TryParseInt(cols[15]),
            });
        }

        string CON_outDir = Path.Combine(Application.streamingAssetsPath, "CONData");
        Directory.CreateDirectory(CON_outDir);
        string CON_outPath = Path.Combine(CON_outDir, "CONs.bytes");

        CONDatabaseManager.SaveEncrypted(CON_db, CON_outPath);

        //------------------------------------------------------------------------------------------------
        var MAT_db = new MATDatabaseWrapper { MATs = new List<MATData>() };
        string[] MAT_lines = File.ReadAllLines(MATData_csvPath);

        for (int i = 1; i < MAT_lines.Length; i++) // 첫 줄은 헤더
        {
            string[] cols = MAT_lines[i].Split(',');
            if (cols.Length < 3) continue;

            MAT_db.MATs.Add(new MATData
            {
                //Id = TryParseInt(cols[0]),
                Id = cols[0],
                Name = cols[1],
                Rarity = cols[2],
                Description = cols[3],
            });
        }

        string MAT_outDir = Path.Combine(Application.streamingAssetsPath, "MATData");
        Directory.CreateDirectory(MAT_outDir);
        string MAT_outPath = Path.Combine(CON_outDir, "MATs.bytes");

        MATDatabaseManager.SaveEncrypted(MAT_db, MAT_outPath);

        //------------------------------------------------------------------------------------------------
        var VAL_db = new VALDatabaseWrapper { VALs = new List<VALData>() };
        string[] VAL_lines = File.ReadAllLines(VALData_csvPath);

        for (int i = 1; i < VAL_lines.Length; i++) // 첫 줄은 헤더
        {
            string[] cols = VAL_lines[i].Split(',');
            if (cols.Length < 3) continue;

            VAL_db.VALs.Add(new VALData
            {
                //Id = TryParseInt(cols[0]),
                Id = cols[0],
                Name = cols[1],
                Rarity = cols[2],
                Description = cols[3],
            });
        }

        string VAL_outDir = Path.Combine(Application.streamingAssetsPath, "VALData");
        Directory.CreateDirectory(VAL_outDir);
        string VAL_outPath = Path.Combine(VAL_outDir, "VALs.bytes");

        VALDatabaseManager.SaveEncrypted(VAL_db, VAL_outPath);

        //------------------------------------------------------------------------------------------------
    }

    private static int TryParseInt(string value)
    {
        if (int.TryParse(value, out int result))
            return result;
        Debug.LogWarning($"정수 변환 실패: '{value}'");
        return 0; // 기본값 또는 원하는 fallback 값
    }

}
#endif
