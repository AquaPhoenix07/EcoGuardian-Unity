using CORE.Game_Manager;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [Tooltip("Gõ số của màn chơi này vào đây. VD: Đang ở màn 1 thì gõ 1")]
    public int currentLevelIndex; 

    // Các hàm Start, Update, OpenSetting... của bạn giữ nguyên bên dưới
    // ...

    // Hàm WinLevel dùng chung cho vạn vật
    public void WinLevel()
    {
        Debug.Log("Đã thắng level " + currentLevelIndex);

        // Chìa khóa vạn năng: Mở khóa màn tiếp theo = Màn hiện tại + 1
        int nextLevel = currentLevelIndex + 1;
        SaveManager.Instance.UnlockNextLevel(nextLevel); 
    }
}