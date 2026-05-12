using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnToggleSound(bool isOn)
    {
        if (isOn)
        {
            AudioListener.pause = true;
        }
        else if (!isOn)
        {
            AudioListener.pause = false;
        }
    }
}
