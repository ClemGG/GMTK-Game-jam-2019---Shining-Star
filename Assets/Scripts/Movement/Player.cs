using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Ballerine
{

    [Space(10)]
    [Header("Player : ")]
    [Space(10)]
    
    public bool isAtking;


    [Space(10)]
    [Header("Line : ")]
    [Space(10)]

    [SerializeField] private LineRenderer lr;
    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    protected override void Start()
    {
        base.Start();
        InitializeLine();

    }

    private void InitializeLine()
    {
        startPoint.position = t.position;

        endPoint.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endPoint.position = new Vector3(endPoint.position.x, endPoint.position.y, 0f);

        startPoint.gameObject.SetActive(false);
        endPoint.gameObject.SetActive(false);

        lr.SetPositions(new Vector3[] { startPoint.position, startPoint.position });
        lr.enabled = false;
    }

    private void OnMouseDown()
    {
        if (isAtking || ScoreManager.instance.jeuTerminé)
            return;

        startPoint.position = t.position;

        endPoint.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endPoint.position = new Vector3(endPoint.position.x, endPoint.position.y, 0f);

        startPoint.gameObject.SetActive(true);
        endPoint.gameObject.SetActive(true);
        lr.enabled = true;
    }

    private void OnMouseUp()
    {
        if (isAtking || !lr.enabled || ScoreManager.instance.jeuTerminé)
            return;

        InitializeLine();
        CalculateMovement();
    }

    private void CalculateMovement()
    {
        isAtking = true;
        moveCurveTimer = 0f;
        moveDir = (startPoint.position - endPoint.position).normalized;
    }

    private void OnMouseDrag()
    {
        if (isAtking || ScoreManager.instance.jeuTerminé)
            return;

        startPoint.position = t.position;
        endPoint.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        endPoint.position = new Vector3(endPoint.position.x, endPoint.position.y, 0f);
        lr.SetPositions(new Vector3[] { endPoint.position, startPoint.position });
    }


    // Update is called once per frame
    protected override void Update()
    {
        if (isAtking && !ScoreManager.instance.jeuTerminé)
            base.Update();

        if (moveCurveTimer > .8f)
        {
            isAtking = false;
        }
    }



    protected override void OnCollisionEnter2D(Collision2D c)
    {
        if(isAtking && !ScoreManager.instance.jeuTerminé)
            base.OnCollisionEnter2D(c);
    }
}
