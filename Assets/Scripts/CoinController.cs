using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    public float bobSpeed;
    public float rotateSpeed;
    public float bobHeight;

    private Vector3 startPos;
    private Vector3 targetPos;

    void Awake ()
    {
        startPos = transform.position;
        targetPos = startPos + new Vector3(0, bobHeight, 0);
    }

    void Update ()
    {
        // move towards the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPos, bobSpeed * Time.deltaTime);

        // rotate over time
        transform.Rotate(Vector3.up, rotateSpeed * Time.deltaTime, Space.World);

        // are we at the target position
        if(transform.position == targetPos)
        {
            if(targetPos == startPos)
                targetPos = startPos + new Vector3(0, bobHeight, 0);
            else if(targetPos == startPos + new Vector3(0, bobHeight, 0))
                targetPos = startPos;
        }
    }
}