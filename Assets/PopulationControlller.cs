using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PopulationControlller : MonoBehaviour
{
    public GameObject CharacterType = null;
    public int PopulationSize;
    public int MaxRunTime = 20;
    public int AppliedStimulusCount = 500;
    private List<GeneralCharacter> Population = new List<GeneralCharacter>();
    public Transform InitialLocation;
    public List<Vector3> Positions;
    public List<Quaternion> Rotations;
    private GameObject origin = null;
    private int EliteCount;
    private int SimCounter;

    /*
     * List of tuples, each tuple belongs to an individual and contains:
     * 1) All the horizontal motion for the current simmulation - as a List of Lists of Vector3s; 
     * 2) All the vertical motion for the current simmulation - as a List of Lists of floats;
     * 3) All the torque motion for the current simmulation - as a List of Lists of Vector3s;
     * Size - x individuals(tuples) * y frames * z rigidbodies * 1 force
    */
    //private static List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>> CurrentPopulationDNA = new List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>>();

    /*
     * List 1 - Contains individuals - 50,100,etc
     * List 2 - Contains each frame - 500, etc
     * List 3 - Contains each RigidBody torque applied - 17 rbs per individual.
     */
    private static List< List<List<Vector3>>> CurrentPopulationDNA = new List< List<List<Vector3>>>();

    // -- GeneticAlg 
    public bool done = false;
    // -- 

    public void SetEliteCount(int elites)
    {
        EliteCount = elites;
    }
    public void SetSimCounter(int simcounter)
    {
        SimCounter = simcounter;
    }
    private void InitPopulation()
    {
        origin = Instantiate(CharacterType, InitialLocation.position, Quaternion.identity);
        origin.SetActive(false);
        for (int i = 0; i < PopulationSize; i++)
        {
            List<List<Vector3>> torque = new List<List<Vector3>>();
            if (CurrentPopulationDNA.Count < PopulationSize)
            {
                // Debug.Log("Entered");
                for (int j = 0; j < AppliedStimulusCount; j++)
                {
                    List<Vector3> allrbTorque = new List<Vector3>();
                    for (int k = 0; k < 17; k++)
                    {
                        allrbTorque.Add(new Vector3(
                            UnityEngine.Random.Range(-5, 5f),
                            UnityEngine.Random.Range(-5f, 5f),
                            UnityEngine.Random.Range(-5f, 5f)));
                    }
                    torque.Add(allrbTorque);
                }
                CurrentPopulationDNA.Add(torque);
            }
            GameObject obj = CreateAndPrepare(CurrentPopulationDNA[i]);
            obj.name = "Smith" + i;
            obj.transform.SetParent(transform);

            Population.Add(obj.GetComponent<GeneralCharacter>());
        }
        Debug.Log("InitPopulation Done");
    }
    private string PrintDNA()
    {
        string result = "";
        int i = 0;
        foreach (var list in CurrentPopulationDNA)
        {
            result += ("\n" + "Smith" + i + "\n");
            string h = "";
            for (int j = 0; j < AppliedStimulusCount; j++)
            {
                h += ("[" + String.Join(",", list[j]) + "]");
            }
            result += (h + "\n");
            i++;
        }
        return result;
    }
    private GameObject CreateAndPrepare( List<List<Vector3>> t)
    {
        GameObject obj = Instantiate(origin, InitialLocation.position, Quaternion.identity);
        GeneralCharacter individual = obj.GetComponent<GeneralCharacter>();
        individual.SetDNA(t);
        individual.SetChromosomeLength(AppliedStimulusCount);
        individual.stage = 0;
        return obj;
    }
    // Start is called before the first frame update
    private void Awake()
    {
        //Random.seed = 42;   
        InitPopulation();
        IgnoreRagdollCollisions();
    }
    void Start()
    {
        if (SimCounter > 0)
        for(var i = 0; i < EliteCount; i++)
        {
            var outline1 = Population[i].gameObject.AddComponent<Outline>();

            outline1.OutlineMode = Outline.Mode.OutlineAll;
            outline1.OutlineColor = Color.green;
            outline1.OutlineWidth = 3f;
        }
    }

    // Update is called once per frame  
    void Update()
    {

    }
    public void ResetState()
    {

        //Debug.Break();
        /* Destroy(origin);
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
            for (int j = 0; j < collidersforI.Count - 1; j++)
            {
                for (int k = j + 1; k < collidersforI.Count; k++)
                    Physics.IgnoreCollision(collidersforI[j], collidersforI[k], false);
            }
        }
    }

    public float GetHeadPositionY()
    {
        return Population[0].GetHeadY();
    }
    public float GetFootPositionY()
    {
        return Population[0].GetFootY();
    }
    public List<GeneralCharacter> GetPopulation()
    {
        return Population;
    }

    public void StartMovement(int index = -1)
    {
        if (index == -1)
        {

            for (int i = 0; i < PopulationSize; i++)
            {
                Population[i].gameObject.SetActive(true);
                Population[i].Act();
            }
        }
        else
        {
            Population[index].gameObject.SetActive(true);
            Population[index].Act();
        }
    }
    public List<List<List<Vector3>>> GetPopulationDNA()
    {
        return CurrentPopulationDNA;
    }
    public void SetDNA(List< List<List<Vector3>>> DnaFromNextGen)
    {
        CurrentPopulationDNA = DnaFromNextGen;
    }
}
