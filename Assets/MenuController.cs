using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    private static MenuController instance = null;
    public Button LoadButton, ResumeButton, LeftArrow, RightArrow;
    public Slider SpeedSlider;
    public GameObject SavePanel;
    public List<Button> SaveButtonList = new List<Button>();
    public ReplayController ReplayComponent;
    public static bool IsReplayRun = false;
    public static float speed = 1;
    private void Awake()
    {
  
      
        
    }
    void Start()
    {
        Init();

        ResetMenus();
        if (IsReplayRun)
        {
            TriggerReplayMenu();
        }
        else
        {
            TriggerGeneralMenu();
        }
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
        Time.timeScale = speed;
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
    private void Init()
    {
        LoadButton.onClick.AddListener(OpenLoadTab);
        ResumeButton.onClick.AddListener(GoBackToSim);
        for (int i = 0; i < SaveButtonList.Count; i++)
        {
            SaveButtonList[i].gameObject.SetActive(false);
            var idx = i;
            SaveButtonList[i].onClick.AddListener(delegate {
                IsReplayRun = true;
                Time.timeScale = speed;
                ReplayComponent.LoadReplay(SaveButtonList[idx].GetComponentInChildren<TMP_Text>().text);
            });
        }

        if (ReplayComponent.GetIndex() == ReplayComponent.PopulationComponent.PopulationSize - 1)
        {
            RightArrow.interactable = false;
        }
        if (ReplayComponent.GetIndex() == 0)
        {
            LeftArrow.interactable = false;
        }
        LeftArrow.onClick.AddListener(delegate
        {
            ReplayComponent.ChangeCandidate(-1);
  
        });

        RightArrow.onClick.AddListener(delegate
        {
            ReplayComponent.ChangeCandidate(1);

        });
        SpeedSlider.value = speed;
        SpeedSlider.onValueChanged.AddListener(delegate {
            speed = SpeedSlider.value;
            Time.timeScale = speed; });


    }
    private void ResetMenus()
    {
        SavePanel.SetActive(false);
        ResumeButton.gameObject.SetActive(false);
        LoadButton.gameObject.SetActive(false);

        LeftArrow.gameObject.SetActive(false);
        RightArrow.gameObject.SetActive(false);
        //SpeedSlider.gameObject.SetActive(false);
    }
    private void TriggerReplayMenu()
    {
        LeftArrow.gameObject.SetActive(true);
        RightArrow.gameObject.SetActive(true);
        //SpeedSlider.gameObject.SetActive(true);
    }
    private void TriggerGeneralMenu()
    {
        LoadButton.gameObject.SetActive(true);
    }
}
