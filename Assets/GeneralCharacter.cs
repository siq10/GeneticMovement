using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneralCharacter : MonoBehaviour
{
    //17 movable rigidbodies
    private List<Vector3> HorizontalMovementDNA = new List<Vector3>();
    private List<Vector3> torqueDNA = new List<Vector3>();
    private List<float> VerticalMovementDNA = new List<float>();

    private int ChromosomeLength = 50;
    // horizontal,vertical,torque
    // vector3 * DNAlength
    [SerializeField]
    public List<string> TerrainList = new List<string>();

    private Rigidbody headrb;
    Transform[] allchildren; 
    float distToGround;
    List<GameObject> limbs = new List<GameObject>();

    public void SetDNA(List<Vector3> horizontalMovement, List<float> verticalMovement, List<Vector3> torque)
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

        //RandomMovement();
        //Debug.Log(GetHeadYcoordinate());

    }
    void RandomMovement()
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


    public List<Collider> GetAllColliders()
    {
        List<Collider> result = new List<Collider>();
        var allchildren = transform.GetComponentsInChildren<Transform>(true);
        for (int j = 0; j < allchildren.Length; j++)
        {
            if (GetComponent<Collider>() != null)
            {
                result.Add(allchildren[j].GetComponent<Collider>());
            }
        }
        return result;
    }

}
