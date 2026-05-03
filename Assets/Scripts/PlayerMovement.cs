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

    private bool lockKey_WhenMove = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        animator.SetBool("IsIdle", true);
    }

    // Update is called once per frame
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
        
        //elapseTime:
        float elapsedTime = 0;
        float timeToMove = 0.3f;
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
            Obstacles.transform.position = endPos;
            return true;
        }
        return false;
    }
    
    
}
