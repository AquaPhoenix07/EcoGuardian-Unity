using System;
using CORE.Game_Manager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [Tooltip("Gõ số của màn chơi này vào đây. VD: Đang ở màn 1 thì gõ 1")]
    public int currentLevelIndex; 
    public GameObject WinMenu;
    
    //Animal Number
    private int totalAnimals = 0;
    private int safeAnimalsCount = 0;

    public void Start()
    {
        totalAnimals = FindObjectsByType<AnimalAI>(FindObjectsSortMode.None).Length;
    }

    // Hàm WinLevel dùng chung cho vạn vật
    public void WinLevel()
    {
        Debug.Log("Đã thắng level " + currentLevelIndex);
        WinMenu.SetActive(true);
        // Chìa khóa vạn năng: Mở khóa màn tiếp theo = Màn hiện tại + 1
        int nextLevel = currentLevelIndex + 1;
        SaveManager.Instance.UnlockNextLevel(nextLevel); 
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
        safeAnimalsCount++; // Cộng 1 vào danh sách an toàn
        Debug.Log($"Tiến độ: {safeAnimalsCount}/{totalAnimals} con vật đã về chuồng!");

        // 3. Kiểm tra: Đã đủ quân số chưa?
        if (safeAnimalsCount >= totalAnimals)
        {
            WinLevel(); // Đủ rồi thì mới thổi còi kết thúc game!
        }
    }
}