using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

        Debug.Log("Fitnesslist has " + FitnessList.Count + " values.");
    }

    public void CrossOver(int charindex1, int charindex2)
    {

    }

}
