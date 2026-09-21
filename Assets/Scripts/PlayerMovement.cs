using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Movement")]
    public float movementSpeed = 5f;
    private float horizontalMovement;
    public bool canMove = true;
    public enum movementDirectionEnum { Left, Right };
    private movementDirectionEnum _movementDirection;
    private Vector2 movementDirectionVector = Vector2.right;

    public movementDirectionEnum movementDirection
    {
        get { return _movementDirection; }
        set
        {
            if (_movementDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);

                if (value == movementDirectionEnum.Left)
                {
                    movementDirectionVector = Vector2.left;
                }
                else if (value == movementDirectionEnum.Right)
                {
                    movementDirectionVector = Vector2.right;
                }
            }
            _movementDirection = value;
        }
    }

    [Header("Jumping")]
    public float jumpPower = 10f;
    public int maxJumps = 1; //Current implementation has issues and not counting the first jump as being used so jumps 0 and 1 will count as the 2 jumps for the double jump.
    private int jumpsRemaining;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask groundLayer;
    public bool isGrounded = false;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    [Header("Sound")]
    [SerializeField] AudioSource audioSourceRef;
    [SerializeField] AudioClip footStepSound;
    [SerializeField] AudioClip attackSound;

    //For testing box casting
    //public Vector2 boxSize;
    //public float castDistance;

    void FixedUpdate()
    {
        if (!GameManager.instance.canFight || GameManager.instance.isGamePaused)
        {
            audioSourceRef.mute = true;
            return;
        }

        if (isGrounded)
        {
            rb.linearVelocity = new Vector2(horizontalMovement * movementSpeed, rb.linearVelocity.y);
        }
        else
        {

        }
        //rb.linearVelocity = new Vector2(horizontalMovement * movementSpeed, rb.linearVelocity.y);

        GroundCheck();
        Gravity();
        //animator.SetFloat("yVelocity", rb.linearVelocity.y);
        //animator.SetFloat("magnitude", horizontalMovement);
        animator.SetFloat("magnitude", rb.linearVelocity.magnitude);
        //Debug.Log(jumpsRemaining);

        //Set direction to face the target
        if (transform.position.x < GameManager.instance.player2.gameObject.transform.position.x)
        {
            movementDirection = movementDirectionEnum.Left;
        }
        else
        {
            movementDirection = movementDirectionEnum.Right;
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!GameManager.instance.canFight || GameManager.instance.isGamePaused)
        {
            return;
        }

        if (jumpsRemaining > 0)
        {
            if (context.performed) //Hold down jump button
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                jumpsRemaining--;
                animator.SetTrigger("jump");
            }
            else if (context.canceled && rb.linearVelocity.y > 0) //Light tap of jump button
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpPower);
                jumpsRemaining--;
                animator.SetTrigger("jump");
            }
        }
    }

    public void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }
        else
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }

        ////if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, groundLayer))
        ////{
        ////    jumpsRemaining = maxJumps;
        ////}
    }

    public void Gravity()
    {
        if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier; //Fall increasingly faster
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);

        //Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize); //Draw box cast
    }

    public void PlayFootStepSound()
    {
        if (audioSourceRef && footStepSound)
        {
            //audioSourceRef.clip = footStepSound;
            audioSourceRef.PlayOneShot(footStepSound, 1.0f);
        }
    }

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        isGrounded = true;
    //        jumpsRemaining = maxJumps;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
    //    {
    //        isGrounded = false;
    //    }
    //}
}
