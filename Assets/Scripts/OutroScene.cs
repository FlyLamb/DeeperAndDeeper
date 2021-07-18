using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutroScene : MonoBehaviour
{
    void Update() {
        if(Input.GetButtonDown("Jump")) {
            Application.Quit();
        }
    }
}
