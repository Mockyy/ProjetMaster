using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grapple : MonoBehaviour
{
    private DistanceJoint2D distJoint = default;
    [SerializeField] private LayerMask grapLayer = default;
    [SerializeField] private float hookRange = default;
    public float glideSpeed = default;
    public bool glideDirection = default;

    private Collider2D[] grapSurface = default;

    private void Start()
    {
        distJoint = GetComponent<DistanceJoint2D>();
        distJoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        grapSurface = Physics2D.OverlapCircleAll(transform.position, hookRange, grapLayer);

        if (grapSurface != null)
        {
            if (Input.GetButtonDown("Grapple") && transform.GetComponent<Rigidbody2D>().velocity.y != 0 && !distJoint.enabled)
            {
                Debug.Log("Shoot");
                distJoint.enabled = true;
                distJoint.connectedBody = grapSurface[0].transform.GetComponent<Rigidbody2D>();

                if (transform.position.x < grapSurface[0].transform.position.x)
                {
                    glideDirection = false;
                }
                else
                {
                    glideDirection = true;
                }
            }
            else if (Input.GetButtonDown("Grapple") && distJoint)
            {
                distJoint.enabled = false;
                distJoint.connectedBody = null;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, hookRange);
    }
}
