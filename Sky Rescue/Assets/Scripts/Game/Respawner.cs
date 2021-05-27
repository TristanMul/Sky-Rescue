using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawner : MonoBehaviour
{
    public GameObject player;
    Vector3 respawnPoint;

    // Start is called before the first frame update
    void Start()
    {
        respawnPoint = player.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        other.transform.position = respawnPoint;
        player.transform.position = respawnPoint;
    }
}
