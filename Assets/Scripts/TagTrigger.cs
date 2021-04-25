using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TagTrigger : MonoBehaviour {
    public UnityEvent triggerEvent;
    public string requiredTag;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag(requiredTag)) {
            triggerEvent.Invoke();
        }
    }
}
