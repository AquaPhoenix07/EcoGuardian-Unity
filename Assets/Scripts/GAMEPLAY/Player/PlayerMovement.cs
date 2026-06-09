using UnityEngine;
using System.Collections;
using GAMEPLAY.Interactable;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;
    private Animator animator;
    
    //Chỉ để khoá bàn phím chớ không có ben Animator
    private bool lockKey_WhenMove = false;
    
    [Header("Radar Settings")]
    public float searchRadius = 3f; // Bán kính quét xung quanh người
    public LayerMask pushableLayer; // Phân biệt layer của vật đẩy được
    private PushableItem currentNearestItem; // Nhớ vật gần nhất hiện tại
    
    // Mobile UI
    [Header("Swipe Controls (Mobile)")]
    public float minSwipeDistance = 50f; // Quãng đường vuốt tối thiểu để nhận lệnh (chống chạm nhầm)
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsIdle", true);
    }
    
    void Update()
    {
        if (lockKey_WhenMove) return;
        MovementSetting();
        SwipeDetection();  // THÊM DÒNG NÀY: Để nhận diện vuốt trên điện thoại
        CheckNearestHighlight();
    }

    private void MovementSetting()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) direction = Vector3.up;
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) direction = Vector3.left;
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) direction = Vector3.right;
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) direction = Vector3.down;
        
        if (direction != Vector3.zero)
        {
            ExecuteMove(direction); // Thay vì gọi thẳng SmoothMovement như trước
        }
    }

    private IEnumerator SmoothMovement(Vector3 direction)
    {
        lockKey_WhenMove = true;
        animator.SetBool("IsIdle", false);
        
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + direction;
        
        //elapseTime: giống cái đồng hồ đếm giờ
        float elapsedTime = 0;
        float timeToMove = 0.3f;
        
        /*
         Hàm Lerp (Linear Interpolation - Nội suy tuyến tính):
            + Hàm này dùng để tìm một điểm nằm giữa hai điểm khác dựa trên một tỷ lệ phần trăm.  
            Cú pháp: Vector3.Lerp(Điểm_A, Điểm_B, t)
                Đối số t: Là một con số từ 0 đến 1.
                Nếu $t = 0$: Kết quả là Điểm A.
                Nếu t = 1: Kết quả là Điểm B.
                Nếu t = 0.5: Kết quả là điểm chính giữa A và B. 
            --> Tạo ra chuyển động mượt mà hơn
         */
        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        animator.SetBool("IsIdle", true);
        lockKey_WhenMove = false;
    }

    private IEnumerator SmoothPush(GameObject obstacle, Vector3 direction)
    {
        lockKey_WhenMove = true;
        animator.SetTrigger("PushTrigger");
        
        Vector3 startPos = obstacle.transform.position;
        Vector3 endPos = startPos + direction;

        float elapsedTime = 0;
        float timeToMove = 0.3f;
        while (elapsedTime < timeToMove)
        {
            if (obstacle == null)
            {
                lockKey_WhenMove = false;
                yield break;
            }
            obstacle.transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obstacle.transform.position = endPos;
        animator.SetBool("IsIdle", true);
        lockKey_WhenMove = false;
    }
    
    
    private bool CanMoveTo(Vector3 direction)
    {
        //transform.position trả ra toạ độ  ở center
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + direction;
        
        //Vẽ tạm cái raycast
        // Debug.DrawLine(startPos, endPos, Color.red, 0.5f);
        RaycastHit2D hit = Physics2D.Linecast(startPos, endPos, obstacleLayer);
        if (hit.collider == null)
        {
            return true;
        }
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            return false;
        }

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
        {
            return false;
        }
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            return TryPush(hit.collider.gameObject, direction);
        }
        return false;
    }

    private bool TryPush(GameObject Obstacles, Vector3 direction)
    {
        // Lấy Collider ra trước
        BoxCollider2D col = Obstacles.GetComponent<BoxCollider2D>();
    
        // BẮT ĐẦU TIA TỪ TÂM CỦA CÁI KHUNG XANH LÁ (col.bounds.center)
        // TÂM NÀY CHẮC CHẮN NẰM GIỮA THÙNG NƯỚC, BẤT CHẤP PIVOT NẰM Ở ĐÂU
        Vector3 rayStartPos = col.bounds.center; 
        Vector3 rayEndPos = rayStartPos + direction;
    
        // Tắt collider để tia không chạm vào chính nó
        col.enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(rayStartPos, rayEndPos, obstacleLayer);
        col.enabled = true;
    
        // Debug để thấy tia đỏ bắn từ giữa ngực thùng nước ra
        Debug.DrawLine(rayStartPos, rayEndPos, Color.red, 2f);
        
        if (hit.collider != null)
        {
            Debug.Log($"Thùng nước đang bị cản bởi: {hit.collider.gameObject.name} | Thuộc Layer: {LayerMask.LayerToName(hit.collider.gameObject.layer)}");
        }
        // --------------------------------------
        
        if (hit.collider == null )
        {
            StartCoroutine(SmoothPush(Obstacles, direction));
            return true;
        }

        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
        {
            StartCoroutine(SmoothPush(Obstacles, direction));
            return true;
        }
        return false;
    }
    
    private void CheckNearestHighlight()
    {
        // Quét 1 vòng tròn quanh Player để lấy tất cả các vật phẩm nằm trên layer được chỉ định
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, searchRadius, pushableLayer);

        PushableItem nearestItem = null;
        float minDistance = Mathf.Infinity;

        // Lọc ra thằng nào đứng GẦN NHẤT
        foreach (Collider2D hit in hits)
        {
            PushableItem item = hit.GetComponent<PushableItem>();
            if (item != null)
            {
                float dist = Vector3.Distance(transform.position, hit.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearestItem = item;
                }
            }
        }

        // NẾU VẬT GẦN NHẤT CÓ SỰ THAY ĐỔI
        if (nearestItem != currentNearestItem)
        {
            // Tắt Highlight của vật CŨ và Đích cũ đi
            if (currentNearestItem != null)
            {
                currentNearestItem.SetHighlight(false);
            }

            // Ghi nhớ vật MỚI
            currentNearestItem = nearestItem;

            // Bật Highlight của vật MỚI và Đích mới lên
            if (currentNearestItem != null)
            {
                currentNearestItem.SetHighlight(true);
            }
        }
    }

    // Vẽ cái vòng tròn vàng trong Editor để bạn dễ căn chỉnh bán kính quét
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
    
    // ==========================================
    // HỆ THỐNG ĐIỀU KHIỂN VUỐT (SWIPE) CHO ĐIỆN THOẠI
    // ==========================================
    private void SwipeDetection()
    {
        // Kiểm tra xem có ngón tay nào đang chạm vào màn hình không
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Ngón tay BẮT ĐẦU chạm vào màn hình
            if (touch.phase == TouchPhase.Began)
            {
                startTouchPosition = touch.position;
                endTouchPosition = touch.position; 
            }
            // Ngón tay NHẤC LÊN khỏi màn hình
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                endTouchPosition = touch.position;
                DetectSwipeDirection();
            }
        }
    }

    private void DetectSwipeDirection()
    {
        // Tính toán khoảng cách và hướng giữa điểm đặt tay và điểm nhấc tay
        Vector2 swipeDelta = endTouchPosition - startTouchPosition;

        // Nếu vuốt quá ngắn (như kiểu lỡ chạm nhẹ) thì bỏ qua
        if (swipeDelta.magnitude >= minSwipeDistance)
        {
            Vector3 direction = Vector3.zero;

            // So sánh độ dài trục X và trục Y để biết là vuốt NGANG hay vuốt DỌC
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                // Vuốt theo trục NGANG
                if (swipeDelta.x > 0) direction = Vector3.right; // Vuốt sang phải
                else direction = Vector3.left;                   // Vuốt sang trái
            }
            else
            {
                // Vuốt theo trục DỌC
                if (swipeDelta.y > 0) direction = Vector3.up;    // Vuốt lên trên
                else direction = Vector3.down;                   // Vuốt xuống dưới
            }

            // Thực thi lệnh di chuyển
            if (direction != Vector3.zero)
            {
                ExecuteMove(direction);
            }
        }
    }

    // Gom chung lệnh thực thi (Cả PC và Điện thoại đều xài chung hàm này)
    private void ExecuteMove(Vector3 direction)
    {
        if (lockKey_WhenMove) return;

        animator.SetFloat("MoveX", direction.x);
        animator.SetFloat("MoveY", direction.y);
        
        if (CanMoveTo(direction))
        {
            StartCoroutine(SmoothMovement(direction));
        }
    }
}
