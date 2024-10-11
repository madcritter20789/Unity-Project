using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{
    public Node previousWaypoint;
    public Node nextWaypoint;


    public Vector3 getPosition()
    {
        Vector3 minBound = transform.position + transform.right*5;
        Vector3 maxBound = transform.position + transform.right*5;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0, 1));
    }
}
