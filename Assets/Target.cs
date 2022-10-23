using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] Transform topRightBound, bottomLeftBound;
    [SerializeField] LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Collider2D>().IsTouchingLayers(playerLayer)){
            transform.position = new Vector2(Random.Range(bottomLeftBound.position.x, topRightBound.position.x), 
            Random.Range(bottomLeftBound.position.y, topRightBound.position.y));
        }
    }
}
