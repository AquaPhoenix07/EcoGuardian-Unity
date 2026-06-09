using UnityEngine;

namespace CORE.Game_Manager
{
    // Cái khuôn chứa dữ liệu
    [System.Serializable]
    public class PlayerData
    {
        public int highestUnlockedLevel = 1; // Mặc định vào game là cho mở Level 1
    }

    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance { get; private set; }
        
        // Khởi tạo sẵn một bộ dữ liệu mặc định
        public PlayerData currentData = new PlayerData(); 

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // --- BỔ SUNG HÀM MỞ KHÓA LEVEL VÀO ĐÂY ---
        public void UnlockNextLevel(int nextLevelIndex)
        {
            // Kiểm tra xem level định mở có cao hơn level hiện tại không
            if (nextLevelIndex > currentData.highestUnlockedLevel)
            {
                currentData.highestUnlockedLevel = nextLevelIndex;
                Debug.Log("Bộ nhớ đã ghi nhận: Mở khóa tới Level " + nextLevelIndex);
            }
        }
    }
}