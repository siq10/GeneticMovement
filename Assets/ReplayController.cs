using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayController : MonoBehaviour
{
    private static List<Tuple<List<List<Vector3>>, List<List<float>>, List<List<Vector3>>>> ReplayDNA;
    private static List<float> ScoreList;
    // Start is called before the first frame update
    void Awake()
    {
        if(MenuController.IsReplayRun == false)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadReplay(string name)
    {
        ReplUtils.LoadSimmulation(name, out ReplayDNA, out ScoreList);
        Debug.Log(ReplayDNA.Count);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}