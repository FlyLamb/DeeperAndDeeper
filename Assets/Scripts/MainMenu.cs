using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    public GameObject pressspace;
    private int time = 0;
    void Update() {
        if(Input.GetButtonDown("Jump"))
            SceneManager.LoadScene(1);
        if(Input.GetButtonDown("Cancel"))
            Application.Quit();
        

        
    }

    void FixedUpdate() {
        if(time % 50 == 0)
            pressspace.SetActive(!pressspace.activeSelf);
        time++;
    }
    
}
