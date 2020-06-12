using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlg : MonoBehaviour
{
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
    }
    // Start is called before the first frame update
    void Start()
    {
        InitialLocation = PopulationComponent.InitialLocation;
        DesiredLocation = PopulationComponent.DesiredLocation;
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

    }

    IEnumerator Coordinate()
    {
        for (int i = 0; i < NumberOfIterations; i++)
        {
            StartSimulation();
            while (! EvalComponent.done)
            {
                yield return new WaitForSeconds(1f);
            }
            GeneticOperationsComponent.SelectPopulation(EvalComponent.GetFitnessList());
            break;
        }

    }
}
