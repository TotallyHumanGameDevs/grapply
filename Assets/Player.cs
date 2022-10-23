using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool boostAvailable;
    public float boostAmount;
    public float hookSpeed;
    public float addedBallVel;
    public float pullSpeed;
    public float releaseSpeed;


    Rigidbody2D rb;
    DistanceJoint2D joint;
    LineRenderer line;
    Rope ropeThing;
    [SerializeField] Rigidbody2D hook, ball;

    bool hookLaunching;
    bool hookRetracting;
    bool hookConnected;
    Vector2 hookPos;
    Vector2 hookDestination;
    bool queueLaunch;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<DistanceJoint2D>();
        line = GetComponent<LineRenderer>();
        ropeThing = GetComponent<Rope>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(transform.position, new Vector2(rb.velocity.x + transform.position.x, rb.velocity.y + transform.position.y), Color.red);

        if ((Input.GetKeyDown(KeyCode.Mouse0) && !hookRetracting)){
            line.enabled = true;
            hookLaunching = true;

            hookDestination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 direction = new Vector2(hookDestination.x - transform.position.x, hookDestination.y - transform.position.y);
            hook.transform.position = transform.position;

            hook.gameObject.SetActive(true);
            float hyp = FindDistance(direction);
            hook.velocity = new Vector2((direction.x / hyp) * hookSpeed, (direction.y / hyp) * hookSpeed);
        }
        if (Input.GetKey(KeyCode.Mouse0) && queueLaunch && !hook.gameObject.activeInHierarchy){
            line.enabled = true;
            hookLaunching = true;
            queueLaunch = false;

            Vector2 direction = new Vector2(hookDestination.x - transform.position.x, hookDestination.y - transform.position.y);
            hook.transform.position = transform.position;

            hook.gameObject.SetActive(true);
            float hyp = FindDistance(direction);
            hook.velocity = new Vector2((direction.x / hyp) * hookSpeed, (direction.y / hyp) * hookSpeed);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && hookRetracting){
            queueLaunch = true;
            hookDestination = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else if (hookRetracting && Input.GetKeyUp(KeyCode.Mouse0)){
            queueLaunch = false;
        }

        if (hookLaunching){
            line.SetPosition(1, hook.position);

            if(Input.GetKeyUp(KeyCode.Mouse0)){
                hookLaunching = false;
                hookRetracting = true;
            }
            else if ((hook.velocity.y > 0 && hook.position.y >= hookDestination.y) || (hook.velocity.y <= 0 && hook.position.y <= hookDestination.y)){
                hookLaunching = false;
                hookConnected = true;
                hook.velocity = new Vector2();

                joint.enabled = true;
                joint.connectedAnchor = hookDestination;
                hook.position = hookDestination;

                ropeThing.enabled = true;
                ropeThing.length = joint.distance;
            }
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && hookConnected){
            line.positionCount = 2;
            joint.enabled = false;
            ropeThing.enabled = false;

            hookLaunching = false;
            hookConnected = false;
            hook.velocity = new Vector2();

            hookRetracting = true;

            if(Input.GetKey(KeyCode.W)){
                Vector2 hookVect = new Vector2(hook.position.x - transform.position.x, hook.position.y - transform.position.y);
                hookVect /= Vector2.Distance(new Vector2(), hookVect);
                rb.velocity = new Vector2(rb.velocity.x + (pullSpeed * hookVect.x), rb.velocity.y + (pullSpeed * hookVect.y));
            }
        }
        if(hookRetracting){
            line.SetPosition(1, hook.position);

            Vector2 direction = new Vector2(transform.position.x - hook.position.x, transform.position.y - hook.position.y);
            float hyp = FindDistance(direction);
            hook.velocity = new Vector2((direction.x / hyp) * hookSpeed * 2f, (direction.y / hyp) * hookSpeed * 2f);

            if (hyp <= 1f){
                hookRetracting = false;
                line.enabled = false;
                ropeThing.enabled = false;
                hook.velocity = new Vector2();
                hook.gameObject.SetActive(false);
            }
        }

        line.SetPosition(0, transform.position);


        if(Input.GetKey(KeyCode.W) && hookConnected){
            /*pulling = true;

            Vector2 hookVector = new Vector2(hook.position.x - transform.position.x, hook.position.y - transform.position.y);
            float centrifugalForce = Mathf.Cos((180f - Vector2.Angle(hookVector, rb.velocity)) * Mathf.Deg2Rad) * FindDistance(rb.velocity);

            Vector2 centForceVect = (hookVector / FindDistance(hookVector)) * centrifugalForce;
            Vector2 pullForceVect = (hookVector / FindDistance(hookVector)) * pullSpeed;

            rb.velocity = new Vector2(rb.velocity.x + (centForceVect.x + pullForceVect.x) * Time.deltaTime, 
                rb.velocity.y + (centForceVect.y + pullForceVect.y));*/

            Vector2 hookVector = new Vector2(hook.position.x - transform.position.x, hook.position.y - transform.position.y);
            float centrifugalForce = Mathf.Cos((180f - Vector2.Angle(hookVector, rb.velocity)) * Mathf.Deg2Rad) * FindDistance(rb.velocity);

            Vector2 centForceVect = (rb.velocity / FindDistance(rb.velocity)) * centrifugalForce;

            rb.velocity = new Vector2(rb.velocity.x + centForceVect.x * pullSpeed * Time.deltaTime, rb.velocity.y + centForceVect.y * pullSpeed * Time.deltaTime);

            joint.distance -= pullSpeed * Time.deltaTime;
            ropeThing.length -= pullSpeed * Time.deltaTime;
        }
        if(Input.GetKey(KeyCode.S)){
            joint.distance += releaseSpeed * Time.deltaTime;
            ropeThing.length += releaseSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space)){
            if (rb.velocity == new Vector2()){
                rb.velocity = new Vector2(0f, boostAmount);
            }
            else{
            float hyp = FindDistance(rb.velocity);
            rb.velocity = new Vector2((rb.velocity.x / hyp) * boostAmount, (rb.velocity.y / hyp) * boostAmount);
            }
        }

        if (Input.GetKeyDown(KeyCode.C)){
            ball.gameObject.SetActive(true);
            ball.position = transform.position;
            float hyp = FindDistance(rb.velocity);
            ball.velocity = new Vector2(rb.velocity.x + (addedBallVel * (rb.velocity.x / hyp)), rb.velocity.y + (addedBallVel * (rb.velocity.y / hyp)));
        }
    } 

    float FindDistance(Vector2 vector1){
        return Mathf.Sqrt(Mathf.Pow(vector1.x, 2f) + Mathf.Pow(vector1.y, 2f));
    }
    float FindDistance(Vector2 vector1, Vector2 vector2){
        Vector2.Angle(new Vector2(transform.position.x - hook.position.x, transform.position.y - hook.position.y), rb.velocity);
        return Mathf.Sqrt(Mathf.Pow(vector1.x - vector2.x, 2f) + Mathf.Pow(vector1.y - vector2.y, 2f));
    }
}
