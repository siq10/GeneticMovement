﻿using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using UnityEngine;

[System.Serializable]
public class GeneralCharacter : MonoBehaviour
{
    //17 movable rigidbodies
    private List<List<Vector3>> HorizontalMovementDNA = new List<List<Vector3>>();
    private List<List<Vector3>> torqueDNA = new List<List<Vector3>>();
    private List<List<float>> VerticalMovementDNA = new List<List<float>>();
    private List<Rigidbody> allrigidbodies = new List<Rigidbody>();
    private int stage = 0;
    private int ChromosomeLength = 500;
    // horizontal,vertical,torque
    // vector3 * DNAlength
    [SerializeField]
    public List<string> TerrainList = new List<string>();

    public bool started = false, finished = false;

    private Rigidbody headrb;
    Transform[] allchildren = { };
    private List<Collider> allcolliders = new List<Collider>();
    float distToGround;
    List<GameObject> limbs = new List<GameObject>();

    public void SetDNA(List<List<Vector3>> horizontalMovement, List<List<float>> verticalMovement, List<List<Vector3>> torque)
    {
        HorizontalMovementDNA.AddRange(horizontalMovement);
        VerticalMovementDNA.AddRange(verticalMovement);
        torqueDNA.AddRange(torque);
    }
    public void SetChromosomeLength(int length)
    {
        ChromosomeLength = length;
    }

    void Start()
    {

        for (int i = 0; i < GetChildren().Length; i++)
        {
            if (allchildren[i].CompareTag("CHead"))
            {
                headrb = allchildren[i].GetComponent<Rigidbody>();
                Debug.Log(allchildren[i].name);

            }
            if (allchildren[i].gameObject.layer == 10)
            {
                limbs.Add(allchildren[i].gameObject);
            }
        }
        Debug.Log("Limbs layer count: " + limbs.Count);

    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        if(started && !finished)
        {
            for(int i =0; i< GetRigidBodies().Count;i++)
            {
                HorizontalMovement(allrigidbodies[i], HorizontalMovementDNA[stage][i], torqueDNA[stage][i]);
                VerticalMovement(allrigidbodies[i], VerticalMovementDNA[stage][i]);
            }
            stage++;
            if (stage == ChromosomeLength)
                finished = true;
        }
        //RandomMovement();
        //Debug.Log(GetHeadYcoordinate());

    }
    /*void RandomMovement()
    {
        int count = 0;
        for (int i = 1; i < allchildren.Length; i++)
        {

            var rb = allchildren[i].GetComponent<Rigidbody>();

            if (rb)
            {
                //Debug.Log(allchildren[i].name);
                count++;
                
                HorizontalMovement(rb);
                VerticalMovement(rb);
            }

            //child is your child transform
        }

    }*/
    public List<Rigidbody> GetRigidBodies()
    {
        if (allrigidbodies.Count == 0)
        {
            for (int i = 1; i < GetChildren().Length; i++)
            {
                var rb = allchildren[i].GetComponent<Rigidbody>();
                if (rb)
                {
                    allrigidbodies.Add(rb);
                }
            }
        }
        return allrigidbodies;

    }
    public Transform[] GetChildren()
    {
        if (allchildren.Length == 0)
        {
            allchildren = transform.GetComponentsInChildren<Transform>(true);
        }
        return allchildren;
    }
    bool IsGrounded()
    {
        bool result = false;
        for (var i = 0; i < limbs.Count; i++)
        {
            RaycastHit[] hits;
            hits = Physics.RaycastAll(limbs[i].transform.position, new Vector3(0, -1, 0), limbs[i].GetComponent<Collider>().bounds.extents.y + 0.1f);
            for (int j = 0; j < hits.Length; j++)
            {
                if(TerrainList.Contains(hits[j].transform.name) )
                {
                    Debug.DrawRay(limbs[i].transform.position, new Vector3(0, -1, 0) * (limbs[i].GetComponent<Collider>().bounds.extents.y + 0.1f), Color.red);

                    result = true;
                    Debug.Log(hits[j].transform.name + " is " + hits[j].distance + " units away from " + limbs[i].name);
                    break;
                }
            }
            if(result == true)
            {
                break;
            }
        }
        return result;

    }
    void HorizontalMovement(Rigidbody rb, Vector3 horizontalforces, Vector3 torque)
    {
        rb.AddTorque(torque);
        rb.AddForce(Vector3.Scale(new Vector3(1, 1, 1), horizontalforces), ForceMode.Force);
    }
    void VerticalMovement(Rigidbody rb, float verticalforce)
    {
        if(IsGrounded())
        {
            rb.AddForce(new Vector3(0, 1, 0) * verticalforce);
        }
    }
    float GetHeadYcoordinate()
    {
        return headrb.position.y;
    }


    public List<Collider> GetAllColliders()
    {
        List<Collider> result = new List<Collider>();
        if(allcolliders.Count == 0)
        {
            for (int j = 0; j < GetChildren().Length; j++)
            {
                if (allchildren[j].GetComponent<Collider>() != null)
                {
                    result.Add(allchildren[j].GetComponent<Collider>());
                }
            }
            allcolliders = result;
        }
        else
        {
            result = allcolliders;
        }
        return result;
    }
    public void Act()
    {
        started = true;
    }

}
