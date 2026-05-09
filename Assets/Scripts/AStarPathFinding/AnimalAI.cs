using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AnimalAI : MonoBehaviour
{
    [Header("Shelter")]
    public string targetShelterTag; 
    private GameObject targetShelter;
    
    [Header("Movement Setting")]
    public float moveDelay = 0.5f;   // Thời gian nghỉ giữa mỗi bước đi
    public float moveSpeed = 3f;     // Tốc độ trượt giữa các ô
    
    private AStarManager aStarManager;
    private bool isMoving = false;
    private List<Vector3> resultPath;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aStarManager = FindObjectOfType<AStarManager>();
        // Bắt đầu chu kỳ suy nghĩ và di chuyển
        StartCoroutine(ThinkAndMoveRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private IEnumerator ThinkAndMoveRoutine()
    {
        //1. Chờ đến khi scanmap xong và Instan ra shelter
        while (targetShelter == null)
        {
            GameObject myShelter = GameObject.FindWithTag(targetShelterTag);
            if (myShelter != null)
            {
                targetShelter = myShelter;
            }
            // Luôn có yield để không treo máy trong khi chờ
            yield return new WaitForSeconds(2f); 
        }
        // 2. Lúc di chuyeern
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
            yield return new WaitForSeconds(moveDelay);
        }
    }
    
    private IEnumerator SmoothMovement(Vector3 endPos)
    {
        isMoving = true;
        float elapsedTime = 0;
        float timeToMove = 1f / moveSpeed; // Thời gian để đi hết 1 ô
        
        Vector3 startPos = transform.position;

        // Xử lý quay mặt (Flip Sprite) dựa trên hướng đi
        SpriteRenderer spriteRender = GetComponent<SpriteRenderer>();
        if (spriteRender != null)
        {
            //Nếu x đích(endPos) > x đi( startPos) thì đang đi sang phair và nguowcj lại
            if (endPos.x > startPos.x) spriteRender.flipX = false; // Nhìn phải
            else if (endPos.x < startPos.x) spriteRender.flipX = true; // Nhìn trái
        }

        while (elapsedTime < timeToMove)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / timeToMove);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPos;
        isMoving = false;
    }
}
