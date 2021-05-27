using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private GameEvent gameResult;
    ParticleSystem confetti;
    // Start is called before the first frame update
    void Start()
    {
        confetti = GetComponent<ParticleSystem>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            confetti.Play();
            gameResult.Raise();
        }
    }
}
