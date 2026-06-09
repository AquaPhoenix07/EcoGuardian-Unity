using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CORE.Game_Manager;

public class LevelButtonUI : MonoBehaviour
{
    [Header("Level Settings")]
    public int levelIndex; // Nút này đại diện cho Level mấy? (VD: 1, 2, 3...)
    public string levelSceneName; // Tên Scene sẽ chuyển sang khi bấm (VD: "Level_1")

    [Header("Visuals")]
    public Sprite unlockedDoorSprite; // Cánh cửa GỖ
    public Sprite lockedDoorSprite;   // Cánh cửa XÁM (Có ổ khóa)

    private Button myButton;
    private Image myImage;

    private void Start()
    {
        myButton = GetComponent<Button>();
        myImage = GetComponent<Image>();

        // Cập nhật giao diện ngay khi bật Menu lên
        UpdateLevelStatus();
    }

    public void UpdateLevelStatus()
    {
        // Hỏi Bộ Não xem Acc này đang chơi tới level mấy rồi
        int unlockedLevel = SaveManager.Instance.currentData.highestUnlockedLevel;

        if (levelIndex <= unlockedLevel)
        {
            // NẾU ĐÃ MỞ KHÓA: Hiện cửa Gỗ, và cho phép bấm
            myImage.sprite = unlockedDoorSprite;
            myButton.interactable = true;
        }
        else
        {
            // NẾU BỊ KHÓA: Hiện cửa Xám, và cấm bấm
            myImage.sprite = lockedDoorSprite;
            myButton.interactable = false;
        }
    }

    // Gắn hàm này vào sự kiện OnClick() của cái nút
    public void OnLevelButtonClicked()
    {
        SoundManager.Instance.StartCoroutine("PlayButtonClicked");
        SceneManager.LoadScene(levelSceneName);
    }
}