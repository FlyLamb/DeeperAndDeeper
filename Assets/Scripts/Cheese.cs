using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheese : MonoBehaviour
{
    private PlayerNeck neck;

    void Start () {
        neck = FindObjectOfType<PlayerNeck>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("Cheese collected");
        if(!neck.cheeseEquipped && (other.gameObject.CompareTag("Head") || other.gameObject.CompareTag("Player"))) {
            neck.cheeseEquipped = true;
            Destroy(gameObject);
        }
   }
}
