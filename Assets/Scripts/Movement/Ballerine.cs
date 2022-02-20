using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballerine : MonoBehaviour
{
    [Space(10)]
    [Header("Scripts & Components : ")]
    [Space(10)]

    protected Transform t, sprite;
    protected Rigidbody2D rb;
    protected Collider2D col;

    [Space(10)]
    [Header("Inputs : ")]
    [Space(10)]

    [SerializeField] protected float moveSpeed = 2f, rotSpeed = 120f;
    [SerializeField] protected bool isDead = false;

    public Vector2 moveDir;
    [SerializeField] protected AnimationCurve moveCurve;
    protected float moveCurveTimer;

    public Vector2 SetMoveDir {set => moveDir = value; }


    // Start is called before the first frame update
    protected virtual void Start()
    {
        t = transform;
        sprite = t.GetChild(0);

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    protected virtual void Update()
    {
        if (!isDead)
        {
            if (Limits.instance.IsOffBordersY(this))
            {
                moveDir.y *= -1f;
            }

            else if (Limits.instance.IsOffBordersX(this))
            {
                moveDir.x *= -1f;
            }
        }
    }


    // Update is called once per frame
    protected void FixedUpdate()
    {
        if (!isDead && !ScoreManager.instance.jeuTerminé)
        {
            MoveBallerine();
            RotateBallerine();
        }
    }

    protected void MoveBallerine()
    {
        moveCurveTimer += Time.deltaTime;
        t.Translate(moveDir * Time.deltaTime * moveSpeed * moveCurve.Evaluate(moveCurveTimer));
    }

    protected void RotateBallerine()
    {
        sprite.Rotate(new Vector3(0f, 0f, Time.deltaTime * rotSpeed));
    }

    protected virtual void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.CompareTag("MurX"))
        {
            moveDir.x *= -1f;
        }
        else if (c.gameObject.CompareTag("MurY"))
        {
            moveDir.y *= -1f;
        }
        else if (c.gameObject.CompareTag("Ballerine"))
        {
            ChangeDirection(c.transform.position);
        }

    }
    


    protected void ChangeDirection(Vector2 relativePosition)
    {
        Bounds bounds = GetComponent<Collider2D>().bounds;
        //if the right side of the collider is hit
        if (relativePosition.x > 0f && relativePosition.y <= bounds.max.y && relativePosition.y >= bounds.min.y)
        {
            //print ("Collided with the right side");
            moveDir.x *= -1f;
        }
        //if the left side of the collider is hit
        else if (relativePosition.x < 0 && relativePosition.y <= bounds.max.y && relativePosition.y >= bounds.min.y)
        {
            //print ("Collided with the left side");
            moveDir.x *= -1f;
        }

        //if the upper side of the collider is hit
        else if (relativePosition.y > 0 && relativePosition.x <= bounds.max.x && relativePosition.x >= bounds.min.x)
        {
            //print ("Collided with the upper side");
            moveDir.y *= -1f;
        }
        //if the lower side of the collider is hit
        else if (relativePosition.y < 0 && relativePosition.x <= bounds.max.x && relativePosition.x >= bounds.min.x)
        {
            //print ("Collided with the lower side");
            moveDir.y *= -1f;
        }
        else
        {
            moveDir.x *= -1f;
        }


    }

}
