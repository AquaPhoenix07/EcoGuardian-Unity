using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private LayerMask obstacleLayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovementSetting();
    }

    private void MovementSetting()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (CanMoveTo(Vector3.up))
            {
                transform.position += Vector3.up;
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (CanMoveTo(Vector3.left))
            {
                transform.position += Vector3.left;
            }
        }

        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (CanMoveTo(Vector3.right))
            {
                transform.position += Vector3.right;
            }
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (CanMoveTo(Vector3.down))
            {
                transform.position += Vector3.down;
            }
        }
    }

    private bool CanMoveTo(Vector3 direction)
    {
        //Do chỉnh pivot là bottomleft nên phải có phần bù vào
        Vector3 centerOffset = new Vector3(0.5f, 0.5f, 0);
        //transform.position trả ra toạ độ  ở bottom left nên phải cộng thêm offset
        Vector3 startPos = transform.position + centerOffset;
        Vector3 endPos = startPos + direction;
        
        //Vẽ tạm cái raycast
        Debug.DrawLine(startPos, endPos, Color.red, 0.5f);
        RaycastHit2D hit = Physics2D.Linecast(startPos, endPos, obstacleLayer);
        return hit.collider == null;
    }
    
    
}
