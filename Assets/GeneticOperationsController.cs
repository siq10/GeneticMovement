using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditorInternal;
using UnityEngine;
using Random = UnityEngine.Random;

public enum SelectionMethod
{
    LuckWheel,
    Rank,
    Tournament
}
public class GeneticOperationsController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool done;
    public SelectionMethod SelectMethod = SelectionMethod.LuckWheel;
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

    public void SelectFittest(List<float> FitnessList)
    {
        Debug.Log("Fitnesslist has " + FitnessList.Count + " values.");
    }
    public void SetPopulationDNA(List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>> dna)
    {
        PopulationDNA = dna;
    }
    public void ComputeNextGeneration(List<float> FitnessList)
    {
        PhysicsSteps = PopulationDNA[0].Item1.Count;
        RigidBodiesPerIndividual = PopulationDNA[0].Item1[0].Count;

        Debug.Log("Fitnesslist has " + FitnessList.Count + " values.");
        CrossOver(0, 1);
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
        List<List<Vector3>> repl1 = new List<List<Vector3>>();
        List<List<Vector3>> repl2 = new List<List<Vector3>>();
        List<List<Vector3>> horizontal_X = PopulationDNA[charindex1].Item1;
        List<List<Vector3>> horizontal_Y = PopulationDNA[charindex2].Item1;

        List<List<float>> vertical_X = PopulationDNA[charindex1].Item2;
        List<List<float>> vertical_Y = PopulationDNA[charindex2].Item2;
        
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
                repl1.AddRange(horizontal_X.GetRange(startI, endI - startI));
                repl2.AddRange(horizontal_Y.GetRange(startI, endI - startI));
            }
            else
            {
                repl1.AddRange(horizontal_Y.GetRange(startI, endI - startI));
                repl2.AddRange(horizontal_X.GetRange(startI, endI - startI));
            }
            startI = endI;
            swap = !swap;
        }
        endI = PhysicsSteps;
        if (swap == false)
        {
            repl1.AddRange(horizontal_X.GetRange(startI, endI - startI));
            repl2.AddRange(horizontal_Y.GetRange(startI, endI - startI));
        }
        else
        {
            repl1.AddRange(horizontal_Y.GetRange(startI, endI - startI));
            repl2.AddRange(horizontal_X.GetRange(startI, endI - startI));
        }
        // last index to the list end.

        //printV(repl1, horizontal_X);



        //Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>> replacement = new Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>(); 
    }

}
