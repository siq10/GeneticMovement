using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : MonoBehaviour
{
    private List<Rigidbody> allrigidbodies = new List<Rigidbody>();
    Transform[] allchildren = { };
    public int index = 0;
    // Start is called before the first frame update
    void Start()
    {
        var Population = GetRigidBodies();
        foreach (var x in Population)
        {
            x.GetComponent<Rigidbody>().centerOfMass = new Vector3(0, 0, 0);

            x.maxAngularVelocity = 100f;
            x.angularDrag = 0;
        }
        Debug.Log(Population[index].gameObject.name);

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

    // Update is called once per frame
    void FixedUpdate()
    {
        var x = GetRigidBodies();
        //Debug.Log(x[index].gameObject.name);
        //x[index].AddTorque(x[index].transform.right, ForceMode.Impulse);


        foreach (var bone in x)
        {
            bone.AddRelativeTorque(bone.transform.right*3, ForceMode.Impulse);
        }
    }
}
