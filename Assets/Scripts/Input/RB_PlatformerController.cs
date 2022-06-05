using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RB_PlatformerController : MonoBehaviour, IDamageable
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private bool dashing = false;
    [SerializeField] private bool jumping = false;
    [SerializeField] private bool isInvuln = false;
    [SerializeField] private LayerMask dashLayerMask;
    //===============================================//
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCollider2D;
    //===============================================//
    private Vector3 xDir = new Vector3(1, 0, 0);
    //===============================================//
    bool facingRight = true;
    float dashDistance;
    float distanceMoved;

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myCollider2D = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Dash();
    }

    public void Movement(InputAction.CallbackContext context)
    {
        if(!dashing)
        {
            Vector2 inputVector = context.ReadValue<Vector2>();

            Vector2 playerVelocity = new Vector2(moveSpeed, myRigidbody.velocity.y) * inputVector;

            myRigidbody.velocity = playerVelocity;
            
            if(inputVector.x == 1)
            {
                facingRight = true;
                FlipSprite();
            }
            else if(inputVector.x == -1)
            {
                facingRight = false;
                FlipSprite();
            }
        }     
    }

    public void StartJump(InputAction.CallbackContext context)
    {
        if (context.performed && !jumping)
        {
            jumping = true;
            myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, myRigidbody.velocity.y + jumpSpeed);
        }
    }

    public void StartDash(InputAction.CallbackContext context)
    {
        if(context.performed && !dashing)
        {
            dashing = true;
            myRigidbody.velocity = Vector2.zero;
        }
    }

    private void Dash() //refactor into coroutine?
    {
        if(dashing)
        {
            isInvuln = true;
            myAnimator.SetBool("dashing", true);
            Vector2 dashVelocity = new Vector2(xDir.x * moveSpeed * 2, 0f);
            RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, xDir, dashDistance, dashLayerMask);

            if (raycastHit2D.collider != null)
            {
                if (Vector2.Distance(transform.position, raycastHit2D.point) <= 1)
                {
                    distanceMoved = 0f;
                    myRigidbody.velocity = Vector2.zero;
                    myAnimator.SetBool("dashing", false);
                    dashing = false;
                    
                }
            }
            myRigidbody.velocity = dashVelocity;

            if (dashVelocity.x < 0)
            {
                distanceMoved -= dashVelocity.x;
            }
            else
            {
                distanceMoved += dashVelocity.x;
            }
            
            if (distanceMoved >= dashDistance)
            {
                distanceMoved = 0f;
                myRigidbody.velocity = Vector2.zero;
                myAnimator.SetBool("dashing", false);
                dashing = false;
            }
        }
    }

    private void FlipSprite()
    {
        if(facingRight)
        {
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0f);
            xDir = new Vector3(1, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0.0f, -180.0f, 0f);
            xDir = new Vector3(-1, 0, 0);
        }

    }

    public void InflictDamage(float damageToInflict)
    {
        if(!isInvuln)
        {

        }
    }
}
