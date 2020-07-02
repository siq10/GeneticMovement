using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;

public class EvaluationController : MonoBehaviour
{
    public List<GeneralCharacter> PopulationReference = new List<GeneralCharacter>();
    private List<float> IndividualFitnessList = new List<float>();
    private float DistanceFromStartToFinish;
    private Transform Destination;
    private int bestsmithindex;
    // -- GeneticAlg 
    public bool done = false;
    public List<float> GetFitnessList()
    {
        return IndividualFitnessList;
    }
    // -- 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPopulation(List<GeneralCharacter> population)
    {
        PopulationReference = population;
        float[] fitnesslist = new float[population.Count];
        IndividualFitnessList = fitnesslist.ToList();
    }

    public void SetDestination(Transform target)
    {
        Destination = target;
    }

    public void StartEvaluation()
    {
        StartCoroutine("ComputeFitness");
    }
    IEnumerator ComputeFitness()
    {
        int bestsmithindex = 0;

        //yield return StartCoroutine(LastFrameDistanceFromDestinationFitness());
        yield return StartCoroutine(MediumDistanceToDestinationFitness());

        Debug.Log("Smith no " + bestsmithindex + " is the best");
        NotifyFinish();
        //PopulationReference[bestsmithindex].gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
    IEnumerator LastFrameDistanceFromDestinationFitness()
    {
        for (int i = 0; i < 12; i++)
        {
            bestsmithindex = 0;
            for (int j = 0; j < IndividualFitnessList.Count; j++)
            {
                var DistanceToFinish = Vector3.Distance(PopulationReference[j].GetRigidBodies()[0].position, Destination.position);
                var NormalizedDistanceToFinish = GetNormalizedValue(DistanceToFinish, 0f, DistanceFromStartToFinish);

                IndividualFitnessList[j] = (1 - NormalizedDistanceToFinish);
                if (IndividualFitnessList[j] > IndividualFitnessList[bestsmithindex])
                {
                    bestsmithindex = j;
                }
                //.Log("Measured" + (i+1) + " - smith" + j + " value " + Vector3.Distance(PopulationReference[j].GetRigidBodies()[0].position, Destination.position));
            }
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator MediumDistanceToDestinationFitness()
    {
        for (int i = 0; i < 12; i++)
        {
            bestsmithindex = 0;
            for (int j = 0; j < IndividualFitnessList.Count; j++)
            {
                var DistanceToFinish = Vector3.Distance(PopulationReference[j].GetRigidBodies()[0].position, Destination.position);
                var NormalizedDistanceToFinish = GetNormalizedValue(DistanceToFinish, 0f, DistanceFromStartToFinish);

                IndividualFitnessList[j] += (1 - NormalizedDistanceToFinish);
                if (IndividualFitnessList[j] > IndividualFitnessList[bestsmithindex])
                {
                    bestsmithindex = j;
                }
                //.Log("Measured" + (i+1) + " - smith" + j + " value " + Vector3.Distance(PopulationReference[j].GetRigidBodies()[0].position, Destination.position));

            }

            yield return new WaitForSeconds(1f);
        }
        for (int j = 0; j < IndividualFitnessList.Count; j++)
        {
            IndividualFitnessList[j] /= 12f;
        }
    }
    public void SetDistanceFromStartToEnd(float dist)
    {
        DistanceFromStartToFinish = dist;
    }

    private void NotifyFinish()
    {
        done = true;
    }

    public void ResetState()
    {
        done = false;
    }

    private float GetNormalizedValue(float value, float minValue, float maxValue)
    {
        //if (minValue == maxValue)
        //  return 0.5f;
        /*if (value > maxValue)
            return 1f;*/
        return (value - minValue) / (maxValue - minValue);
    }


}
