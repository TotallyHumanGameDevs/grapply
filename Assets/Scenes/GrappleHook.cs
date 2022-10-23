using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    public bool boostAvailable;
    public float boostAmount;
    public float hookSpeed;
    [SerializeField] Rigidbody2D hook;


    Rigidbody2D rb;
    DistanceJoint2D joint;
    LineRenderer line;
    

    bool hookLaunching;
    Vector2 hookDestination;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
        line = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Mouse0)){
            line.enabled = true;
            hookLaunching = true;
            hookDestination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        /*if (hookLaunching){
            
        }*/



        if (Input.GetKeyDown(KeyCode.Mouse0)){
            joint.connectedAnchor = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            line.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            joint.enabled = true;
            line.enabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0)){
            joint.enabled = false;
            line.enabled = false;
        }
        line.SetPosition(0, transform.position);


        if (Input.GetKeyDown(KeyCode.Space)){
            float hyp = Mathf.Sqrt(Mathf.Pow(rb.velocity.x, 2f) + Mathf.Pow(rb.velocity.y, 2f));
            Vector2 velVector = new Vector2(rb.velocity.x / hyp, rb.velocity.y / hyp);

            rb.velocity = new Vector2(velVector.x * boostAmount, velVector.y * boostAmount);
        }
    }
}
