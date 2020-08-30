using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject PlatformTemplate;
    //public int PopulationSize = 100;
    //public int HumansPerPlatform = 10;

    private void Awake()
    {
        /*int platformcount = PopulationSize / HumansPerPlatform;
        for(int i = 1; i < platformcount; i++)
        {
            Instantiate(PlatformTemplate, PlatformTemplate.transform.position + new Vector3(0, i*15, 0), Quaternion.identity);
        }*/
        Instantiate(PlatformTemplate, PlatformTemplate.transform.position + new Vector3(0, 0, 0), Quaternion.identity);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
