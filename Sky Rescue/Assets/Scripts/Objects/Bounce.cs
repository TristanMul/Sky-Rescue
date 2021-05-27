using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounce : MonoBehaviour
{
    [SerializeField] private Transform passTarget;
    [SerializeField] private Transform passHeight;
    private ArcCalculator arcCalculator;
    private void Start()
    {
        arcCalculator = new ArcCalculator();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            other.GetComponent<Rigidbody>().velocity = arcCalculator.CalculateArc(other.transform, passTarget, passHeight.position.y);
        }
    }
}
