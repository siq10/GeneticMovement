using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SelectionMethod
{
    RouletteWheel,
    Rank,
    Tournament
}
public class GeneticOperationsController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool done;
    public SelectionMethod SelectMethod = SelectionMethod.RouletteWheel;
    public bool Elitism = true;
    public int EliteCount = 2;
    public float MutationRate = 0.02f;

    private int PhysicsSteps = 500;
    private int RigidBodiesPerIndividual;
    private List<List<List<Vector3>>> PopulationDNA;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<int> Select(List<float> FitnessList)
    {
        //Debug.Log("Fitnesslist has " + FitnessList.Count + " values.");
        //Debug.Log(String.Join(", ", FitnessList));

        List<int> indexes = new List<int>();
        switch(SelectMethod)
        {
            case SelectionMethod.RouletteWheel:
                float fitness_sum = FitnessList.Sum();
                int popsize = FitnessList.Count;
                List<float> selection_probabilities = new List<float>(popsize);
                foreach (var fitness in FitnessList)
                {
                    selection_probabilities.Add(fitness/fitness_sum);
                }
                List<float> selection_probabilities_cumulated = new List<float>(new float[popsize+1]);
                //roulette
                selection_probabilities_cumulated[0] = 0f;
                for (var j = 0; j < popsize; j++)
                {
                    selection_probabilities_cumulated[j+1] = selection_probabilities_cumulated[j] + selection_probabilities[j];
                }
                for (var i = 0; i < popsize; i++)
                {
                    var rand = Random.Range(0.0000001f, 1);
                    indexes.Add(BinarySearch(selection_probabilities_cumulated, rand));
                }
                break;
            case SelectionMethod.Rank:
                break;
            case SelectionMethod.Tournament:
                break;
        }
        Debug.Log(String.Join(", ", indexes));
        return indexes;
    }
    public void SetPopulationDNA(List< List<List<Vector3>>> dna)
    {
        PopulationDNA = dna;
    }
    public void ComputeNextGeneration(List<float> FitnessList, out List<List<List<Vector3>>> NewDna )
    {
        PhysicsSteps = PopulationDNA[0].Count;
        RigidBodiesPerIndividual = PopulationDNA[0][0].Count;
        List<List<List<Vector3>>> NewDNA = new List<List<List<Vector3>>>();
        var halfpopsize = FitnessList.Count / 2;

        List<int> survivors = Select(FitnessList);
        /*foreach(var x in PopulationDNA)
        {
            NewDNA.Add(PopulationDNA[0]);
        }*/

        for(int i= 0; i< halfpopsize ;i++)
        {
            var parent1index = survivors[Random.Range(0, survivors.Count)];
            var parent2index = survivors[Random.Range(0, survivors.Count)];
            NewDNA.AddRange(CrossOver(parent1index, parent2index));
           // Debug.Log("Crossed over: " + parent1index + ", " + parent2index);
        }
        for(int j = 0; j< NewDNA.Count;j++)
        {
            Mutation(j, NewDNA);
        }

        // Elitism - preserving the best x individuals from the current generation.
        if (Elitism == true)
        {
            int[] bestindexes = GetBestX(FitnessList);
            for (int i = 0; i < bestindexes.Length; i++)
            {
                NewDNA[i] = (DeepClone(PopulationDNA[bestindexes[i]]));
            }
            Debug.Log("Elitism: " + String.Join(", ", bestindexes));
            float[] vals = new float[bestindexes.Length];
            for (int i=0;i<vals.Length;i++)
            {
                vals[i] = FitnessList[bestindexes[i]];
            }
            Debug.Log("Elitism values: " + String.Join(", ", vals));
        }
        //PopulationDNA = NewDNA;
        NewDna = NewDNA;
       // printV(PopulationDNA[0].Item1, PopulationDNA[1].Item1);
    }
    private int[] GetBestX(List<float> FitnessList)
    {
        int[] result = new int[EliteCount];
        float[] maxvalues = new float[EliteCount];
        var maxlen = maxvalues.Length;
        for (int i =0; i< FitnessList.Count; i++)
        {
            for(int j = 0; j < maxlen; j++)
            {
                if(FitnessList[i] > maxvalues[j])
                {
                    for(int k = maxlen - 2; k >= j; k--)
                    {
                        maxvalues[k+1] = maxvalues[k];
                        result[k + 1] = result[k];
                    }
                    maxvalues[j] = FitnessList[i];
                    result[j] = i;
                    break;
                }
            }
        }
        return result;
    }

    private void printV(List<List<Vector3>> a, List<List<Vector3>> b)
    {
        for(var i =0; i < a.Count; i++)
        {
            string txta = "A: ";
            string txtb = "B: ";
            for(int j= 0; j< a[i].Count;j++)
            {
                txta += a[i][j].ToString();
                txta += ", ";
                txtb += b[i][j].ToString();
                txtb += ", ";
            }
            Debug.Log(txta);
            Debug.Log(txtb);
        }
    }
    private List<List<Vector3>> DeepClone(List<List<Vector3>> x)
    {
        List<List<Vector3>> result = new List<List<Vector3>>();
        foreach (var i in x)
        {
            var c = new List<Vector3>();
            foreach (var j in i)
            {
                c.Add(new Vector3(j.x,j.y,j.z));
            }
            result.Add(c);
        }
        return result;
    }
    private List<List<float>> DeepClone(List<List<float>> x)
    {
        List<List<float>> result = new List<List<float>>();
        foreach (var i in x)
        {
            var c = new List<float>();
            foreach (var j in i)
            {
                c.Add(j);
            }
            result.Add(c);
        }
        return result;
    }

    public List<List<List<Vector3>>> CrossOver(int charindex1, int charindex2)
    {
        List<List<Vector3>> torque_result_X = new List<List<Vector3>>();
        List<List<Vector3>> torque_result_Y = new List<List<Vector3>>();
        List<List<Vector3>> torque_X = DeepClone(PopulationDNA[charindex1]);
        List<List<Vector3>> torque_Y = DeepClone(PopulationDNA[charindex2]); 
        
        var cutcount = Random.Range(1, PhysicsSteps);
        SortedSet<int> cutindexes = new SortedSet<int>();
        
        for (int k=0; k< cutcount;k++)
        {
            cutindexes.Add(Random.Range(1, PhysicsSteps));
        }

        int startI = 0;
        int endI = 0;
        bool swap = false;
        foreach(var idx in cutindexes)
        {
            endI = idx;
            if (swap == false)
            {
                torque_result_X.AddRange(torque_X.GetRange(startI, endI - startI));
                torque_result_Y.AddRange(torque_Y.GetRange(startI, endI - startI));
            }
            else
            {
                torque_result_X.AddRange(torque_Y.GetRange(startI, endI - startI));
                torque_result_Y.AddRange(torque_X.GetRange(startI, endI - startI));
            }
            startI = endI;
            swap = !swap;
        }
        endI = PhysicsSteps;
        if (swap == false)
        {
            torque_result_X.AddRange(torque_X.GetRange(startI, endI - startI));
            torque_result_Y.AddRange(torque_Y.GetRange(startI, endI - startI));
        }
        else
        {
            torque_result_X.AddRange(torque_Y.GetRange(startI, endI - startI));
            torque_result_Y.AddRange(torque_X.GetRange(startI, endI - startI));
        }
        // last index from list  ----->  last physicstep (last frame).

        //printV(repl1, horizontal_X);

        List<List<Vector3>> replacement_X = torque_result_X;

        List<List<Vector3>> replacement_Y = torque_result_Y;

        List<List<List<Vector3>>> result = new List<List<List<Vector3>>>();
        result.Add(replacement_X);
        result.Add(replacement_Y);
        return result;
    }

    public void Mutation(int charindex, List< List<List<Vector3>>> NewDNA)
    {
        var chance = 0f;
        for (int affected_step = 0; affected_step < PhysicsSteps; affected_step++)
        {
            var torque_list = NewDNA[charindex][affected_step]; 
            var len = torque_list.Count;
            for (int i = 0; i < len; i++)
            {
                chance = Random.Range(0f, 1f);
                if (chance < MutationRate)
                {
                    Debug.Log("Smith" + charindex + " mutated gene " + affected_step);
                    torque_list[i] = (new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f)));
                }
            }
        }
    }

    private int BinarySearch(List<float> a, float item)
    {
        int result_index = 0;
        int start_index = 0;
        int end_index = a.Count - 1;
        while (start_index <= end_index)
        {
            result_index = start_index + (end_index - start_index) / 2;
            if (item > a[result_index])
                start_index = result_index + 1;
            else
                end_index = result_index - 1;
            if (a[result_index] == item)
            {
                return result_index - 1;
                //return the index before the found number.
            }
        }
        return a[result_index] < item ? result_index : result_index - 1;
        //return the index closest to the number, on the left side.
    }
}
