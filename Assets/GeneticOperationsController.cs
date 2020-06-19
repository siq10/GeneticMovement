using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
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
    public float MutationRate = 0.02f;

    private int PhysicsSteps = 500;
    private int RigidBodiesPerIndividual;
    private List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>> PopulationDNA;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public List<int> Select(List<float> FitnessList)
    {
        Debug.Log("Fitnesslist has " + FitnessList.Count + " values.");
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
        return indexes;
    }
    public void SetPopulationDNA(List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>> dna)
    {
        PopulationDNA = dna;
    }
    public void ComputeNextGeneration(List<float> FitnessList)
    {
        PhysicsSteps = PopulationDNA[0].Item1.Count;
        RigidBodiesPerIndividual = PopulationDNA[0].Item1[0].Count;
        List<int> survivors = Select(FitnessList);
        Debug.Log("Fitnesslist has " + FitnessList.Count + " values.");
        
        CrossOver(0, 1);
        Mutation(0);
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
    public void CrossOver(int charindex1, int charindex2)
    {
        List<List<Vector3>> horizontal_result_X = new List<List<Vector3>>();
        List<List<Vector3>> horizontal_result_Y = new List<List<Vector3>>();
        List<List<Vector3>> horizontal_X = PopulationDNA[charindex1].Item1;
        List<List<Vector3>> horizontal_Y = PopulationDNA[charindex2].Item1;

        List<List<float>> vertical_result_X = new List<List<float>>();
        List<List<float>> vertical_result_Y = new List<List<float>>();
        List<List<float>> vertical_X = PopulationDNA[charindex1].Item2;
        List<List<float>> vertical_Y = PopulationDNA[charindex2].Item2;

        List<List<Vector3>> torque_result_X = new List<List<Vector3>>();
        List<List<Vector3>> torque_result_Y = new List<List<Vector3>>();
        List<List<Vector3>> torque_X = PopulationDNA[charindex1].Item3;
        List<List<Vector3>> torque_Y = PopulationDNA[charindex2].Item3; 
        
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
                horizontal_result_X.AddRange(horizontal_X.GetRange(startI, endI - startI));
                horizontal_result_Y.AddRange(horizontal_Y.GetRange(startI, endI - startI));

                vertical_result_X.AddRange(vertical_X.GetRange(startI, endI - startI));
                vertical_result_Y.AddRange(vertical_Y.GetRange(startI, endI - startI));
                
                torque_result_X.AddRange(torque_X.GetRange(startI, endI - startI));
                torque_result_Y.AddRange(torque_Y.GetRange(startI, endI - startI));
            }
            else
            {
                horizontal_result_X.AddRange(horizontal_Y.GetRange(startI, endI - startI));
                horizontal_result_Y.AddRange(horizontal_X.GetRange(startI, endI - startI));

                vertical_result_X.AddRange(vertical_Y.GetRange(startI, endI - startI));
                vertical_result_Y.AddRange(vertical_X.GetRange(startI, endI - startI));

                torque_result_X.AddRange(torque_Y.GetRange(startI, endI - startI));
                torque_result_Y.AddRange(torque_X.GetRange(startI, endI - startI));

            }
            startI = endI;
            swap = !swap;
        }
        endI = PhysicsSteps;
        if (swap == false)
        {
            horizontal_result_X.AddRange(horizontal_X.GetRange(startI, endI - startI));
            horizontal_result_Y.AddRange(horizontal_Y.GetRange(startI, endI - startI));

            vertical_result_X.AddRange(vertical_X.GetRange(startI, endI - startI));
            vertical_result_Y.AddRange(vertical_Y.GetRange(startI, endI - startI));

            torque_result_X.AddRange(torque_X.GetRange(startI, endI - startI));
            torque_result_Y.AddRange(torque_Y.GetRange(startI, endI - startI));
        }
        else
        {
            horizontal_result_X.AddRange(horizontal_Y.GetRange(startI, endI - startI));
            horizontal_result_Y.AddRange(horizontal_X.GetRange(startI, endI - startI));

            vertical_result_X.AddRange(vertical_Y.GetRange(startI, endI - startI));
            vertical_result_Y.AddRange(vertical_X.GetRange(startI, endI - startI));

            torque_result_X.AddRange(torque_Y.GetRange(startI, endI - startI));
            torque_result_Y.AddRange(torque_X.GetRange(startI, endI - startI));

        }
        // last index from list  ----->  last physicstep (last frame).
 
        //printV(repl1, horizontal_X);

        Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>> replacement_X = 
            new Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>(horizontal_result_X,vertical_result_X,torque_result_X); 

        Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>> replacement_Y = 
            new Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>(horizontal_result_Y,vertical_result_Y,torque_result_Y);

        PopulationDNA[charindex1] = replacement_X;
        PopulationDNA[charindex2] = replacement_Y;
    }

    public void Mutation(int charindex)
    {
        int affected_step = Random.Range(0, PhysicsSteps);
        var horizontal_list = PopulationDNA[charindex].Item1[affected_step];
        var vertical_list = PopulationDNA[charindex].Item2[affected_step];
        var torque_list = PopulationDNA[charindex].Item3[affected_step];

        List<Vector3> allrbHforces = new List<Vector3>();
        List<float> allrbVforces = new List<float>();
        List<Vector3> allrbTorque = new List<Vector3>();
        var len = horizontal_list.Count;
        for (int i=0; i< len;i++)
        {
            allrbHforces.Add(new Vector3(UnityEngine.Random.Range(-1000f, 1000f), Random.Range(-100f, 100f), Random.Range(-1000f, 1000f)));
            allrbVforces.Add(Random.Range(-1000f, 1000f));
            allrbTorque.Add(new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f)));
        }
        horizontal_list = allrbHforces;
        vertical_list = allrbVforces;
        torque_list = allrbTorque;
    }

    private int BinarySearch(List<float> a, float item)
    {
        int start_index = 0;
        int end_index = a.Count - 1;
        int result_index = 0;
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
