using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.WSA;

public class MenuController : MonoBehaviour
{
    // Start is called before the first frame update
    private static MenuController instance = null;
    public Button LoadButton, ResumeButton, LeftArrow, RightArrow,BackToFolderView;
    public Slider SpeedSlider;
    public GameObject SavePanel;
    public GameObject ItemsWindow;
    public ReplayController ReplayComponent;
    public static bool IsReplayRun = false;
    public static float speed = 1;
    public List<GameObject> FileItems;
    public List<GameObject> FolderItems;
    public GameObject FolderIcon;
    public GameObject FileIcon;
    private string CurrentFolderPath;
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
            CreateCurrentFolder();
        }
    }
    private void CreateCurrentFolder()
    {
        string uniquename = Guid.NewGuid().ToString();
        string path = Path.Combine(UnityEngine.Application.persistentDataPath, uniquename);
        Directory.CreateDirectory(path);
        ReplUtils.AssignCurrentFolder(path);
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

    private void HideFolderIcons()
    {
        for (int i = 0; i < FolderItems.Count; i++)
        {
            FolderItems[i].SetActive(false);
        }
    }
    private void RemoveFileIcons()
    {
        for (int i = 0; i < FileItems.Count; i++)
        {
            Destroy(FileItems[i]);
        }
        FileItems.Clear();
    }
    private void PopulateFileList(string folderpath)
    {
        List<string> filenames = ReplUtils.GetSaveFileNames(folderpath);
        if (folderpath != CurrentFolderPath)
        {
            RemoveFileIcons();
            for (int i = 0; i < filenames.Count; i++)
            {
                GameObject file = Instantiate(FileIcon, ItemsWindow.transform);
                var textcomponent = file.GetComponentInChildren<TMP_Text>();
                textcomponent.text += filenames[i];
                file.GetComponentInChildren<Button>().onClick.AddListener(delegate {
                    IsReplayRun = true;
                    Time.timeScale = speed;
                    ReplayComponent.LoadReplay(Path.Combine(folderpath, textcomponent.text));
                });
                FileItems.Add(file);
            }
            CurrentFolderPath = folderpath;
        }
        else
        {
            ShowFileIcons();
        }
    }
    private void ShowFileIcons()
    {
        foreach (var fileicon in FileItems)
        {
            fileicon.SetActive(true);
        }
    }
    private void HideFileIcons()
    {
        foreach (var fileicon in FileItems)
        {
            fileicon.SetActive(false);
        }
    }

    private void PopulateFolderList(string[] paths)
    {
            for (int i = 0; i < paths.Length; i++)
            {
                GameObject folder = Instantiate(FolderIcon, ItemsWindow.transform);
                string formattedtext = Path.GetDirectoryName(paths[i]).Substring(0, 4) + "...";
                folder.GetComponentInChildren<TMP_Text>().text = formattedtext;
                var idx = i;
                folder.GetComponentInChildren<Button>().onClick.AddListener(delegate
                {
                    HideFolderIcons();
                    PopulateFileList(paths[idx]);
                    BackToFolderView.interactable = true;
                });
                FolderItems.Add(folder);
            }
    }
    private void ShowFolderIcons()
    {
        foreach (GameObject foldericon in FolderItems)
        {
            foldericon.SetActive(true);
        }
    }
    private void OpenLoadTab()
    {
        Time.timeScale = 0f;
        if (FolderItems.Count == 0)
        {
            string[] folderpaths = ReplUtils.GetFolderNames();
            PopulateFolderList(folderpaths);
        }
        else
        {
            ShowFolderIcons();
        }
        SavePanel.SetActive(true);
        ResumeButton.gameObject.SetActive(true);
        LoadButton.gameObject.SetActive(false);
    }
    private void Init()
    {
        BackToFolderView.interactable = false;
        BackToFolderView.onClick.AddListener(delegate
        {
                HideFileIcons();
                ShowFolderIcons();
                BackToFolderView.interactable = false;
        });
        LoadButton.onClick.AddListener(OpenLoadTab);
        ResumeButton.onClick.AddListener(GoBackToSim);
  
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
        LoadButton.gameObject.SetActive(true);

        //SpeedSlider.gameObject.SetActive(true);
    }
    private void TriggerGeneralMenu()
    {
        LoadButton.gameObject.SetActive(true);
    }
}
