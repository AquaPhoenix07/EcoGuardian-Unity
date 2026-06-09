using CORE.Game_Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager1 : MonoBehaviour
{
    [Header("Canvas Settings")]
    public GameObject SettingMenu;
    public GameObject MissionMenu;
    
    [Header("Sound Settings")] 
    private bool currentMusicState;
    
    [Header("UI References")]
    public Toggle musicToggle;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentMusicState = SoundManager.Instance.isBackgroundMusicOn;
        if (musicToggle != null)
        {
            musicToggle.SetIsOnWithoutNotify(!currentMusicState);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartLevel();
        }
    }
    // RESTART BUTTON
    public void RestartLevel()
    {
        SoundManager.Instance.RestartButtonClicked();
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    
    //SETTING BUTTON 
    public void OpenSetting_Button()
    {
        SoundManager.Instance.SettingButtonClicked();
        SettingMenu.SetActive(true);
    }

    public void CloseSetting_Button()
    {
        SoundManager.Instance.SettingButtonClicked();
        SettingMenu.SetActive(false);
    }
    public void Exit_Button()
    {
        SoundManager.Instance.SettingButtonClicked();
        SceneManager.LoadScene(0);
    }
    
    //MISSION BUTTON
    public void OpenMission()
    {
        SoundManager.Instance.MissionButtonClicked();
        MissionMenu.SetActive(true);
    }
    public void CloseMission()
    {
        SoundManager.Instance.MissionButtonClicked();
        MissionMenu.SetActive(false);
    }
    //Sound Button
    public void SoundToggle(bool isOn)
    {
        SoundManager.Instance.BackgroundMusicToggle(isOn);
    }
}
