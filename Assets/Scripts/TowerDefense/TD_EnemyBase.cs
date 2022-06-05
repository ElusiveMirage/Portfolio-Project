using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD_EnemyBase : MonoBehaviour, IDamageable
{
    [SerializeField] private float HP;
    [SerializeField] private float maxHP;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float damage;
    [SerializeField] private int scoreValue;
    //================================================//
    [SerializeField] private int currentNode;
    [SerializeField] private int currentWaypoint;
    [SerializeField] private GameObject deathEffect;
    [SerializeField] private UI_Bar HPBar;
    [SerializeField] List<Transform> waypointRoute = new List<Transform>();
    [SerializeField] List<Node2D> aStarPath = new List<Node2D>();
    //================================================//
    public bool penetrated;
    private bool isAlive;
    private float originalSpeed;
    Rigidbody2D myRigidbody;
    Animator myAnimator2D;
    
    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator2D = GetComponent<Animator>();
        HP = maxHP;
        currentNode = 0;
        currentWaypoint = 0;
        originalSpeed = moveSpeed;
        isAlive = true;
        penetrated = false;

        if (waypointRoute.Count > 0)
        {
            GetAstarPath(waypointRoute[currentWaypoint]);
        }       
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive && !penetrated)
        {
            HPBar.SetMaxValue(maxHP);
            HPBar.SetMinValue(HP);

            if (HP > 0)
            {
                if (waypointRoute.Count > 0 && currentWaypoint < waypointRoute.Count)
                {
                    if (aStarPath.Count > 0 && currentNode < aStarPath.Count)
                    {
                        GridMovement();
                    }
                }
            }
            else
            {
                Death();
                Instantiate(deathEffect, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
                Destroy(gameObject, 1f);
                isAlive = false;
            }
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Lerp(GetComponent<SpriteRenderer>().color.a, 0f, Time.deltaTime));
        }
    }

    private void GetAstarPath(Transform targetTransform)
    {
        if (GetComponent<AStarPathfinding>())
        {
            aStarPath = GetComponent<AStarPathfinding>().CalculatePath(transform.position, targetTransform.transform.position);
        }
    }

    private void GridMovement()
    {
        if(currentNode < aStarPath.Count)
        {
            if (transform.position != aStarPath[currentNode].worldPos && !aStarPath[currentNode].unitPlaced)
            {
                transform.position = Vector3.MoveTowards(transform.position, aStarPath[currentNode].worldPos, moveSpeed * Time.deltaTime);
            }
            else
            {
                currentNode++;
            }
        }

        if(currentNode == aStarPath.Count)
        {
            currentWaypoint++;

            if (currentWaypoint < waypointRoute.Count)
            {             
                GetAstarPath(waypointRoute[currentWaypoint]);
                currentNode = 0;
            }
        }
    }

    public void Initialize(List<Transform> route, NodeGrid2D grid)
    {
        waypointRoute = route;
        GetComponent<AStarPathfinding>().nodeGrid = grid;
    }

    public void InflictDamage(float damageToInflict)
    {
        HP -= damageToInflict;
    }

    public void ModifyMoveSpeed(float modifier)
    {
        moveSpeed = originalSpeed * modifier;
    }

    public void RestoreMoveSpeed()
    {
        moveSpeed = originalSpeed;
    }

    private void Death()
    {       
        TD_GameManager.Instance.stageScore += scoreValue;
        TD_GameManager.Instance.enemyCount--;
    }
}
