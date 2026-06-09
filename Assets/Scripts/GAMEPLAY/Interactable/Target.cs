using CORE.Game_Manager;
using GAMEPLAY.Interactable;
using UnityEngine;

public class Target : MonoBehaviour
{
    [Header("Accepted Item ID")]
    public ItemType acceptedType; 
    
    public GameObject completedObject; // Vật sinh ra sau khi lấp (VD: Cỏ, Cây)
    private GameManager gameManager;
    private bool isCompleted = false;

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Nếu đã hoàn thành rồi thì bỏ qua, không xét nữa
        if (isCompleted) return;

        Debug.Log($"Có vật thể chạm vào Target: {collision.gameObject.name}");

        // 1. Kiểm tra xem vật chạm vào có script PushableItem không
        PushableItem pushedItem = collision.GetComponent<PushableItem>();
        
        if (pushedItem != null)
        {
            // 2. Đối chiếu mã vạch (Enum)
            if (pushedItem.myItemType == acceptedType)
            {
                isCompleted = true; 
                
                // Xóa vật thể đẩy (Ví dụ: Khúc gỗ)
                Destroy(pushedItem.gameObject); 
                
                // Sinh ra vật thể mới (Ví dụ: Cầu gỗ bắt ngang)
                if (completedObject != null)
                {
                    Instantiate(completedObject, transform.position, completedObject.transform.rotation);
                }
                
                // Cộng điểm
                gameManager.UpdateScore(1);
                Destroy(gameObject); 
            }
            else
            {
                Debug.Log($"Sai vật thể! Cần {acceptedType} nhưng lại đẩy {pushedItem.myItemType} vào.");
            }
        }
        else
        {
            Debug.Log($"Vật thể {collision.gameObject.name} không có script PushableItem!");
        }
    }
}