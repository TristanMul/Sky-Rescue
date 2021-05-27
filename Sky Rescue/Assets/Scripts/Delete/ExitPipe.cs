using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;


public class ExitPipe : MonoBehaviour
{
    private ArcCalculator arcCalculator;
    [SerializeField] private Transform passTarget;
    [SerializeField] private Transform passHeight;
    private void Start()
    {
        arcCalculator = new ArcCalculator();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            other.GetComponent<Rigidbody>().useGravity = true;
            other.GetComponent<PathFollower>().enabled = false;
            other.GetComponent<PathFollower>().pathCreator = null;
            other.GetComponent<Rigidbody>().velocity = arcCalculator.CalculateArc(other.transform, passTarget, passHeight.position.y);
        }
    }
}
