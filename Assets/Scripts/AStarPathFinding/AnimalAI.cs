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
    
    //Other Class 
    private AStarManager aStarManager;
    private GameManager gameManager;
    private Animator animator;
    
    private List<Vector3> resultPath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aStarManager = FindObjectOfType<AStarManager>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        animator = GetComponentInChildren<Animator>();
        // Bắt đầu chu kỳ suy nghĩ và di chuyển
        StartCoroutine(ThinkAndMoveRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator ThinkAndMoveRoutine()
    {
        yield return new WaitUntil(() => gameManager.Target == gameManager.currentTarget);
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
                yield return new WaitForSeconds(3.0f);
            }
    }
    
    private IEnumerator SmoothMovement(Vector3 endPos)
    {
        Vector3 startPos = transform.position;
        float elapseTime = 0;
        float timeToMove = 0.3f;
        while (elapseTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapseTime / timeToMove);
            elapseTime += Time.deltaTime;
            yield return null;
        }
        animator.SetBool("IsMoving", true);
        transform.position = endPos;
    }
    
    //Random Emotion
    private void RandomEmotion()
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
}
