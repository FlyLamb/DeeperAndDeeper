using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RedButton : MonoBehaviour
{
    public Tag[] activators;
    public Color buttonColor = new Color(0.7f,0.1f,0.1f);

    [HideInInspector]
    public bool __SHOULDTRIGGER; // a stupid approach, but it is working lol
    [HideInInspector]
    public float __DELAY;

    private MeshRenderer button;
    private AudioSource source;

    void Start() {
        button = transform.Find("Button").GetComponent<MeshRenderer>(); // suboptimal
        source = GetComponent<AudioSource>();
        button.material.color = buttonColor;
        button.material.SetColor("_EmissionColor", buttonColor * 4);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.HasAnyTag(activators)) {
            if(__DELAY <= 0f) {
                __SHOULDTRIGGER = true;
                __DELAY = 0.2f;
                source.Play();
            }
        }
    }

    void OnTriggerStay2D(Collider2D other) {
        if(other.HasAnyTag(activators)) {
            __SHOULDTRIGGER = false;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.HasAnyTag(activators)) {
            __SHOULDTRIGGER = false;
        }
    }

    void FixedUpdate() {
        if(__DELAY > 0) {
            __DELAY -= Time.fixedDeltaTime;
            button.material.EnableKeyword("_EMISSION");
        } else
        button.material.DisableKeyword("_EMISSION");
    }
}
