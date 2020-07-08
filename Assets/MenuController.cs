using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public Button LoadButton, ResumeButton;
    public GameObject SavePanel;
    public List<Button> SaveButtonList = new List<Button>();
    public ReplayController ReplayComponent;
    public static bool IsReplayRun = false;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (GameObject.Find(gameObject.name)
                  && GameObject.Find(gameObject.name) != this.gameObject)
        {
            Destroy(GameObject.Find(gameObject.name));
        }
    }
    void Start()
    {
        LoadButton.onClick.AddListener(OpenLoadTab);
        ResumeButton.onClick.AddListener(GoBackToSim);
        for(int i =0; i< SaveButtonList.Count; i++)
        {
            SaveButtonList[i].gameObject.SetActive(false);
            var idx = i;
            SaveButtonList[i].onClick.AddListener(delegate {
                IsReplayRun = true;
                ReplayComponent.LoadReplay(SaveButtonList[idx].GetComponentInChildren<TMP_Text>().text); 
            });
        }
        SavePanel.SetActive(false);
        ResumeButton.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void GoBackToSim()
    {
        SavePanel.SetActive(false);
        ResumeButton.gameObject.SetActive(false);
        LoadButton.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }

    private void OpenLoadTab()
    {
        Time.timeScale = 0f;
        List<string> filenames = ReplUtils.GetSaveFiles();
        for (int i = 0; i < filenames.Count; i++)
        {
            SaveButtonList[i].gameObject.SetActive(true);
            SaveButtonList[i].GetComponentInChildren<TMP_Text>().text = filenames[i];
        }
        SavePanel.SetActive(true);
        ResumeButton.gameObject.SetActive(true);
        LoadButton.gameObject.SetActive(false);
    }
}
