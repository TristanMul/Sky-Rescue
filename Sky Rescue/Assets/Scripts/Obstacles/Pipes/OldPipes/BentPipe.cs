using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BentPipe : PipeLerp
{
    [SerializeField] private Transform middlePoint;
    // Start is called before the first frame update
    void Start()
    {
        start = middlePoint;
        end = next;
        totalDistance = Vector3.Distance(start.position, end.position);
    }
    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }
}
