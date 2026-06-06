using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AnimalAI : MonoBehaviour
{
    //Shelter
    [Header("Shelter")]
    public string targetShelterTag; 
    private GameObject targetShelter;

    private float timeToMove = 0.3f;
    private bool isMoving;
    
    //Other Class 
    private AStarManager aStarManager;
    private GameManager gameManager;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    
    private List<Vector3> resultPath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aStarManager = FindObjectOfType<AStarManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        
        // Bắt đầu chu kỳ suy nghĩ và di chuyển
        StartCoroutine(ThinkAndMoveRoutine());
        
        //Random Emotion
        InvokeRepeating("RandomEmotion", 3.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator ThinkAndMoveRoutine()
    {
        isMoving = false;
        yield return new WaitUntil(() => 
            gameManager.Target == gameManager.currentTarget && 
            gameManager.isMapReady);
        isMoving = true;
        targetShelter = GameObject.FindWithTag(targetShelterTag);
        if (targetShelter == null) {
            Debug.LogError("Không tìm thấy Shelter với Tag: " + targetShelterTag);
            yield break;
        }
        while (true)
            {
                if (targetShelter != null && Vector3.Distance(transform.position, targetShelter.transform.position) < 0.1f)
                {
                    Debug.Log(gameObject.name + " đã về đích an toàn!");
                    animator.SetBool("IsMoving", false);
                    isMoving = false;
                    yield break; 
                }
                // Đường về
                resultPath = aStarManager.FindPath(transform.position, targetShelter.transform.position);
                // Kiểm tra danh sách đường đi (phải > 1 vì index [0] là vị trí hiện tại)
                if (resultPath != null && resultPath.Count > 1)
                {
                    Vector3 nextPos = resultPath[1];
                    yield return StartCoroutine(SmoothMovement(nextPos));
                }
                yield return new WaitForSeconds(0.25f);
            }
    }
    
    private IEnumerator SmoothMovement(Vector3 endPos)
    {
        animator.SetBool("IsMoving", true);
        Vector3 startPos = transform.position;
        //Xoay mặt cho Animal
        if (endPos.x < startPos.x)
        {
            spriteRenderer.flipX = true;
        }
        else if (endPos.x > startPos.x)
        {
            spriteRenderer.flipX = false;
        }
        float elapseTime = 0;
        float timeToMove = 0.3f;
        
        while (elapseTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapseTime / timeToMove);
            elapseTime += Time.deltaTime;
            yield return null;
        }
        transform.position = endPos;
    }
    
    //Random Emotion
    private void RandomEmotion()
    {
        
        if (!isMoving)
        {
            if (gameObject.CompareTag("Armadillo"))
            {
                int randomIndex = Random.Range(0, 2);
                switch (randomIndex)
                {
                    case 0:
                    {
                        animator.SetTrigger("Spin_Trigger");
                        break;
                    }
                    case 1:
                    {
                        animator.SetTrigger("Sleep_Trigger");
                        break;
                    }
                }
            }

            if (gameObject.CompareTag("RedPanda"))
            {
                int randomIndex = Random.Range(0, 4);
                switch (randomIndex)
                {
                    case 0:
                    {
                        animator.SetTrigger("StepUp_Trigger");
                        break;
                    }
                    case 1:
                    {
                        animator.SetTrigger("Lay_Trigger");
                        break;
                    }
                    case 2:
                    {
                        animator.SetTrigger("CoverFace_Trigger");
                        break;
                    }
                    case 3:
                    {
                        animator.SetTrigger("Sleep_Trigger");
                        break;
                    }
                }
            }

            if (gameObject.CompareTag("Ferret"))
            {
                int randomIndex = Random.Range(0, 3);
                switch (randomIndex)
                {
                    case 0:
                    {
                        animator.SetTrigger("Sleep");
                        break;
                    }
                    case 1:
                    {
                        animator.SetTrigger("Idle");
                        break;
                    }
                    case 2:
                    {
                        animator.SetTrigger("Dig");
                        break;
                    }
                }
            }

            if (gameObject.CompareTag("Fox"))
            {
                int randomIndex = Random.Range(0, 3);
                switch (randomIndex)
                {
                    case 0:
                    {
                        animator.SetTrigger("Sleep");
                        break;
                    }
                    case 1:
                    {
                        animator.SetTrigger("Idle");
                        break;
                    }
                    case 2:
                    {
                        animator.SetTrigger("Growl");
                        break;
                    }
                }
            }
            
            
        }
    }
}
