using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : ConveyourOther {
    private new Rigidbody2D rigidbody;

    public float speed = 3;
    public float jumpForce = 300;

    private bool onGround = false;

    public Vector2 groundColliderSize = new Vector2(.075f,0.1f);

    public LayerMask groundMask;
    public Transform graphic;

    public Transform cast1,cast2,cast3;

    private Quaternion targetRotation;

    public PlayerNeck neck;


    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        if(!neck.isAlive) return;

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
        rigidbody.velocity = new Vector2(input.x * speed,rigidbody.velocity.y) + velocity;
        velocity = Vector2.zero;
        var hit = BoxCast(transform.position - new Vector3(.5f,.55f), groundColliderSize, 0, Vector2.right, 1, groundMask);
        onGround = hit;

        RaycastHit2D p1 = Physics2D.Raycast(cast1.position, -cast1.up,6f,groundMask);
        RaycastHit2D p2 = Physics2D.Raycast(cast2.position, -cast2.up,6f,groundMask);
        RaycastHit2D p3 = Physics2D.Raycast(cast3.position, -cast3.up,6f,groundMask);
        Vector2 avgNormal = (p1.normal + p2.normal + p3.normal) * 0.33f;
        
        Debug.DrawLine(cast1.position,p1.point,Color.red, Time.fixedDeltaTime);
        Debug.DrawLine(cast2.position,p2.point,Color.red, Time.fixedDeltaTime);
        targetRotation = Quaternion.LookRotation( Quaternion.Euler(0,0,90) * avgNormal);

        graphic.rotation = Quaternion.Lerp(graphic.rotation,targetRotation,Time.deltaTime * 6);
        Debug.DrawLine(graphic.position,(Vector2)graphic.position + avgNormal * 7, Color.red, Time.fixedDeltaTime);
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
