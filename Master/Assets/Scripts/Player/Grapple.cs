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

    private Collider2D grapSurface = default;

    private void Start()
    {
        distJoint = GetComponent<DistanceJoint2D>();
        distJoint.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        grapSurface = Physics2D.OverlapCircle(transform.position, hookRange, grapLayer);

        if (grapSurface != null)
        {
            if (Input.GetButton("Grapple") && transform.GetComponent<Rigidbody2D>().velocity.y != 0 && !distJoint.enabled)
            {
                Debug.Log("Shoot");
                distJoint.enabled = true;
                distJoint.connectedBody = grapSurface.transform.GetComponent<Rigidbody2D>();
                Debug.DrawLine(transform.position, distJoint.transform.position, Color.black);

                if (transform.position.x < grapSurface.transform.position.x)
                {
                    glideDirection = false;
                }
                else
                {
                    glideDirection = true;
                }

                if (Input.GetButtonDown("Jump"))
                {
                    distJoint.enabled = false;
                    distJoint.connectedBody = null;
                    transform.GetComponent<PlayerMovement>().Jump();
                }
            }
            else if (Input.GetButtonUp("Grapple") && distJoint)
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
