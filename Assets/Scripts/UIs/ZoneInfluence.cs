using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ZoneInfluence : MonoBehaviour
{
    private BoxCollider2D col;
    private Transform t;

    public LayerMask collisionMask;
    
    // Start is called before the first frame update
    void Start()
    {
        t = transform;

        col = GetComponent<BoxCollider2D>();
        col.isTrigger = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Collider2D[] cols = Physics2D.OverlapBoxAll(t.position, col.size, 0f, collisionMask);
        ScoreManager.instance.shouldIncreaseValue = cols.Length == 0 ? 1f : -1f;
    }
}
