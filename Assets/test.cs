using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        var x = GetComponent<Rigidbody>();
        x.AddRelativeTorque(new Vector3(0.5f,2f,1f), ForceMode.Impulse);
    }
}
