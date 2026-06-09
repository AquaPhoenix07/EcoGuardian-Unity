using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices; 

namespace CORE.Game_Manager
{
    [System.Serializable]
    public class PlayerData
    {
        public int highestUnlockedLevel = 1; 
        public List<string> unlockedSpecies = new List<string>(); 
        
        // === [THÊM MỚI] Biến để lưu trạng thái huy hiệu bảo vệ môi trường ===
        // Mặc định ban đầu vào game sẽ là false (chưa có)
        public bool hasGuardianBadge = false;
        // ===================================================================
    }

    public class SaveManager : MonoBehaviour
    {
        private static SaveManager _instance;
        public static SaveManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Object.FindAnyObjectByType<SaveManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("Save_Manager");
                        _instance = go.AddComponent<SaveManager>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        public void Start()
        {
            RequestProgressFromWeb();
        }
        

        public PlayerData currentData = new PlayerData(); 

        [DllImport("__Internal")]
        private static extern void SyncDataToWeb(string jsonDataString);

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        public void UnlockNextLevel(int nextLevelIndex)
        {
            if (nextLevelIndex > currentData.highestUnlockedLevel)
            {
                currentData.highestUnlockedLevel = nextLevelIndex;
                Debug.Log("Bộ nhớ đã ghi nhận: Mở khóa tới Level " + nextLevelIndex);
            }
        }

        // === [SỬA ĐỔI] Nâng cấp hàm thêm một biến 'bool checkFinalLevel' ở cuối ===
        public void SaveProgressToWeb(int newLevel, List<string> newAnimals, bool checkFinalLevel)
        {
            UnlockNextLevel(newLevel);

            if (newAnimals != null && newAnimals.Count > 0)
            {
                foreach (string animal in newAnimals)
                {
                    if (!string.IsNullOrEmpty(animal) && !currentData.unlockedSpecies.Contains(animal))
                    {
                        currentData.unlockedSpecies.Add(animal);
                    }
                }
            }

            // === [THÊM MỚI LOGIC] Nếu Trọng tài báo đây là màn cuối -> Kích hoạt huy hiệu ===
            if (checkFinalLevel == true)
            {
                currentData.hasGuardianBadge = true;
                Debug.Log("Chúc mừng! Bạn đã nhận được Huy hiệu Bảo vệ Môi trường!");
            }
            // ==============================================================================

            string jsonString = JsonUtility.ToJson(currentData);

            #if UNITY_WEBGL && !UNITY_EDITOR
                SyncDataToWeb(jsonString); 
            #else
                Debug.Log("Chạy trên Editor - Cục JSON sẽ gửi đi: " + jsonString); 
            #endif
        }
        // Trong SaveManager.cs
        public void RequestProgressFromWeb() {
                #if UNITY_WEBGL && !UNITY_EDITOR
                    Application.ExternalCall("RequestDataFromWeb"); 
                    #endif
        }

        // Hàm này sẽ được JS gọi lại khi nhận được data từ DB
                public void OnReceiveDataFromWeb(string jsonString) {
                    currentData = JsonUtility.FromJson<PlayerData>(jsonString);
                    Debug.Log("Game đã nhận data cũ: " + jsonString);
                    // Cập nhật lại UI các cánh cửa ở đây
                }
    }
}