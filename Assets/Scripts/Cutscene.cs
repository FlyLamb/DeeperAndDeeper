using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Cutscene : MonoBehaviour
{
    public bool skip;

    void Update() {
        skip = skip || Input.GetButtonDown("Jump");
        if(skip) {
            SceneManager.LoadScene(2);
        }
    }
}
