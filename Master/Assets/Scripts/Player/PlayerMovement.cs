using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D col;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private Transform wallCheckPoint;
    [SerializeField] private LayerMask whatIsGround = default;
    [SerializeField] private LayerMask whatIsWall = default;

    private float movementInput;
    [SerializeField] private float speed = default;
    [SerializeField] private float jumpForce = default;
    [SerializeField] private float wallSlidingSpeed = default;
    [SerializeField] private float xWallJumpForce = default;
    [SerializeField] private float yWallJumpForce = default;

    private float wallJumpTime = 0.2f;
    private float wallJumpCounter;

    private bool isOnWall;
    private bool isGrounded;    //Si le player touche le sol
    private int jumpsLeft;      //Le nombre de sauts restants
    private int jumpsMax = 1;       //Le nombre max de sauts

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (wallJumpCounter <= 0)
        {
            Move();

            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatIsGround);
            if (isGrounded && jumpsLeft < jumpsMax)
            {
                jumpsLeft = jumpsMax;
            }

            isOnWall = Physics2D.OverlapCircle(wallCheckPoint.position, 0.2f, whatIsWall);

            if (isOnWall && !isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }

            if (Input.GetButtonDown("Jump"))
            {
                if ((isGrounded || jumpsLeft > 0))
                {
                    Jump();
                }
                else if (isOnWall && !isGrounded)
                {
                    wallJumpCounter = wallJumpTime;
                    WallJump();
                }
            }
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }
    }

    private void Move()
    {
        movementInput = Input.GetAxisRaw("Horizontal");

        rb.velocity = new Vector2(movementInput * speed, rb.velocity.y);
        //rb.position += new Vector2(movementInput * speed * Time.deltaTime, 0);

        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-10f, 10f);
        }
        else if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(10f, 10f);
        }
    }

    private void Jump()
    {
         resetVelocity();
         rb.AddForce(Vector2.up * jumpForce);
         jumpsLeft--;
    }

    private void WallJump()
    {
        //resetVelocity();
        rb.velocity = new Vector2(xWallJumpForce * -movementInput, yWallJumpForce);
    }

    //private bool CheckGround()
    //{
    //    RaycastHit2D raycastHit = Physics2D.Raycast(
    //        col.bounds.center, Vector2.down, col.bounds.extents.y + 0.3f, platformsLayer);
    //    Color rayColor;

    //    if (raycastHit.collider != null)
    //    {
    //        jumpsLeft = jumpsMax;
    //        rayColor = Color.green;
    //    }
    //    else
    //    {
    //        rayColor = Color.red;
    //    }

    //    Debug.DrawRay(col.bounds.center, Vector3.down * (col.bounds.extents.y + 0.3f), rayColor);

    //    return raycastHit.collider != null;
    //}

    //private bool CheckWalls()
    //{
    //    RaycastHit2D raycastHitLeft = Physics2D.Raycast(
    //        col.bounds.center, Vector2.left, col.bounds.extents.x + 0.3f, platformsLayer);
    //    RaycastHit2D raycastHitRight = Physics2D.Raycast(
    //        col.bounds.center, Vector2.right, col.bounds.extents.x + 0.3f, platformsLayer);
    //    Color rayColor;

    //    if (raycastHitLeft.collider != null || raycastHitRight.collider != null)
    //    {
    //        rayColor = Color.green;
    //    }
    //    else
    //    {
    //        rayColor = Color.red;
    //    }

    //    Debug.DrawRay(col.bounds.center, Vector3.left * (col.bounds.extents.y + 0.3f), rayColor);
    //    Debug.DrawRay(col.bounds.center, Vector3.right * (col.bounds.extents.y + 0.3f), rayColor);

    //    if (raycastHitLeft.collider != null || raycastHitRight.collider != null)
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    private void resetVelocity()
    {
        rb.velocity = new Vector2(0, 0);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheckPoint.position, .2f);
        Gizmos.DrawSphere(wallCheckPoint.position, .2f);
    }
}
