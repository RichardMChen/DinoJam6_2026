using System;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public CharacterBase charBaseRef;
    public Rigidbody2D rb;
    public Animator animator;

    [Header("Movement")]
    public float movementSpeed = 5f;

    public bool canMove = true;
    public enum movementDirectionEnum { Left, Right };
    private movementDirectionEnum _movementDirection;
    private Vector2 movementDirectionVector = Vector2.left;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.5f, 0.5f);
    public LayerMask groundLayer;
    public bool isGrounded = false;

    [Header("Sound")]
    [SerializeField] AudioSource audioSourceRef;
    [SerializeField] AudioClip footStepSound;

    public movementDirectionEnum movementDirection
    {
        get { return _movementDirection; }
        set {
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
            _movementDirection = value; }
    }

    private void Awake()
    {
        charBaseRef = GetComponent<CharacterBase>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.canFight || GameManager.instance.isGamePaused)
        {
            audioSourceRef.mute = true;
        }
        //if (!charBaseRef.isDead && canMove)
        //{
        //    rb.linearVelocity = new Vector2(movementSpeed * movementDirectionVector.x, rb.linearVelocity.y);
        //}
        //else
        //{
        //    rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        //}
        animator.SetFloat("magnitude", rb.linearVelocity.magnitude);
        GroundCheck();
    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            isGrounded = true;
            animator.SetBool("isGrounded", true);
        }
        else
        {
            isGrounded = false;
            animator.SetBool("isGrounded", false);
        }
    }

    [SerializeField] float horizontalPushForce = 5f;
    [SerializeField] float verticalPushForce = 5f;

    private void OnCollisionEnter2D(Collision2D other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        //{
        //    FlipDirection();
        //}

        //Slight push away of if another character lands on top of this character.
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            //Check if other character is landing from above
            if (other.contacts[0].normal.y < -0.5f)
            {
                //Push the object colliding sideways or knock them back
                float pushDirection = transform.position.x > other.transform.position.x ? 1f : -1f;
                Rigidbody2D otherRigidBody = other.gameObject.GetComponent<Rigidbody2D>();
                otherRigidBody.AddForce(new Vector2(pushDirection * horizontalPushForce, verticalPushForce), ForceMode2D.Impulse);
            }
        }
    }

    private void FlipDirection()
    {
        if (movementDirection == movementDirectionEnum.Left)
        {
            movementDirection = movementDirectionEnum.Right;
        }
        else if (movementDirection == movementDirectionEnum.Right)
        {
            movementDirection = movementDirectionEnum.Left;
        }
        else
        {
            Debug.LogError("Movement direction was not set.");
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
            audioSourceRef.PlayOneShot(footStepSound);
        }
    }
}
