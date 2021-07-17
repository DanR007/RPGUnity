using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    [SerializeField] private GameObject loadRoom;
    [SerializeField] private Button save, exit, load, resume;
    private Load _load;

    private void Start()
    {
        GetReference();


        pauseMenu.SetActive(false);
        loadRoom.SetActive(false);

        AddButtonEvents();
    }

    private void GetReference()
    {
        _load = GetComponent<Load>();
    }

    private void AddButtonEvents()
    {
        exit.onClick.AddListener(() => ExitToMenu());
        save.onClick.AddListener(() => Save());
        load.onClick.AddListener(delegate
        {
            _load._curY = 0;
            _load.UpdateSavingData();
            loadRoom.SetActive(true);
            pauseMenu.SetActive(false);
        });


        resume.onClick.AddListener(delegate
        {
            Background.BackgroundActive = false;
            pauseMenu.SetActive(false);
        });
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Background.BackgroundActive == false)
            {
                pauseMenu.SetActive(true);
                Background.BackgroundActive = true;
            }

            else
            {
                pauseMenu.SetActive(false);
                Background.BackgroundActive = false;
            }
        }

    }

    public bool GetActive
    {
        get
        {
            if (pauseMenu.activeSelf || loadRoom.activeSelf)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
    private void Save()
    {
        GetComponent<Save>().CreateSaveData();
    }
    private void ExitToMenu()
    {
        NameFileSave.FullFileName = string.Empty;
        SceneManager.LoadScene(0);
    }
}
