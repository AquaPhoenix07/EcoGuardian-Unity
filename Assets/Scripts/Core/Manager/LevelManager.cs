using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LevelManager : MonoBehaviour
{
    public Canvas canvas;
    public GameObject SettingMenu;
    public GameObject MissionMenu;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }

    public void OpenSetting_Button()
    {
        SettingMenu.SetActive(true);
    }

    public void CloseSetting_Button()
    {
        SettingMenu.SetActive(false);
    }

    public void OnToggleSound(bool isOn)
    {
        if (isOn)
        {
            AudioListener.pause = true;
        }

        if (!isOn)
        {
            AudioListener.pause = false;
        }
    }

    public void OpenMission()
    {
        MissionMenu.SetActive(true);
    }

    public void CloseMission()
    {
        MissionMenu.SetActive(false);
    }


}
