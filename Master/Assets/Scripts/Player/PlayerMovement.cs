using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb; //RigidBody2D de l'avatar
    private Collider2D col; //colliderBox2D de l'avatat
    [SerializeField] private Transform groundCheckPoint;    //Un objet enfant de l'avatar qui sert à localiser le sol
    [SerializeField] private Transform wallCheckPoint;      //Un objet enfant de l'avatar qui sert à localiser les murs
    [SerializeField] private LayerMask whatIsGround = default;  //Le Layer qui contient les objets considérés comme du sol
    [SerializeField] private LayerMask whatIsWall = default;    //Le Layer qui contient les objets considérés comme des murs

    private float movementInput;    //Input de mouvement horizontal
    [SerializeField] private float speed = default;     //Vitesse de l'avatar
    [SerializeField] private float jumpForce = default; //Force de saut de l'avatar
    [SerializeField] private float wallSlidingSpeed = default;  //Vitesse à laquelle l'avatar glisse le long d'un mur
    [SerializeField] private float xWallJumpForce = default;    //Force de saut horizontal depuis un mur
    [SerializeField] private float yWallJumpForce = default;    //Force de saut vertical

    private float wallJumpTime = 0.2f;  //Temps pendant lequel le joueur sera incapacité après un wall jump
    private float wallJumpCounter;      //Compteur du temps

    private bool isOnWall;      //Si le joueur touche un mur
    private bool isGrounded;    //Si le player touche le sol
    private int jumpsLeft;      //Le nombre de sauts restants
    private int jumpsMax = 1;   //Le nombre max de sauts

    private DistanceJoint2D distJoint = default;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        distJoint = GetComponent<DistanceJoint2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Si le timer de wall jump n'est pas en cours, le joueur peut se déplacer
        if (wallJumpCounter <= 0)
        {
            if (distJoint.enabled)
            {
                resetVelocity();
                if (GetComponent<Grapple>().glideDirection)
                {
                    SetHorizontalVelocity(-GetComponent<Grapple>().glideSpeed);
                }
                else
                {
                    SetHorizontalVelocity(10f);
                }
            }
            else if (!isOnWall)
            {
                //Déplacement
                Move();
            }

            //Check si l'avatar touche le sol
            isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, .2f, whatIsGround);
            if (isGrounded && jumpsLeft < jumpsMax)
            {
                jumpsLeft = jumpsMax;
            }

            //Check si l'avatar touche un mur
            isOnWall = Physics2D.OverlapCircle(wallCheckPoint.position, 0.2f, whatIsWall);

            //Si l'avatar est sur un mur et pas au sol on réduit sa vélocité verticale pour le faire glisser le long du mur
            if (isOnWall && !isGrounded && !distJoint.enabled)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            }

            //Quand on appuie sur la touche de saut
            if (Input.GetButtonDown("Jump"))
            {
                //Si l'avatar est au sol ou qu'il lui reste son deuxième saut il saute
                if ((isGrounded || jumpsLeft > 0))
                {
                    Jump();
                }
                //Sinon si il est sur un mur et ne touche pas le sol on lance le timer du wall jump et on effectue le wall jump
                else if (isOnWall && !isGrounded)
                {
                    wallJumpCounter = wallJumpTime;
                    WallJump();
                }
            }
        }
        //Tant que le timer de wall jump est en cours, il décrémente et bloque les inputs
        else
        {
            wallJumpCounter -= Time.deltaTime;
        }
    }

    private void Move()
    {
        //Récupération de l'input
        movementInput = Input.GetAxisRaw("Horizontal");

        //Application du mouvement
        SetHorizontalVelocity(movementInput * speed);
        //rb.position += new Vector2(movementInput * speed * Time.deltaTime, 0);

        //Si l'avatar va vers la gauche, on flip l'avatar horizontalement
        if (rb.velocity.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f);
        }
        else if (rb.velocity.x > 0)
        {
            transform.localScale = new Vector3(1f, 1f);
        }
    }

    private void SetHorizontalVelocity(float input)
    {
        rb.velocity = new Vector2(input, rb.velocity.y);
    }

    public void Jump()
    {
        resetVelocity();   //Avant de sauter on reset la vélocité de l'avatar
        rb.AddForce(Vector2.up * jumpForce);   //Application de la force
        jumpsLeft--;   //Double saut
    }

    private void WallJump()
    {
        resetVelocity();
        rb.velocity = new Vector2(xWallJumpForce * -movementInput, yWallJumpForce);
    }

    private void resetVelocity()
    {
        rb.velocity = new Vector2(0, 0);
    }

    //Debug
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(groundCheckPoint.position, .2f);
        Gizmos.DrawSphere(wallCheckPoint.position, .2f);
    }

    public int GetJumpsLeft()
    {
        return jumpsLeft;
    }

    public bool GetGrounded()
    {
        return isGrounded;
    }

}
