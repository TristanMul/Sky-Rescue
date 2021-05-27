using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation.Examples;

public class EnterPipe : MonoBehaviour
{
    [SerializeField] private PathCreation.PathCreator LinkedPath;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Ball"))
        {
            other.GetComponent<PathFollower>().pathCreator = LinkedPath;
            other.GetComponent<Rigidbody>().useGravity = false;
            other.GetComponent<PathFollower>().enabled = true;
        }
    }
}
