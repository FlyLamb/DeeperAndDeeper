using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    private new Rigidbody2D rigidbody;

    public float speed = 3;
    public float jumpForce = 300;

    private bool onGround = false;

    public Vector2 groundColliderSize = new Vector2(.075f,0.1f);

    public LayerMask groundMask;

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        Vector2 input = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        rigidbody.velocity = new Vector2(input.x * speed,rigidbody.velocity.y);
        onGround = BoxCast(transform.position - new Vector3(.5f,.55f), groundColliderSize, 0, Vector2.right, 1, groundMask);
    }

    void Update() {
        if(Input.GetButtonDown("Jump") && onGround) {
            rigidbody.AddForce(Vector2.up * jumpForce * rigidbody.mass);
            
        }
    }

    static public RaycastHit2D BoxCast( Vector2 origen, Vector2 size, float angle, Vector2 direction, float distance, int mask ) {
        RaycastHit2D hit = Physics2D.BoxCast(origen, size, angle, direction, distance, mask);

        //Setting up the points to draw the cast
        Vector2 p1, p2, p3, p4, p5, p6, p7, p8;
        float w = size.x * 0.5f;
        float h = size.y * 0.5f;
        p1 = new Vector2(-w, h);
        p2 = new Vector2(w, h);
        p3 = new Vector2(w, -h);
        p4 = new Vector2(-w, -h);

        Quaternion q = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        p1 = q * p1;
        p2 = q * p2;
        p3 = q * p3;
        p4 = q * p4;

        p1 += origen;
        p2 += origen;
        p3 += origen;
        p4 += origen;

        Vector2 realDistance = direction.normalized * distance;
        p5 = p1 + realDistance;
        p6 = p2 + realDistance;
        p7 = p3 + realDistance;
        p8 = p4 + realDistance;


        //Drawing the cast
        Color castColor = hit ? Color.red : Color.green;
        Debug.DrawLine(p1, p2, castColor);
        Debug.DrawLine(p2, p3, castColor);
        Debug.DrawLine(p3, p4, castColor);
        Debug.DrawLine(p4, p1, castColor);

        Debug.DrawLine(p5, p6, castColor);
        Debug.DrawLine(p6, p7, castColor);
        Debug.DrawLine(p7, p8, castColor);
        Debug.DrawLine(p8, p5, castColor);

        Debug.DrawLine(p1, p5, Color.grey);
        Debug.DrawLine(p2, p6, Color.grey);
        Debug.DrawLine(p3, p7, Color.grey);
        Debug.DrawLine(p4, p8, Color.grey);
        if(hit) {
            Debug.DrawLine(hit.point, hit.point + hit.normal.normalized * 0.2f, Color.yellow);
        }

        return hit;
    }
}
