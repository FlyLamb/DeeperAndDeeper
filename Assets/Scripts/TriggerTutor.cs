using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TriggerTutor : MonoBehaviour
{
    public string txt; public string wtag; public bool oneTime = true;
    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.CompareTag(wtag)) {
            Messenger.instance.Msg(txt);
            if(oneTime) Destroy(gameObject);
        }
    }
}
