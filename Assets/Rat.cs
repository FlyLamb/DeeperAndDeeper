using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : ConveyourOther {
    
    
    public float jumpForce;
    public Vector2 groundColliderSize = new Vector2(.075f,0.1f);
    public LayerMask groundMask;
    public float jumpCooldown;

    public bool onGround;

    private float cooldown;

    private new Rigidbody2D rigidbody;
    private Transform playerHead;

    public bool active = true;

    private Quaternion destRotation;
    private Animator animator;
    private Transform graphic;
    public GameObject i_cheese;

    public bool faceLeft = true;

    public Vector2 multiplyThrowSpeed = Vector2.one;

    public void Activate() {
        if(!animator.GetBool("pacified"))
            active = true;
    }

    void Start() {
        cooldown = jumpCooldown;
        Debug.Log("Rat spawn");
        rigidbody = GetComponent<Rigidbody2D>();
        playerHead = GameObject.FindGameObjectWithTag("Head").transform;
        graphic = transform.GetChild(0);
        animator = graphic.GetComponent<Animator>();
        if(faceLeft)
            destRotation = Quaternion.Euler(0,-90,0);
            else destRotation = Quaternion.Euler(0,90,0);
    }

    void Update() {
        graphic.rotation = Quaternion.Lerp(graphic.rotation,destRotation,Time.deltaTime * 10);
    }

    void FixedUpdate() {
        animator.SetBool("walk",onGround);
        if(!active) return;

        onGround = Physics2D.BoxCast(transform.position - new Vector3(.5f,.55f), groundColliderSize, 0, Vector2.right, 1, groundMask);
        
        if(onGround && cooldown > 0) {
            cooldown -= Time.fixedDeltaTime;
            
            destRotation *= Quaternion.Euler(0,Time.deltaTime * 90,0);

            if(cooldown <= 0) {
                Jump();
            }
        }

        if(!onGround) {
            if(rigidbody.velocity.x > 0)
                destRotation = Quaternion.Euler(0,90,0);
            else if(rigidbody.velocity.x < 0 )
                destRotation = Quaternion.Euler(0,-90,0);
        }
    }

    public void Jump() {
        cooldown = jumpCooldown;

        Vector2 directionToPlayerHead = playerHead.position - transform.position;
        directionToPlayerHead = directionToPlayerHead.normalized * jumpForce * Random.Range(0.7f,1f);
        directionToPlayerHead.y = Mathf.Abs(directionToPlayerHead.y) + jumpForce / 2.82f;

        directionToPlayerHead.x *= multiplyThrowSpeed.x;
        directionToPlayerHead.y *= multiplyThrowSpeed.y;
        rigidbody.AddForce(directionToPlayerHead);
    }

    public void Pacify() {
        active = false;
        destRotation = Quaternion.Euler(0,-90,0);
        animator.SetBool("walk",false);
        animator.SetBool("pacified",true);
        i_cheese.SetActive(true);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.gameObject.CompareTag("Cheese")) {
            Pacify();
            Destroy(collision.collider.gameObject);
        }
    }
}
