using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerNeck : MonoBehaviour {
    

    [System.Serializable]
    public class NeckDirection {
        public float distance;
        public Dir direction;


        public NeckDirection(Dir dir) {
            this.direction = dir;
        }
[System.Serializable]
        public enum Dir {
            up,
            down,
            left,
            right,
            none
        }
    }


    private List<NeckDirection> neckMovement;
    private new Rigidbody2D rigidbody;
    
    public float maxLength = 5;
    public float currentLength;
    
    public LineRenderer lineRenderer;
    public LayerMask neckBreakMask;
    public bool isAlive = true;

    public float neckSpeed = 2;
    public Transform headGfx;

    public Transform headRoot;
    public GameObject adparticle,adlight1;

    public GameObject deadPanel;

    private Quaternion destHeadRotation;

    public bool cheeseEquipped = false;
    public GameObject cheesePrefab;
    public float shootForce = 400;
    public GameObject cheeseGfx;

    public AudioSource audioSource;
    public AudioClip dieClip;

    private NeckDirection.Dir lastDir; // LEFT RIGHT DIR

    private Vector3 headPositionDelta = Vector3.zero;
    private Vector3 headPositionPrevious = Vector3.zero;


    void Start() {
        deadPanel = GameObject.Find("Dead Panel");
        LeanTween.scaleX(deadPanel, 0, 0f);
        rigidbody = GetComponent<Rigidbody2D>();
        neckMovement = new List<NeckDirection>();
        destHeadRotation = Quaternion.Euler(-90,0,-90);

        headPositionPrevious = headGfx.localPosition;
    }

    void DoNeckMovement() {
        Vector2 input = new Vector2(Input.GetAxis("Neck Horizontal"),Input.GetAxis("Neck Vertical"));
        var direction = V2Dir(input);

        audioSource.volume = (direction.distance < 2) ? 0 : 0.5f;

        if(direction.distance == 0) return;


        
        if(neckMovement.Count == 0) {
            neckMovement.Add(direction);
        }

        // Snap the neck back when is in the error margin
        if(neckMovement.Last().distance < 2 && neckMovement.Last().direction != direction.direction) 
            neckMovement.Remove(neckMovement.Last());      

        if(neckMovement.Count == 0) return;
        if(neckMovement.Last().distance < 40 && neckMovement.Count > 1) {
            if(IsOpposite(neckMovement[neckMovement.Count - 2].direction,direction.direction) ) { 
                neckMovement.Remove(neckMovement.Last());
            }
        }
        if(neckMovement.Last().distance < 10 && neckMovement.Count > 1) {
            if(neckMovement[neckMovement.Count - 2].direction == direction.direction) {
                neckMovement.Remove(neckMovement.Last());
            }
        }

        if(neckMovement.Last().direction == NeckDirection.Dir.right || neckMovement.Count == 1) {
            destHeadRotation = Quaternion.Euler(-90,0,-90);
            lastDir = NeckDirection.Dir.right;
        } else if(neckMovement.Last().direction == NeckDirection.Dir.left) {
            destHeadRotation = Quaternion.Euler(-90,180,-90);
            lastDir = NeckDirection.Dir.left;
        }

        currentLength = CalculateNeckLength(neckMovement);
        if(neckMovement.Last().direction == direction.direction && currentLength < maxLength && CanMove(neckMovement,direction,Time.fixedDeltaTime)) neckMovement.Last().distance += direction.distance;
        else if(IsOpposite(neckMovement.Last().direction, direction.direction) && CanMove(neckMovement,direction,Time.fixedDeltaTime)) neckMovement.Last().distance -= direction.distance;
        else if(currentLength < maxLength && CanMove(neckMovement,direction,Time.fixedDeltaTime)) neckMovement.Add(direction);
    }

    void DoCollision() {
        var poss = GetPointsFromInstruction(neckMovement,Time.fixedDeltaTime);
        bool shouldBreak = false;

        for(int i = 0; i < poss.Length - 1 ; i++) {
            Vector2 pointA = poss[i];
            Vector2 pointB = poss[i+1];

            shouldBreak = shouldBreak || Physics2D.Linecast((Vector2)transform.parent.position + pointA, (Vector2)transform.parent.position + pointB, neckBreakMask);
        }

        if(shouldBreak) {
            StartCoroutine(Die());
        }

        isAlive &= !shouldBreak;
    }

    IEnumerator Die() {
        audioSource.Stop();
        audioSource.clip = dieClip;
        audioSource.loop = false;
        audioSource.Play();
        LeanTween.scaleX(deadPanel, 1, 0.2f).setEase(LeanTweenType.easeOutQuad);
        
        headRoot.parent = null;
        Destroy(gameObject.GetComponent<Rigidbody2D>());
        headRoot.gameObject.AddComponent<CircleCollider2D>();
        headRoot.gameObject.AddComponent<Rigidbody2D>();
        adparticle.SetActive(true);
        adlight1.SetActive(false);
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(2f);

        LevelLoader.instance.StartCoroutine(LevelLoader.instance.LoadLevel(false));

    }

    void DoVisual() {
        var poss = GetPointsFromInstruction(neckMovement,Time.fixedDeltaTime, 4);
        lineRenderer.positionCount = poss.Length;
        lineRenderer.SetPositions(poss);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.R)) {
            LevelLoader.instance.StartCoroutine(LevelLoader.instance.LoadLevel(false));
        }


        if(!isAlive) return;

        headGfx.rotation = Quaternion.Lerp(headGfx.rotation,destHeadRotation,Time.deltaTime * 4);

        if(Input.GetButtonDown("Jump")) {
            if(cheeseEquipped) {
                var ins = Instantiate(cheesePrefab,(Vector2)transform.position + Dir2V(lastDir), Quaternion.identity);
                ins.GetComponent<Rigidbody2D>().AddForce(Dir2V(lastDir) * shootForce);

                cheeseEquipped = false;
            }
        }

        cheeseGfx.SetActive(cheeseEquipped);
    }

    void FixedUpdate() {
        if(!isAlive) return;

        headPositionDelta = headGfx.localPosition - headPositionPrevious;

        headPositionPrevious = headGfx.localPosition;

        // DO NECK MOVEMENT
        DoNeckMovement();

        // ACTUALLY TRANSPOSE THE NECK
        rigidbody.MovePosition((Vector2)transform.parent.position + GetVectorFromNeckInstruction(neckMovement,Time.fixedDeltaTime));

        // UPDATE LINE RENDERER
        DoVisual();

        // CHECK FOR INTERSECTIONS
        DoCollision();

    }

    public float CalculateNeckLength(List<NeckDirection> instruction) {
        float len = 0;
        foreach(var item in instruction) {
            len += item.distance;
        }

        return len;
    }

    public Vector3[] GetPointsFromInstruction(List<NeckDirection> instruction, float multiplier = 1, int addmidpoints = 0) {
        if(instruction == null || instruction.Count == 0) return new Vector3[0];
        List<Vector3> pts = new List<Vector3>();
        pts.Add(Vector2.zero);
        Vector2 curpos = Vector2.zero;
        foreach(var item in instruction) {
            if(addmidpoints == 0) {
                curpos += Dir2V(item.direction) * item.distance;
                
                pts.Add(curpos * Time.deltaTime);
            } else {
                for(int i = 0; i< addmidpoints; i++) {
                    curpos += Dir2V(item.direction) * item.distance / addmidpoints;
                
                    pts.Add(curpos * Time.deltaTime);
                }
            }
        }

        return pts.ToArray();
    }

    public Vector2 GetVectorFromNeckInstruction(List<NeckDirection> instruction, float multiplier = 1) {
        if(instruction == null || instruction.Count == 0) return Vector2.zero;
        Vector2 sum = Vector2.zero;

        foreach(var item in instruction) {
            sum += Dir2V(item.direction) * item.distance;
        }

        return sum * Time.deltaTime;
    }

    public NeckDirection V2Dir(Vector2 v) {
        if(v.magnitude < 0.01f) return new NeckDirection(NeckDirection.Dir.none);
        v = v.normalized;
        
        if(v.y > 0.5f) return new NeckDirection(NeckDirection.Dir.up) {distance = neckSpeed};
        if(v.y < -0.5f) return new NeckDirection(NeckDirection.Dir.down) {distance =neckSpeed};

        if(v.x > 0.5f) return new NeckDirection(NeckDirection.Dir.right) {distance = neckSpeed};
        if(v.x < -0.5f) return new NeckDirection(NeckDirection.Dir.left) {distance = neckSpeed};

        return new NeckDirection(NeckDirection.Dir.none);
    }

    public Vector2 Dir2V(NeckDirection.Dir dir) {
        switch (dir) {
            case NeckDirection.Dir.up: return Vector2.up;
            case NeckDirection.Dir.down: return Vector2.down;
            case NeckDirection.Dir.left: return Vector2.left;
            case NeckDirection.Dir.right: return Vector2.right;
            default: return Vector2.zero;
        }
    }

    public bool CanMove(List<NeckDirection> instruction, NeckDirection execute, float multiplier = 1) {
        Vector2 position = GetVectorFromNeckInstruction(instruction,multiplier);
        position += Dir2V(execute.direction) * execute.distance * multiplier;
        bool result = !Physics2D.CircleCast((Vector2)transform.parent.position + position,0.03f, Vector2.right,0.03f,neckBreakMask);
       
        Debug.DrawLine(transform.parent.position,(Vector2)transform.parent.position + position,Color.red,0.2f);
        return result;
    }

    public bool IsOpposite(NeckDirection.Dir a,NeckDirection.Dir b) {
        if(a == b) return false;

        
        
        if(a == NeckDirection.Dir.down && b == NeckDirection.Dir.up) return true;
        if(a == NeckDirection.Dir.left && b == NeckDirection.Dir.right) return true;

        var tmp = b;
        b = a;
        a = tmp;
        
        if(a == NeckDirection.Dir.down && b == NeckDirection.Dir.up) return true;
        if(a == NeckDirection.Dir.left && b == NeckDirection.Dir.right) return true;
        
        return false;
    }
}
