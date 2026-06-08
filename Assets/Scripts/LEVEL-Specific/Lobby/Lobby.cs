using System;
using CORE.Game_Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Lobby : MonoBehaviour
{
    private SoundManager soundManager;
    [Header("UI References")]
    public Toggle musicToggle;
    public void Start()
    {
        soundManager = SoundManager.Instance;
        if (musicToggle != null)
        {
            bool currentMusicState = soundManager.isBackgroundMusicOn;
            
            // Dùng dấu ! vì logic là nút Mute (Tắt tiếng)
            musicToggle.SetIsOnWithoutNotify(!currentMusicState);
        }
    }

    public void Update()
    {
        
    }

    public void PlayButtonClicked()
    {
        soundManager.StartCoroutine("PlayButtonClicked");
        SceneManager.LoadScene("Level_1");
    }

    public void BackgroundMusicToggle(bool isOn)
    {
        soundManager.BackgroundMusicToggle(isOn);
    }
}