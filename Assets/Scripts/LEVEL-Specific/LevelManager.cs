using System;
using CORE.Game_Manager;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic; 

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [Tooltip("Gõ số của màn chơi này vào đây. VD: Đang ở màn 1 thì gõ 1")]
    public int currentLevelIndex; 
    
    [Tooltip("Danh sách các động vật cứu được ở màn này. VD: Armadillo, RedPanda")]
    public List<string> unlockedAnimalsInThisLevel;
    // === [THÊM MỚI] Biến tích chọn ngoài Unity để xác định đây có phải màn cuối không ===
    [Tooltip("Tích chọn ô này NẾU ĐÂY LÀ MÀN CHƠI CUỐI CÙNG của game")]
    public bool isFinalLevel = false;
    
    public GameObject WinMenu;
    
    //Animal Number
    private int totalAnimals = 0;
    private int safeAnimalsCount = 0;

    public void Start()
    {
        totalAnimals = FindObjectsByType<AnimalAI>(FindObjectsSortMode.None).Length;
    }

    public void WinLevel()
    {
        Debug.Log("Đã thắng level " + currentLevelIndex);
        
        // Vẫn giữ nguyên logic bật Menu chiến thắng của bạn
        WinMenu.SetActive(true); 
        
        int nextLevel = currentLevelIndex + 1;
        
        // === [SỬA ĐỔI] Truyền thêm biến isFinalLevel vào tham số thứ 3 của hàm ===
        SaveManager.Instance.SaveProgressToWeb(nextLevel, unlockedAnimalsInThisLevel, isFinalLevel); 
        // ======================================================================
    }
    
    public void Home()
    {
        SceneManager.LoadScene(0);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(currentLevelIndex + 1);
    }
    
    public void AnimalReachedShelter()
    {
        safeAnimalsCount++; 
        Debug.Log($"Tiến độ: {safeAnimalsCount}/{totalAnimals} con vật đã về chuồng!");

        if (safeAnimalsCount >= totalAnimals)
        {
            WinLevel(); 
            SoundManager.Instance.VictorySound();
        }
    }
}