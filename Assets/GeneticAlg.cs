﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneticAlg : MonoBehaviour
{
    private static int simcounter = 0;
    public PopulationControlller PopulationComponent;
    public EvaluationController EvalComponent;
    public Transform InitialLocation;
    public Transform DesiredLocation;
    public GeneticOperationsController GeneticOperationsComponent;
    public int NumberOfIterations = 20;

    private void Awake()
    {
        if(MenuController.IsReplayRun == true)
        {
            gameObject.SetActive(false);
        }
    }
    private void SetRefs()
    {
        EvalComponent.SetPopulation(PopulationComponent.GetPopulation());
        EvalComponent.SetTransforms(InitialLocation,DesiredLocation);
        EvalComponent.SetDistanceFromStartToEnd(Vector3.Distance(InitialLocation.position, DesiredLocation.position));
        GeneticOperationsComponent.SetPopulationDNA(PopulationComponent.GetPopulationDNA());
        PopulationComponent.SetEliteCount(GeneticOperationsComponent.EliteCount);
        PopulationComponent.SetSimCounter(simcounter);
    }
    // Start is called before the first frame update
    void Start()
    {
        SetRefs();
        if (simcounter < NumberOfIterations)
        {
            Debug.Log("started step " + simcounter);
            StartCoroutine("Coordinate");
        }

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
        //PopulationComponent.ResetState();
        EvalComponent.ResetState();
        List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>> CurrentPopulationDNA;
        List<float> scorelist = new List<float>();
        //ReplUtils.LoadSimmulation("save0", out CurrentPopulationDNA, out scorelist);
        //PopulationComponent.SetDNA(CurrentPopulationDNA);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
    bool ReadyToGo()
    {

        return PopulationComponent.ready;
    }

    IEnumerator Coordinate()
    {
        GeneticOperationsComponent.SetPopulationDNA(PopulationComponent.GetPopulationDNA());
        List<List<List<Vector3>>> DnaFromNextGen = new List<List<List<Vector3>>>();
        while(!ReadyToGo())
        {
            yield return null;
        }
        StartSimulation();
        EvalComponent.SetHeadPositionY(PopulationComponent.GetHeadPositionY());
        EvalComponent.SetFootPositionY(PopulationComponent.GetFootPositionY());
            
        while (! EvalComponent.done)
        {
            yield return new WaitForSeconds(0.5f);
        }
        ReplUtils.SaveSimmulation(PopulationComponent.GetPopulationDNA(), EvalComponent.GetFitnessList(),simcounter);

        GeneticOperationsComponent.ComputeNextGeneration(EvalComponent.GetFitnessList(),out DnaFromNextGen);
        PopulationComponent.SetDNA(DnaFromNextGen);
        ResetState();
        simcounter++;
    }
}
