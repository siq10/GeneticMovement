using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatchCollision : MonoBehaviour
{
    public bool grounded = false;
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("p"))
        {
            Debug.Log(name + " collided with terrain");
            grounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("p"))
        {
            Debug.Log(name + " no longer in contact with terrain");
            grounded = false;
        }

    }
}
