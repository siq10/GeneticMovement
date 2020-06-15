﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PopulationControlller : MonoBehaviour
{
    public GameObject CharacterType = null;
    public int PopulationSize = 50;
    public int MaxRunTime = 20;
    public int AppliedStimulusCount = 500;
    private List<GeneralCharacter> Population = new List<GeneralCharacter>();
    public Transform InitialLocation;
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
        for (int i = 0; i < PopulationSize; i++)
        {
            GameObject obj = Instantiate(CharacterType, InitialLocation.position, Quaternion.identity);
            obj.transform.SetParent(transform);
            GeneralCharacter individual = obj.GetComponent<GeneralCharacter>();
            List<List<Vector3>> horizontalForces = new List<List<Vector3>>();
            List<List<float>> verticalForces = new List<List<float>>();
            List<List<Vector3>> torque = new List<List<Vector3>>();
            for (int j = 0; j < AppliedStimulusCount; j++)
            {
                List<Vector3> allrbHforces = new List<Vector3>();
                List<float> allrbVforces = new List<float>();
                List<Vector3> allrbTorque = new List<Vector3>();
                for (int k = 0; k < individual.GetRigidBodies().Count; k++)
                {
                    allrbHforces.Add(new Vector3(UnityEngine.Random.Range(-1000f, 1000f), Random.Range(-100f, 100f), Random.Range(-1000f, 1000f)));
                    allrbVforces.Add(Random.Range(-1000f, 1000f));
                    allrbTorque.Add(new Vector3(Random.Range(-10f, 10f), Random.Range(-10f, 10f), Random.Range(-10f, 10f)));
                }
                horizontalForces.Add(allrbHforces);
                verticalForces.Add(allrbVforces);
                torque.Add(allrbTorque);
            }
            individual.SetDNA(horizontalForces, verticalForces, torque);

            var tuple = new Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>(horizontalForces,verticalForces,torque);
            CurrentPopulationDNA.Add(tuple);

            individual.SetChromosomeLength(AppliedStimulusCount);
            Debug.Log(individual);
            Population.Add(individual);
        }


        Debug.Log("InitPopulation Done");
    }
    // Start is called before the first frame update
    private void Awake()
    {
        InitPopulation();
        IgnoreRagdollCollisions();
    }
    void Start()
    {


    }

    // Update is called once per frame  
    void Update()
    {

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
        for (int i = 0; i < PopulationSize - 1; i++)
        {
            List<Collider> collidersforI = Population[i].GetAllColliders();
            for (int j = i + 1; j < PopulationSize; j++)
            {
                IgnoreCollisionsBetween(collidersforI, Population[j].GetAllColliders());
            }
        }
        Debug.Log("IgnoreRagdollCollisions Done");
    }

    public List<GeneralCharacter> GetPopulation()
    {
        return Population;
    }

    public void StartMovement()
    {
        for (int i = 0; i < Population.Count; i++)
        {
            Population[i].Act();
        }
    }
    public List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>> GetPopulationDNA()
    {
        return CurrentPopulationDNA;
    }
}
