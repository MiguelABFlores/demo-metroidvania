using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabPatroller : MonoBehaviour
{

    public Transform[] patrolPoints;
    private int currentPoint;

    public float moveSpeed, waitAtPoints;
    private float waitCounter;

    private bool canJumpCooldown = true;
    private float lastTimeJumped = 0.0f;
    public float jumpForce;
    public Transform groundPoint;
    private bool isOnGround;
    public LayerMask whatIsGround;

    public Rigidbody2D theRB;
    public Animator anim;

    void Start()
    {
        waitCounter = waitAtPoints;

        foreach (Transform pPoint in patrolPoints)
        {
            pPoint.SetParent(null);
        }
    }

    void Update()
    {
        if (lastTimeJumped >= 1.0f)
        {
            canJumpCooldown = true;
        }
        else
        {
            canJumpCooldown = false;
        }
        if (Mathf.Abs(transform.position.x - patrolPoints[currentPoint].position.x) > 0.2f)
        {
            if (transform.position.x < patrolPoints[currentPoint].position.x)
            {
                theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);
                transform.localScale = Vector3.one;
            }
            if (transform.position.y < patrolPoints[currentPoint].position.y - 0.5f && theRB.velocity.y < 0.1f && isOnGround == true && canJumpCooldown == true)
            {
                theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);
                lastTimeJumped = 0.0f;
            }
        }
        else
        {
            theRB.velocity = new Vector2(0f, theRB.velocity.y);

            waitCounter -= Time.deltaTime;
            if (waitCounter <= 0)
            {
                waitCounter = waitAtPoints;

                currentPoint++;

                if (currentPoint >= patrolPoints.Length)
                {
                    currentPoint = 0;
                }
            }
        }
        // Crab on Ground
        isOnGround = Physics2D.OverlapCircle(groundPoint.position, .2f, whatIsGround);
        lastTimeJumped += Time.deltaTime;
        anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x));
    }
}
