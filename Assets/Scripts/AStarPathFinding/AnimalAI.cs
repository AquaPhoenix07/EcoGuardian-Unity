using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AnimalAI : MonoBehaviour
{
    public string targetShelterTag;
    private GameObject targetShelter;
    
    public float moveDelay = 0.5f;
    private AStarManager aStarManager;
    private bool isMoving = false;
    private List<Vector3> resultPath;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        aStarManager = FindObjectOfType<AStarManager>();
        StartCoroutine(ThinkAndMoveRoutine());

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator ThinkAndMoveRoutine()
    {
        while (targetShelter == null)
        {
            GameObject myShelter = GameObject.FindWithTag(targetShelterTag);
            if (myShelter != null)
            {
                targetShelter = myShelter;
            }
            yield return new WaitForSeconds(moveDelay);
        }
        while (true)
        {
            yield return new WaitForSeconds(moveDelay);
            if (!isMoving && targetShelter != null)
            {
                if (Vector3.Distance(targetShelter.transform.position, transform.position) < 0.1f)
                {
                    Debug.Log("Về Đích");
                    yield break;
                }
            }
            
            resultPath = aStarManager.FindPath(transform.position, targetShelter.transform.position);
            if (resultPath != null && resultPath.Count > 0)
            {
                Vector3 nextPos = resultPath[0];
                gameObject.transform.position = nextPos;
            }
        }
        

    }
}
