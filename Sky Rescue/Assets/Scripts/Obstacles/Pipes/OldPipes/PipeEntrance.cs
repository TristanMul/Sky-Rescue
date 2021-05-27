using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeEntrance : PipeLerp
{
    [SerializeField] Transform next;
    void Start()
    {
        start = transform.Find("StartPoint");
        end = next;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            isLerping = true;
            other.gameObject.GetComponent<Rigidbody>().useGravity = false;
            StartCoroutine(lerpBall(other.gameObject));
        }
    }
}
