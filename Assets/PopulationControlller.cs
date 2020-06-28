using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using Random = UnityEngine.Random;

public class PopulationControlller : MonoBehaviour
{
    public GameObject CharacterType = null;
    public int PopulationSize = 50;
    public int MaxRunTime = 20;
    public int AppliedStimulusCount = 500;
    private List<GeneralCharacter> Population = new List<GeneralCharacter>();
    public Transform InitialLocation;
    public List<Vector3> Positions;
    public List<Quaternion> Rotations;
    private GameObject origin = null;


    /*
     * List of tuples, each tuple belongs to an individual and contains:
     * 1) All the horizontal motion for the current simmulation - as a List of Lists of Vector3s; 
     * 2) All the vertical motion for the current simmulation - as a List of Lists of floats;
     * 3) All the torque motion for the current simmulation - as a List of Lists of Vector3s;
     * Size - x frames * x rigidbodies * 1 force
    */
    private List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>> CurrentPopulationDNA = new List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>>();


    // -- GeneticAlg 
    public bool done = false;
    // -- 
    private void InitPopulation()
    {
        origin = Instantiate(CharacterType, InitialLocation.position, Quaternion.identity);
        origin.SetActive(false);
        for (int i = 0; i < PopulationSize; i++)
        {
            GeneralCharacter individual = origin.GetComponent<GeneralCharacter>();

            List<List<Vector3>> horizontalForces = new List<List<Vector3>>();
            List<List<float>> verticalForces = new List<List<float>>();
            List<List<Vector3>> torque = new List<List<Vector3>>();
            for (int j = 0; j < AppliedStimulusCount; j++)
            {
                List<Vector3> allrbHforces = new List<Vector3>();
                List<float> allrbVforces = new List<float>();
                List<Vector3> allrbTorque = new List<Vector3>();
                for (int k = 0; k < 17; k++)
                {
                    allrbHforces.Add(new Vector3(UnityEngine.Random.Range(-1000f, 1000f),
                        UnityEngine.Random.Range(-100f, 100f),
                        UnityEngine.Random.Range(-1000f, 1000f)));
                    allrbVforces.Add(UnityEngine.Random.Range(-1000f, 1000f));
                    allrbTorque.Add(new Vector3(
                        UnityEngine.Random.Range(-10f, 10f),
                        UnityEngine.Random.Range(-10f, 10f),
                        UnityEngine.Random.Range(-10f, 10f)));
                }
                horizontalForces.Add(allrbHforces);
                verticalForces.Add(allrbVforces);
                torque.Add(allrbTorque);
            }
            GameObject obj = CreateAndPrepare(horizontalForces, verticalForces, torque);
            obj.name = "Smith" + i;
            obj.transform.SetParent(transform);

            var tuple = new Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>(horizontalForces,verticalForces,torque);
            CurrentPopulationDNA.Add(tuple);
            Population.Add(obj.GetComponent<GeneralCharacter>());
        }
        Debug.Log("InitPopulation Done");
    }

    private GameObject CreateAndPrepare(List<List<Vector3>> h, List<List<float>> v, List<List<Vector3>> t)
    {
        GameObject obj = Instantiate(origin);
        obj.transform.SetParent(transform);
        obj.name = "Smith";
        GeneralCharacter individual = obj.GetComponent<GeneralCharacter>();
        individual.SetDNA(h, v, t);
        individual.SetChromosomeLength(AppliedStimulusCount);
        individual.stage = 0;
        return obj;
    }
    // Start is called before the first frame update
    private void Awake()
    {
        Random.seed = 42;   

        InitPopulation();
        IgnoreRagdollCollisions();

    }
    void Start()
    {
       // Time.timeScale = 16f;

    }

    // Update is called once per frame  
    void Update()
    {

    }
    public void ResetState()
    {
        Debug.Break();
        Destroy(origin);
        origin = null;
        origin = Instantiate(CharacterType, InitialLocation.position, Quaternion.identity);
        origin.SetActive(false);
        for (int i = 0; i < Population.Count; i++)
        {
            Destroy(Population[i].gameObject);
            Population[i] = null;
            Population[i] = CreateAndPrepare(CurrentPopulationDNA[i].Item1, CurrentPopulationDNA[i].Item2, CurrentPopulationDNA[i].Item3).GetComponent<GeneralCharacter>();
            Population[i].name = "Smith" + i;
        }
        IgnoreRagdollCollisions();
       /* foreach (var candidate in Population)
        {
            var i = 0;
            var x = candidate.transform.GetComponentsInChildren<Transform>(true);
            foreach (Transform child in x)
            {
                var rb = child.GetComponent<Rigidbody>();
                if (rb)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }
                child.position = Positions[i];
                child.rotation = Rotations[i];
                i++;
            }
        }

        /* var rbs = candidate.GetRigidBodies();
         int rbcount = rbs.Count; 
         rbs[0].transform.position = InitialLocation.position;
         for (int i = 0; i < rbcount; i++)
         {   
             rbs[i].velocity = Vector3.zero;
             rbs[i].angularVelocity = Vector3.zero;
             rbs[i].position = Positions[i];
             rbs[i].rotation = Rotations[i];
         }*/
        /* candidate.stage = 0;
     }
     foreach (var candidate in Population)
     {
         var x = candidate.transform.GetComponentsInChildren<Transform>(true);
         foreach (Transform child in x)
         {
             var rb = child.GetComponent<Rigidbody>();
             if (rb)
             {
                // rb.isKinematic = false;
                 //rb.useGravity = true;
             }
         }
     }*/
    }

    private void IgnoreCollisionsBetween(List<Collider> allcollidersX, List<Collider> allcollidersY)
    {
        int listsize = allcollidersX.Count;
        for (int i = 0; i < listsize; i++)
        {
            for (int j = 0; j < listsize; j++)
            {
                Physics.IgnoreCollision(allcollidersX[i], allcollidersY[j]);
            }
        }
    }
    private void IgnoreRagdollCollisions()
    {
        /*for (int i = 0; i < PopulationSize - 1; i++)
        {
            List<Collider> collidersforI = Population[i].GetAllColliders();
            for (int j = i + 1; j < PopulationSize; j++)
            {
                IgnoreCollisionsBetween(collidersforI, Population[j].GetAllColliders());
            }
        }
        */

        for (int i = 0; i < PopulationSize; i++)
        {
            List<Collider> collidersforI = Population[i].GetAllColliders();
            for (int j = 0; j < collidersforI.Count-1; j++)
            {
                for (int k = j + 1; k < collidersforI.Count; k++)
                    Physics.IgnoreCollision(collidersforI[j], collidersforI[k],false);
            }
        }
    }

    public List<GeneralCharacter> GetPopulation()
    {
        return Population;
    }

    public void StartMovement()
    {
        for (int i = 0; i < Population.Count; i++)
        {
            Population[i].gameObject.SetActive(true);
            Population[i].Act();
        }
    }
    public List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>> GetPopulationDNA()
    {
        return CurrentPopulationDNA;
    }
}
