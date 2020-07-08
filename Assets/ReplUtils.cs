using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class ReplUtils
{
    public static int populationsize = 100;
    public static int steps = 500;
    public static int filecount = 0;
    public static void SaveSimmulation(List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>> CurrentPopulationDNA, List<float> scorelist)
    {
        var values = ConvertToSerializable(CurrentPopulationDNA);
        GetSaveFiles();
        SaveData data = new SaveData(values, scorelist);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(Path.Combine(Application.persistentDataPath, "save" + (filecount+1)%5), FileMode.Create);
        bf.Serialize(file, data);
        file.Close();
    }

    public static List<string> GetSaveFiles()
    {
        var saveslist = Directory.GetFiles(Application.persistentDataPath);

        List<string> filenames = new List<string>();
        foreach (var x in saveslist)
        {
            filenames.Add(Path.GetFileName(x));
        }
        filecount = filenames.Count;
        return filenames;
    }
    public static List<float> ConvertToSerializable(List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>> CurrentPopulationDNA)
    {
        List<float> values = new List<float>(populationsize * 500 * (17 + 51 + 51));
        foreach (var tup in CurrentPopulationDNA)
        {
            var t1 = tup.Item1;
            var t2 = tup.Item2;
            var t3 = tup.Item3;
            foreach (var x in t1)
            {
                foreach (var y in x)
                {
                    values.Add(y.x);
                    values.Add(y.y);
                    values.Add(y.z);
                }
            }
            foreach (var x in t2)
            {
                values.AddRange(x);
            }
            foreach (var x in t3)
            {
                foreach (var y in x)
                {
                    values.Add(y.x);
                    values.Add(y.y);
                    values.Add(y.z);
                }
            }
        }
        return values;
    }

    public static void LoadSimmulation(string filename, out List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>> CurrentPopulationDNA, out List<float> scorelist)
    {
        CurrentPopulationDNA = new List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>>();
        scorelist = new List<float>();
        string path = Path.Combine(Application.persistentDataPath, filename);
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fs = new FileStream(path, FileMode.Open);
            SaveData data = formatter.Deserialize(fs) as SaveData;
            fs.Close();
            ConvertToGameData(data, out CurrentPopulationDNA, out scorelist);
        }
        else
        {
            Debug.Log("Save file does not exist.");
        }
    }

    public static void ConvertToGameData(SaveData data, out List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>> CurrentPopulationDNA, out List<float> scorelist)
    {
        CurrentPopulationDNA = new List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>>();
        scorelist = new List<float>();
        if (data != null)
        {
            List<float> dna = data.dna;
            List<float> score = data.score;
            int v3values = 51;
            int floatvalues = 17;
            int sum1 = v3values + floatvalues;
            int sum2 = 2 * v3values + floatvalues;
            int idx = 0;
            for (int i = 0; i < populationsize; i++)
            {
                List<List<Vector3>> hforces = new List<List<Vector3>>();
                List<List<float>> vforces = new List<List<float>>();
                List<List<Vector3>> torque = new List<List<Vector3>>();
                for (int p = 0; p < steps; p++)
                {
                    List<Vector3> hlist = new List<Vector3>();
                    for (int o = 0; o < 17; o++)
                    {
                        hlist.Add(new Vector3(dna[idx++], dna[idx++], dna[idx++]));
                    }
                    hforces.Add(hlist);
                }
                for (int p = 0; p < steps; p++)
                {
                    List<float> vlist = new List<float>();
                    for (int o = 0; o < 17; o++)
                    {
                        vlist.Add(dna[idx++]);
                    }
                    vforces.Add(vlist);
                }
                for (int p = 0; p < steps; p++)
                {
                    List<Vector3> tlist = new List<Vector3>();
                    for (int o = 0; o < 17; o++)
                    {
                        tlist.Add(new Vector3(dna[idx++], dna[idx++], dna[idx++]));
                    }
                    torque.Add(tlist);
                }
                CurrentPopulationDNA.Add(new Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>(hforces, vforces, torque));
            }
        }
        //Debug.LogError(CurrentPopulationDNA.Count);

    }
}


