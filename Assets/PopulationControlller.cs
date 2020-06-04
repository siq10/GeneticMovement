using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationControlller : MonoBehaviour
{
    public GameObject CharacterType = null;
    public int PopulationSize = 50;
    public int MaxRunTime = 20;
    public int AppliedForcesCount = 500;
    private List<GeneralCharacter> Population = new List<GeneralCharacter>();
    public Transform InitialLocation;
    public Transform DesiredLocation;

    private void InitPopulation()
    {
        for (int i = 0; i < PopulationSize; i++)
        {
            GameObject obj = Instantiate(CharacterType, InitialLocation.position, Quaternion.identity);
            obj.transform.SetParent(transform);
            GeneralCharacter individual = obj.GetComponent<GeneralCharacter>();
            List<Vector3> horizontalForces = new List<Vector3>();
            List<float> verticalForces = new List<float>();
            List<Vector3> torque = new List<Vector3>();
            for (int j = 0; j < AppliedForcesCount; j++)
            {
                horizontalForces.Add(new Vector3(Random.Range(-1000f, 1000f), Random.Range(-100f, 100f), Random.Range(-1000f, 1000f)));
                verticalForces.Add(Random.Range(-1000f, 1000f));
                torque.Add(new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f)));
            }
            individual.SetDNA(horizontalForces, verticalForces, torque);
            individual.SetChromosomeLength(AppliedForcesCount);
            Debug.Log(individual);
            Population.Add(individual);
        }

        for (int i = 0; i < PopulationSize; i++)
        {
            List<Collider> collidersforI = Population[i].GetAllColliders();
            Debug.Log("Done");
        }

    }
    // Start is called before the first frame update
    void Start()
    {
        InitPopulation();

    }

    // Update is called once per frame  
    void Update()
    {
        
    }

    private void StartSimulation()
    {

    }

    private void StopSimulation()
    {

    }
    private void ResetState()
    {

    }
}
