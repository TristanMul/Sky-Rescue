using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHandling : MonoBehaviour
{
    public Transform ballHolder;
    [HideInInspector] public Vector3 spawnPosition;

    Collider ballTrigger;

    Ball currentBall;
    [HideInInspector] public bool isCaught = false;
    public GameObject animationBall;
    [HideInInspector] public Animator ballAnimator;
    Renderer animationBallRenderer;
    Animator animator;
    // Start is called before the first frame update
    void Awake()
    {
        if (this.GetComponent<Movement>().ActiveOnStart)
        {
            isCaught = true;
        }
        spawnPosition = transform.position;

        ballTrigger = GetComponent<Collider>();
        currentBall = ballHolder.GetComponentInChildren<Ball>();

        ballAnimator = animationBall.GetComponent<Animator>();
        ballAnimator.SetBool("Dribbling", false);
    }


    public void Setup(bool activeOnStart)
    {

        animationBallRenderer = animationBall.GetComponent<Renderer>();

        animationBallRenderer.enabled = activeOnStart;

        animator = GetComponent<Animator>();
        animator.SetBool("HasBall", activeOnStart);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            isCaught = true;
            currentBall = other.GetComponent<Ball>();
            other.GetComponent<Ball>().GetCaught(ballHolder);
            CatchBall();
        }
    }

    public void CatchBall()
    {
        MakeBallVisible();
        animator.SetBool("HasBall", true);
        ballAnimator.SetBool("Dribbling", true);
    }

    public void ThrowBall()
    {
        MakeBallInvisible();
        currentBall = null;
        animator.SetBool("HasBall", false);
    }

    public void MakeBallInvisible()
    {
        animationBallRenderer.enabled = false;
    }
    public void MakeBallVisible()
    {
        if (animationBallRenderer)
        {
            animationBallRenderer.enabled = true;
        }
    }
}
