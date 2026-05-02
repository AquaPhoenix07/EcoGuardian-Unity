using Unity.VisualScripting;
using UnityEngine;

public class Target : MonoBehaviour
{
    private GameManager gameManager;
    private SpriteRenderer sprite;
    public Sprite completedSprite;

    private bool isCompleted = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(isCompleted) return;
        if (collider.CompareTag("Garbage"))
        {
            Destroy(collider.gameObject); 
            sprite.sprite = completedSprite;
            gameManager.UpdateScore(1);
        }
        else if (collider.CompareTag("Fertilizer"))
        {
            Destroy(collider.gameObject);
            sprite.sprite = completedSprite;
            gameManager.UpdateScore(1);
        }
        isCompleted = true;
    }
}
