using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rat : ConveyourOther
{
    private new Rigidbody2D rigidbody;
    private Transform playerHead;

    public float alertDistance = 10, attackDistance = 3;
    public bool wanderAround;
    public float speed, jumpForce, jumpCooldown;

    private float cooldown = 5f;

    public bool hasJumped = false;

    private Transform graphic;
    private Animator animator;

    private Quaternion targetDir;

    private bool pacified;

    public GameObject cheese;

    public Transform pointSensor;

    

    void Start() {
        rigidbody = GetComponent<Rigidbody2D>();
        playerHead = GameObject.FindGameObjectWithTag("Head").transform;

        graphic = transform.GetChild(0);
        animator = graphic.GetComponent<Animator>();
    }

    void FixedUpdate() {
        graphic.rotation = Quaternion.Lerp(graphic.rotation,targetDir,Time.fixedDeltaTime * 10);
        if(pacified) return;
        float distanceToHead = Vector2.Distance(transform.position, playerHead.position);
        Vector2 walk = new Vector2();
        if(distanceToHead < alertDistance) {
            walk = playerHead.position - transform.position;
        }
        walk.y = 0;
        if(distanceToHead < attackDistance && !hasJumped && cooldown < 0) {
            rigidbody.AddForce(Vector2.up * jumpForce + walk * jumpForce /2);
            
            hasJumped = true;
            cooldown = jumpCooldown;
        }
        
        cooldown -= Time.fixedDeltaTime;

        if(cooldown > 0.5f*jumpCooldown) {
            walk = -walk;
        }
        
        walk = walk.normalized;
        animator.SetBool("walk",!hasJumped);
        if(!hasJumped) {
            rigidbody.velocity = new Vector2(0,rigidbody.velocity.y) + walk * speed + velocity;
            if(Physics2D.CircleCast(pointSensor.position,0.02f,Vector2.right,0.02f)) {
                cooldown = jumpCooldown;
                walk = -walk;
                Debug.Log("RAT TURN");
            }
        }


        if(walk.x > 0) {
            targetDir = Quaternion.Euler(0,90,0);
        } else if (walk.x < 0) {
            targetDir = Quaternion.Euler(0,-90,0);
        }

        animator.speed = Mathf.Abs(walk.x);
        
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.gameObject.CompareTag("Cheese")) {
            pacified = true;       
            animator.SetBool("pacified",true);
            cheese.SetActive(true);
            targetDir = Quaternion.Euler(0,-90,0);
            Destroy(collision.collider.gameObject);
        }

        if(hasJumped) 
            foreach(var v in collision.contacts)
                if(v.point.y < transform.position.y)
                    hasJumped = false;

        if(!hasJumped && pacified) {Destroy(GetComponent<Collider2D>()); Destroy(rigidbody); } ;
    }
}
