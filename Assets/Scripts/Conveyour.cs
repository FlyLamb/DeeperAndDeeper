using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyour : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask mask;
    public Material material;

    public MeshRenderer mrender;

    void Start() {
        mrender.material = Instantiate(material);
        mrender.material.SetFloat("_ScrollSpeed",-moveSpeed);
    }

    void OnTriggerStay2D(Collider2D other) {
        if(mask == (mask | (1 << other.gameObject.layer))) {
            var rb = other.GetComponent<Rigidbody2D>();
            var pb = other.GetComponent<ConveyourOther>();
            if(pb != null) {
                pb.velocity = new Vector2(moveSpeed, 0);
            } else if(rb != null && rb.bodyType != RigidbodyType2D.Static) {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            }

            
            
        }
    }
}
