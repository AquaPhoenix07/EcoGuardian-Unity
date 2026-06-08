namespace GAMEPLAY.Interactable
{
    using UnityEngine;
    public class PushableItem : MonoBehaviour
    {
        [Header("Item ID")]
        public ItemType myItemType; 
        
        [Tooltip("Kéo object viền của CHÍNH NÓ vào đây")]
        public GameObject myHighlight; 

        [Tooltip("Điền Tag của cái lỗ mà vật này cần chui vào. VD: Fire_Target")]
        public string targetTag; 

        // Biến này để nhớ chính xác cái đích nào đang được bật, để sau đó tắt cho đúng
        private GameObject currentTargetHighlight;

        public void SetHighlight(bool isOn)
        {
            // 1. Bật/tắt viền của chính cục này
            if (myHighlight != null) 
            {
                myHighlight.SetActive(isOn);
            }

            // 2. Xử lý bật/tắt Đích đến
            if (isOn)
            {
                // Khi BẬT: Đi tìm cái đích GẦN NHẤT
                if (!string.IsNullOrEmpty(targetTag))
                {
                    GameObject[] targets = GameObject.FindGameObjectsWithTag(targetTag);
                    float minDistance = Mathf.Infinity;
                    GameObject nearestTarget = null;

                    // Đo khoảng cách để lọc ra cái đích gần xô nước này nhất
                    foreach (GameObject t in targets)
                    {
                        float dist = Vector3.Distance(transform.position, t.transform.position);
                        if (dist < minDistance)
                        {
                            minDistance = dist;
                            nearestTarget = t;
                        }
                    }

                    // Chỉ bật viền của đúng cái đích gần nhất vừa tìm được
                    if (nearestTarget != null)
                    {
                        Transform targetHighlight = nearestTarget.transform.Find("Highlight 1");
                        if (targetHighlight != null) 
                        {
                            currentTargetHighlight = targetHighlight.gameObject;
                            currentTargetHighlight.SetActive(true);
                        }
                    }
                }
            }
            else
            {
                // Khi TẮT: Chỉ tắt đúng cái viền đích đã được bật lúc nãy
                if (currentTargetHighlight != null)
                {
                    currentTargetHighlight.SetActive(false);
                    currentTargetHighlight = null; // Reset bộ nhớ
                }
            }
        }
    }
}