using System.Collections;
using System.Collections.Generic;
using System.Xml.XPath;
using TMPro;
using UnityEngine;

[System.Serializable]
public class GeneralCharacter : MonoBehaviour
{
    //17 movable rigidbodies

    //private List<List<Vector3>> HorizontalMovementDNA = new List<List<Vector3>>();
    private List<List<Vector3>> torqueDNA = new List<List<Vector3>>();
    //private List<List<float>> VerticalMovementDNA = new List<List<float>>();

    private List<Rigidbody> allrigidbodies = new List<Rigidbody>();
    public int stage = 0;
    public float Power = 5f;
    private int ChromosomeLength = 500;

    private bool grounded = true;
    // horizontal,vertical,torque
    // vector3 * DNAlength
    [SerializeField]
    public List<string> TerrainList = new List<string>();

    public bool started = false, finished = false;

    private Rigidbody headrb;
    Transform[] allchildren = { };
    private List<Collider> allcolliders = new List<Collider>();
    float distToGround;
    List<Rigidbody> limbs = new List<Rigidbody>();

    public void SetDNA(List<List<Vector3>> torque)
    {
        torqueDNA.AddRange(torque);
    }
    public void SetChromosomeLength(int length)
    {
        ChromosomeLength = length;
    }

    void Start()
    {
        var rbs = GetRigidBodies();
       /*GetChildren();
       foreach(var x in allchildren)
        {
            var renderer = x.GetComponent<Renderer>();
            if(renderer)
            {
                renderer.enabled = false;
            }
        }*/
      /*  foreach (Rigidbody obj in rbs)
        {
            obj.maxAngularVelocity = 100f;
        }*/
    }
    private void Awake()
    {
        bool foundhead = false;
        var children = GetChildren();
        var x = new List<int>();
        for (int i = 0; i < children.Length; i++)
        {
            
            if (!foundhead && allchildren[i].CompareTag("CHead"))
            {
                foundhead = true;
                headrb = allchildren[i].GetComponent<Rigidbody>();
               // Debug.Log(name + allchildren[i].name);
            }
            if (allchildren[i].CompareTag("limb"))
            {
                limbs.Add(allchildren[i].GetComponent<Rigidbody>());
                /*if (limbs[limbs.Count - 1].gameObject.name == "LeftFoot" || limbs[limbs.Count - 1].gameObject.name == "RightToeBase" || limbs[limbs.Count - 1].gameObject.name == "LeftToeBase" || limbs[limbs.Count - 1].gameObject.name == "RightFoot")
                {
                    x.Add(limbs.Count - 1);
                    Debug.Log(limbs[limbs.Count - 1].gameObject.name + " " + (limbs.Count-1));
                }*/
            }

        }
        /*string y = "";
        foreach (var p in x)
        {
            y = y + p + ", ";
        }
        Debug.Log(y);
        Debug.LogError(1);*/
        //Debug.Log("Limbs layer count: " + limbs.Count);


    }

    // Update is called once per frame
    void Update()
    {

    }
    void FixedUpdate()
    {
        if (started && !finished)
        {
            for (int i = 0; i < allrigidbodies.Count; i++)
            {
                //Debug.Log(name + " " + allrigidbodies[i].name + " " + "Velocity = " + allrigidbodies[i].velocity);
                //Debug.Log(name + " " + allrigidbodies[i].name + " " + "Ang Velocity = " + allrigidbodies[i].angularVelocity);

                //HorizontalMovement(allrigidbodies[i], HorizontalMovementDNA[stage][i], torqueDNA[stage][i]);
                //VerticalMovement(allrigidbodies[i], VerticalMovementDNA[stage][i]);
                Move(allrigidbodies[i], torqueDNA[stage][i]);
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

    public float GetHeadY()
    {
       // Debug.Log(name +headrb);
        return headrb.transform.position.y;
    }
    public float GetFootY()
    {
        return limbs[1].transform.position.y;
    }

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
    /*bool IsGrounded()
    {
        bool result = false;
        for (var i = 0; i < limbs.Count; i++)
        {
            if (limbs[i].grounded)
            {
                result = true;
                break;
            }
        }
        return result;
    }*/
    /*void HorizontalMovement(Rigidbody rb, Vector3 horizontalforces, Vector3 torque)
    {
        rb.AddTorque(torque*Time.fixedDeltaTime,ForceMode.Impulse);
        if (IsGrounded())
        {
            rb.AddForce(Vector3.Scale(new Vector3(1 * Time.fixedDeltaTime, 1 * Time.fixedDeltaTime, 1 * Time.fixedDeltaTime), horizontalforces), ForceMode.Impulse);
        }
    }*/
    void Move(Rigidbody rb, Vector3 torque)
    {
        rb.AddTorque(torque*100, ForceMode.Force);
        //rb.AddTorque(new Vector3(1, 0, 0) * Power, ForceMode.Impulse);

    }
    /*void VerticalMovement(Rigidbody rb, float verticalforce)
    {
        if(IsGrounded())
        {
            rb.AddForce(new Vector3(0, 1*Time.fixedDeltaTime, 0) * verticalforce,ForceMode.Impulse);
        }
    }*/
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
        finished = false;
        started = true;
    }



}
