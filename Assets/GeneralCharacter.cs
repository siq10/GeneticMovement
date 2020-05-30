using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralCharacter : MonoBehaviour
{
    private Rigidbody headrb;
    Transform[] allchildren;
    float distToGround;
    List<GameObject> limbs = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        allchildren = transform.GetComponentsInChildren<Transform>(true);
        for (int i = 0; i < allchildren.Length; i++)
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

        RandomMovement();
        //Debug.Log(GetHeadYcoordinate());

    }
    void RandomMovement()
    {
        for (int i = 1; i < allchildren.Length; i++)
        {
            //Debug.Log(allchildren[i].name);

            var rb = allchildren[i].GetComponent<Rigidbody>();

            if (rb)
            {
                //Debug.Log(allchildren[i].name);
               
                HorizontalMovement(rb);
                //break;
                VerticalMovement(rb);
                /*transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);
                float speed = 600;
                rigidBody.isKinematic = false;
                Vector3 force = transform.forward;
                force = new Vector3(force.x, 1, force.z);
                rigidBody.AddForce(force * speed);
                rb.AddForce();*/
            }

            //child is your child transform
        }
    }

    bool IsGrounded()
    {
        bool result = false;
        for (var i = 0; i < limbs.Count; i++)
        {
            if(Physics.Raycast(limbs[i].transform.position, -Vector3.up, limbs[i].GetComponent<Collider>().bounds.extents.y + 0.1f))
            result = true ;
            Debug.Log(limbs[i].name + " is on the ground!");
            break;
        }
        return result;

    }
    void HorizontalMovement(Rigidbody rb)
    {
        Vector3 torque;
        torque.x = Random.Range(-10, 10);
        torque.y = Random.Range(-10, 10);
        torque.z = Random.Range(-10, 10);
        rb.AddTorque(torque);
        rb.AddForce(Vector3.Scale(new Vector3(1, 1, 1), new Vector3(Random.Range(-1000f, 1000f), Random.Range(-100f, 100f), Random.Range(-1000f, 1000f))), ForceMode.Force);
    }
    void VerticalMovement(Rigidbody rb)
    {
        if(IsGrounded())
        {
            rb.AddForce(new Vector3(0, 1, 0) * Random.Range(-1000f, 1000f));
        }
    }
    float GetHeadYcoordinate()
    {
        return headrb.position.y;
    }

}
