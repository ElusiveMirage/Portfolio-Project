using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PL_EnemyBase : MonoBehaviour, IDamageable
{
    [SerializeField] private float HP;
    [SerializeField] private float attack;
    [SerializeField] private float moveSpeed;
    //===============================================//
    [SerializeField] private bool facingRight;
    [SerializeField] private bool grounded = false;
    [SerializeField] private bool moveRight = false;
    [SerializeField] private bool moveLeft = false;
    //===============================================//
    [SerializeField] private LayerMask collisionLayerMask;
    [SerializeField] private LayerMask attackLayerMask;
    //===============================================//
    private Rigidbody2D myRigidbody2D;
    private Animator myAnimator;
    //===============================================//
    private Vector2 moveDistance = new Vector2(0, 0);
    private Vector3 facingDir = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

    }

    private void HorizontalMovement()
    {

    }

    public void InflictDamage(float damageToInflict)
    {
        GameObject damageNumber = Instantiate(Resources.Load<GameObject>("DamageNumber"), transform.position, Quaternion.identity);
        damageNumber.GetComponent<FloatingText>().textString = "- " + damageToInflict.ToString();
    }


}
