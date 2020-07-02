using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public class GeneticAlg : MonoBehaviour
{
    private static int simcounter = 0;
    public PopulationControlller PopulationComponent;
    public EvaluationController EvalComponent;
    public Transform InitialLocation;
    public Transform DesiredLocation;
    public GeneticOperationsController GeneticOperationsComponent;
    public int NumberOfIterations = 20;
    private void SetRefs()
    {
        EvalComponent.SetPopulation(PopulationComponent.GetPopulation());
        EvalComponent.SetDestination(DesiredLocation);
        EvalComponent.SetDistanceFromStartToEnd(Vector3.Distance(InitialLocation.position, DesiredLocation.position));
        GeneticOperationsComponent.SetPopulationDNA(PopulationComponent.GetPopulationDNA());
    }
    // Start is called before the first frame update
    void Start()
    {
        SetRefs();
        StartCoroutine("Coordinate");

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void StartSimulation()
    {
        PopulationComponent.StartMovement();
        EvalComponent.StartEvaluation();
    }

    private void StopSimulation()
    {

    }
    private void ResetState()
    {
        PopulationComponent.ResetState();
        EvalComponent.ResetState();
    }

    IEnumerator Coordinate()
    {
        while(simcounter < NumberOfIterations)
        { 
            GeneticOperationsComponent.SetPopulationDNA(PopulationComponent.GetPopulationDNA());
            List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>> DnaFromNextGen = new List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>>();
            StartSimulation();
            while (! EvalComponent.done)
            {
                yield return new WaitForSeconds(1f);
            }
            GeneticOperationsComponent.ComputeNextGeneration(EvalComponent.GetFitnessList(),out DnaFromNextGen);
            PopulationComponent.SetDNA(DnaFromNextGen);
            ResetState();
            simcounter++;
        }

    }
}
