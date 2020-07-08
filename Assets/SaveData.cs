using System;
using System.Collections.Generic;
using System.ComponentModel;

[System.Serializable]
public class SaveData
{
    public SaveData() { }
    public SaveData(SaveData data)
    {
        dna = data.dna;
        score = data.score;
    }
     public SaveData(List<float> dnalist, List<float> scorelist)
       {
           dna = dnalist;
           score = scorelist;
      }

    public List<float> dna;
    public List<float> score;
}
