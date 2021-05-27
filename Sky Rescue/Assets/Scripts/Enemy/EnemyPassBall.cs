using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPassBall : MonoBehaviour
{
    [SerializeField] private float basePassDelay;
    [SerializeField] private float scorePercentage;
    [SerializeField] private FloatVariable passDelay;

    private GameObject ball;
    private float passHeight;
    private Transform passTarget;
    private bool canPass = false;
    private ArcCalculator arcCalculator;
    private BallHandling ballHandling;

    private void Start() {
        arcCalculator = new ArcCalculator();
        ballHandling = GetComponent<BallHandling>();
        passDelay.Value = basePassDelay;
    }

    public void PassBall(){
        GetBall();

        if(!ball || !ball.GetComponent<Rigidbody>() || !canPass) return;

        ball.GetComponent<Renderer>().enabled = true;
        ball.GetComponent<Rigidbody>().isKinematic = false;
        for (int i = 0; i < ball.GetComponents<Collider>().Length; i++)
        {
            ball.GetComponents<Collider>()[i].enabled = true;
        }
        ball.GetComponent<Ball>().Trail.SetActive(true);
        ballHandling.ThrowBall();
        ball.GetComponent<Rigidbody>().velocity = arcCalculator.CalculateArc(ball.transform, passTarget, passHeight);
        canPass = false;
    }

    private void GetBall(){
        if(GetComponentInChildren<Ball>())
            ball = GetComponentInChildren<Ball>().gameObject;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Obstacle") && other.GetComponentInChildren<Obstacle>()){
            GetComponent<Collider>().enabled = false;
            passTarget = other.GetComponent<Obstacle>().PassTargetTransform;
            passHeight = other.GetComponent<Obstacle>().PassHeightTransform.position.y;
            canPass = true;
            Invoke("PassBall", passDelay.Value);
            passDelay.Value = basePassDelay;
        }

        if(other.CompareTag("Finish") && other.GetComponentInChildren<Finish>()){
            GetComponent<Collider>().enabled = false;

            float randomValue = Random.value;
            if(randomValue <= scorePercentage){
                passTarget = other.GetComponent<Finish>().ScoreTargetTransform;
            }
            else{
                passTarget = other.GetComponent<Finish>().MissTargetTransform;
            }
            passHeight = other.GetComponent<Finish>().PassHeightTransform.position.y;

            canPass = true;
            Invoke("PassBall", passDelay.Value);
            passDelay.Value = basePassDelay;
        }
    }
}
