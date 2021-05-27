using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Movement movement;
    [SerializeField] GameEvent onGameStart;
    bool active;
    bool gameStarted = false;
    CameraMovement camMove;
    BallHandling ballHandling;


    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<Movement>();
        ballHandling = GetComponent<BallHandling>();
        active = movement.ActiveOnStart;
    }

    private void Update()
    {
        if (active)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && movement.ActiveOnStart)
            {
                GameStarted();
                movement.StartMovement();
                CameraMovement.instance.started = true;
            }
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ThrowBall();
            }
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            movement.StopMovement();
        }

    }
    public void Activate() { }

    public void ThrowBall()
    {
        active = false;
        movement.StopMovement();
        GetComponent<Collider>().enabled = false;
        ballHandling.ThrowBall();
        CameraMovement.instance.SwitchTarget(Ball.playerBall);
        this.enabled = false;
    }

    public void CatchBall()
    {
        active = true;
        if (!movement.ActiveOnStart)
        {
            movement.StartMovement();
        }
        GetComponent<Collider>().enabled = true;

        CameraMovement.instance.SwitchTarget(transform);
    }

    private void GameStarted(){
        if(gameStarted) return;
        onGameStart.Raise();
    }
}
