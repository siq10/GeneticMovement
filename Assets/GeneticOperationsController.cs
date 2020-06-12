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
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectPopulation(List<float> FitnessList)
    {
        Debug.Log("Fitnesslist has " + FitnessList.Count + " values.");
    }


}
