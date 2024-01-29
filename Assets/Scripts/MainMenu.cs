using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update() {
        if(Input.GetMouseButtonDown(0))
        {
            LoadGame();
        }
        
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Level One");
    }
}
