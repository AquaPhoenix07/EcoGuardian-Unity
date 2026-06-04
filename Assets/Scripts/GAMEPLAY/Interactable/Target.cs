using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject completedObject;

    private bool isCompleted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(isCompleted) return;
        string itemTag = collider.gameObject.tag;
        string objectTag = gameObject.tag;
        if ((itemTag == "Garbage" && objectTag == "EmptyTrashCan") || (itemTag == "Fertilizer" && objectTag == "DeathTree")
                                                                 || (itemTag == "FreshWater" && objectTag == "PoisionousLake")
                                                                 || (itemTag == "MagicSeed" && objectTag =="HoleToPlant" ))
        {
            isCompleted = true; 
            Destroy(collider.gameObject); 
            Instantiate(completedObject, transform.position, completedObject.transform.rotation);
            gameManager.UpdateScore(1);
            Destroy(gameObject); 
        }
    }
}
