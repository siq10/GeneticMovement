using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReplayController : MonoBehaviour
{
    public CameraFollow SpecialCam;
    private static List<float> ScoreList;
    private static int charindex = 0;
    public PopulationControlller PopulationComponent;

    // Start is called before the first frame update
    void Awake()
    {
        if(MenuController.IsReplayRun == false)
        {
            gameObject.SetActive(false);
        }
    }
    private void Start()
    {
        if(MenuController.IsReplayRun == true)
        {
            SpecialCam.enabled = true;
            SpecialCam.target = PopulationComponent.GetPopulation()[charindex].GetSpine();
            PopulationComponent.StartMovement(charindex);
        }

    }
    public int GetIndex()
    {
        return charindex;
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void ChangeCandidate(int direction)
    {
        charindex += direction;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        /*SetupCharacter();
        ResetCharacter();
        character.SetActive(true);
        individual.Act();*/
    }
    public void LoadReplay(string name)
    {
        List<List<List<Vector3>>> data;
        ReplUtils.LoadSimmulation(name, out data, out ScoreList);
        PopulationComponent.SetDNA(data);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}