using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightPipe : PipeLerp
{
    private void Start()
    {
        start = transform.Find("StartPoint");
        end = next;
        totalDistance = Vector3.Distance(start.position, end.position);
    }
    /* private void OnTriggerExit(Collider other)
     {
         start = transform.Find("StartPoint");
         end = transform.Find("EndPoint");
         currentLerpTime = 0;
     }*/
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
