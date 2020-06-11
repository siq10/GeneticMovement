using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EvaluationController : MonoBehaviour
{
    public List<GeneralCharacter> PopulationReference = new List<GeneralCharacter>();
    private List<float> IndividualFitnessList = new List<float>();
    private Transform Destination;
    // Start is called before the first frame update
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
        for (int i = 0; i<11; i++)
        {
            bestsmithindex = 0;
            for (int j = 0; j < IndividualFitnessList.Count; j++)
            {
                IndividualFitnessList[j] = Vector3.Distance(PopulationReference[j].GetRigidBodies()[0].position, Destination.position);
                if (IndividualFitnessList[j] < IndividualFitnessList[bestsmithindex])
                {
                    bestsmithindex = j;
                }
                //.Log("Measured" + (i+1) + " - smith" + j + " value " + Vector3.Distance(PopulationReference[j].GetRigidBodies()[0].position, Destination.position));

            }
            yield return new WaitForSeconds(1f);
        }
        Debug.Log("Smith no " + bestsmithindex + " is the best");
        var outline = PopulationReference[bestsmithindex].gameObject.AddComponent<Outline>();

        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.red;
        outline.OutlineWidth = 5f;
        //PopulationReference[bestsmithindex].gameObject.GetComponent<Renderer>().material.color = Color.red;
    }


}
