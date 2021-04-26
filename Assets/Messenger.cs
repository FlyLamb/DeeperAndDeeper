using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Messenger : MonoBehaviour
{
    public static Messenger instance;

    public TextMeshProUGUI text;
    public CanvasGroup panel;

    void Awake() {
        instance = this;
    }

    public void Msg(string txt) {
        text.text = txt;
        LeanTween.alphaCanvas(panel,1,0.1f);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.E)) {
            LeanTween.alphaCanvas(panel,0,0.1f);
        }
    }
}
