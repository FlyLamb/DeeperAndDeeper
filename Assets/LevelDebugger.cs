using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDebugger : MonoBehaviour
{
    public static bool isDebugMode = false;

    public GameObject debugSetup;
    void Start() {
        if(!GameObject.Find("Canvas")) {
            Debug.LogWarning("Running in debug mode. To disable, disable the player's debugging component.");
            Instantiate(debugSetup);
            isDebugMode = true;
        }
    }
}
