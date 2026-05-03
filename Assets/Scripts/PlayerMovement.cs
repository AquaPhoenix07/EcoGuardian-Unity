using UnityEngine;
using System.Collections;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;
    private Animator animator;
    private AudioSource playerAudio;

    public AudioClip finishSound;
    public AudioClip footStepSound;
    
    //Chỉ để khoá bàn phím chớ không có ben Animator
    private bool lockKey_WhenMove = false;
    
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        animator.SetBool("IsIdle", true);
    }
    
    void Update()
    {
        if (lockKey_WhenMove) return;
        MovementSetting();
    }

    private void MovementSetting()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
                direction = Vector3.up;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
                direction = Vector3.left;
        }

        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
                direction = Vector3.right;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
                direction = Vector3.down;
        }
        
        if (direction != Vector3.zero)
        {
            animator.SetFloat("MoveX", direction.x);
            animator.SetFloat("MoveY", direction.y);
            if (CanMoveTo(direction))
            {
                // Làm mượt movement
                StartCoroutine(SmoothMovement(direction));
            }
        }
    }

    private IEnumerator SmoothMovement(Vector3 direction)
    {
        lockKey_WhenMove = true;
        animator.SetBool("IsIdle", false);
        playerAudio.PlayOneShot(footStepSound,1.0f);

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
        Debug.DrawLine(startPos, endPos, Color.red, 0.5f);
        RaycastHit2D hit = Physics2D.Linecast(startPos, endPos, obstacleLayer);
        if (hit.collider == null)
        {
            return true;
        }
        if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Wall"))
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
        Vector3 startPos = Obstacles.transform.position;
        Vector3 endPos = startPos + direction;
        
        Debug.DrawLine(startPos, endPos, Color.red, 0.5f);
        Obstacles.GetComponent<BoxCollider2D>().enabled = false;
        RaycastHit2D hit = Physics2D.Linecast(startPos, endPos, obstacleLayer);
        Obstacles.GetComponent<BoxCollider2D>().enabled = true;
        
        if (hit.collider == null )
        {
            StartCoroutine(SmoothPush(Obstacles, direction));
            return true;
        }
        return false;
    }
    
    
}
