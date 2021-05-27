using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeLerp : MonoBehaviour
{
    [HideInInspector] public bool isLerping = false;
    [SerializeField] private float speed;
    [HideInInspector] public Transform start, end;
    public Transform next;
    [HideInInspector] public float currentLerpDistance = 0, lerpPercentage;
    [HideInInspector] public float totalDistance;


    public IEnumerator lerpBall(GameObject ball)
    {
       while(lerpPercentage < 1)
        {
            currentLerpDistance += Time.fixedDeltaTime * speed;
            lerpPercentage = currentLerpDistance / totalDistance;
            ball.gameObject.transform.position = Vector3.Lerp(start.position, end.position, lerpPercentage);
            yield return null;
        }
    }
    public virtual void OnTriggerEnter(Collider other)
    {
        StartCoroutine(lerpBall(other.gameObject));
        Debug.Log(totalDistance);
    }
}
