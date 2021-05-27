using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Movement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool activeOnStart;
    public bool ActiveOnStart { get { return activeOnStart; } }
    bool isMoving;
    public bool IsMoving { get { return isMoving; } }
    Rigidbody rb;
    Animator animator;
    BallHandling ballHandling;

    private bool atFinish = false;
    public bool AtFinish { get{ return atFinish; } }
    private Vector3 finishPos;
    public Vector3 FinishPos { get{ return finishPos; } }

    // Start is called before the first frame update
    void Start()
    {
        //References to other components
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        ballHandling = GetComponent<BallHandling>();
        ballHandling.Setup(activeOnStart);
    }


    /// <summary>
    /// Starts the movement of the character, either called from an AI or player input component
    /// </summary>
    public void StartMovement()
    {
        isMoving = true;
        if (rb)
        {
            rb.velocity = new Vector3(0f, 0f, speed);
        }
        if (animator)
        {
            animator.SetBool("IsWalking", true);
        }
        if (ballHandling)
        {
            ballHandling.ballAnimator.SetBool("Dribbling", true);
        }
        StartCoroutine(TurnAround(5f));
    }

    /// <summary>
    /// Stops the movement of the character, also handles the animation changes
    /// </summary>
    public void StopMovement()
    {
        isMoving = false;
        rb.velocity = Vector3.zero;
        animator.SetBool("IsWalking", false);
        ballHandling.ballAnimator.SetBool("Dribbling", false);
    }

    /// <summary>
    /// Modifies the current movementspeed
    /// </summary>
    /// <param name="value">The amount of speed to be added or removed</param>
    void ChangeSpeed(float value)
    {
        speed += value;
        rb.velocity = new Vector3(0f, 0f, speed);
    }

    /// <summary>
    /// Sets the current movement speed, ignores previous speed
    /// </summary>
    /// <param name="value">The speed the player will be set to</param>
    void SetSpeed(float value)
    {
        speed = value;
        rb.velocity = new Vector3(0f, 0f, speed);
    }

    IEnumerator TurnAround(float speed)
    {
        while (transform.rotation != Quaternion.identity)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, speed * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Obstacle" || other.gameObject.tag == "Finish")
        {
            StopMovement();
        }

        if(other.CompareTag("Finish")){
            atFinish = true;
            finishPos = transform.position;
        }
    }
}
