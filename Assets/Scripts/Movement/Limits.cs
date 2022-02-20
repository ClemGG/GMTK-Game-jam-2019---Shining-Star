using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limits : MonoBehaviour
{
    public Transform[] limits, borders;


    public static Limits instance;

    private void Awake()
    {
        if (instance)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    public bool IsOffBordersX(Ballerine b)
    {
        return (b.transform.position.x > borders[0].position.x && Mathf.Sign(b.moveDir.x) == 1 || b.transform.position.x < borders[1].position.x && Mathf.Sign(b.moveDir.x) == -1) ? true : false;
    }

    public bool IsOffBordersY(Ballerine b)
    {
        return (b.transform.position.y > borders[2].position.y && Mathf.Sign(b.moveDir.y) == 1 || b.transform.position.y < borders[3].position.y && Mathf.Sign(b.moveDir.y) == -1) ? true : false;
    }

    public bool IsOffLimitsX(Ballerine b)
    {
        return (b.transform.position.x > limits[0].position.x|| b.transform.position.x < limits[1].position.x) ? true : false;
    }

    public bool IsOffLimitsY(Ballerine b)
    {
        return (b.transform.position.y > limits[2].position.y || b.transform.position.y < limits[3].position.y) ? true : false;
    }


#if UNITY_EDITOR

    private void OnDrawGizmos()
    {
        if (limits != null)
        {

            for (int i = 0; i < limits.Length; i++)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(limits[i].position, .2f);
                UnityEditor.Handles.Label(limits[i].position - new Vector3(0f, .2f, 0f), i.ToString());
            }
        }

        if (borders != null)
        {

            for (int i = 0; i < borders.Length; i++)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(borders[i].position, .2f);
                UnityEditor.Handles.Label(borders[i].position - new Vector3(0f, .2f, 0f), i.ToString());
            }
        }
    }

#endif
}
