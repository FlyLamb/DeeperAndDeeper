using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseProjectile : MonoBehaviour {
    private Rigidbody2D rb;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        GetComponent<Animator>().speed = rb.velocity.magnitude / 4;
    }
}
