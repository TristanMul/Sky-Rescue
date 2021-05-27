using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateParticleEffect : MonoBehaviour
{
    [SerializeField] private ParticleSystem particleSystem;
    [SerializeField] private GameObject GameObject;
    private bool hasTriggered = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") && !hasTriggered)
        {
            hasTriggered = true;
            particleSystem.Play();
            LevelManager.instance.AddCoins(1);
            GameObject.SetActive(false);
        }
    }
}
