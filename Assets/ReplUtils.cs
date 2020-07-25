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
    public static int last = 0;
    public static void SaveSimmulation(List<List<List<Vector3>>> CurrentPopulationDNA, List<float> scorelist)
    {
        var values = ConvertToSerializable(CurrentPopulationDNA);
        GetSaveFiles();
        SaveData data = new SaveData(values, scorelist);
        BinaryFormatter bf = new BinaryFormatter();
        Debug.Log("Saved in " + "save" + (filecount + last) % 5);
        FileStream file = File.Open(Path.Combine(Application.persistentDataPath, "save" + (filecount+last)%5), FileMode.Create);
        bf.Serialize(file, data);
        file.Close();
        last = (last + 1) % 5;
    }

    public static List<string> GetSaveFiles()
    {
        var saveslist = Directory.GetFiles(Application.persistentDataPath);
        List<string> filenames = new List<string>();
        foreach (var x in saveslist)
        {
            filenames.Add(Path.GetFileName(x));
        }
        filenames = filenames.FindAll(s => s.Contains("save"));
        filecount = filenames.Count;
        return filenames;
    }
    public static List<float> ConvertToSerializable(List<List<List<Vector3>>> CurrentPopulationDNA)
    {
        List<float> values = new List<float>(populationsize * 500 *  51);
        foreach (var list in CurrentPopulationDNA)
        {
            foreach (var x in list)
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

    public static void LoadSimmulation(string filename, out List<List<List<Vector3>>> CurrentPopulationDNA, out List<float> scorelist)
    {
        CurrentPopulationDNA = new List<List<List<Vector3>>>();
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

    public static void ConvertToGameData(SaveData data, out List<List<List<Vector3>>> CurrentPopulationDNA, out List<float> scorelist)
    {
        CurrentPopulationDNA = new List<List<List<Vector3>>>();
        scorelist = new List<float>();
        if (data != null)
        {
            List<float> dna = data.dna;
            List<float> score = data.score;

            int idx = 0;
            for (int i = 0; i < populationsize; i++)
            {
                List<List<Vector3>> torque = new List<List<Vector3>>();
                for (int p = 0; p < steps; p++)
                {
                    List<Vector3> tlist = new List<Vector3>();
                    for (int o = 0; o < 17; o++)
                    {
                        tlist.Add(new Vector3(dna[idx++], dna[idx++], dna[idx++]));
                    }
                    torque.Add(tlist);
                }
                CurrentPopulationDNA.Add(torque);
            }
        }
        //Debug.LogError(CurrentPopulationDNA.Count);

    }
}


