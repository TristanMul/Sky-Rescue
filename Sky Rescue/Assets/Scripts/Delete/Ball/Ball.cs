using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] bool ballOfPlayer;
    public static Transform playerBall;
    public static Ball enemyBall;
    public ParticleSystem ballExplode;
    public ParticleSystem respawnTrail;
    public GameObject Trail;
    [SerializeField] private FloatVariable enemyPassDelay;
    [SerializeField] private float addedEnemyPassDelay;
    Transform target;
    float flySpeed = 8f;
    Rigidbody rb;
    Transform ballParent;
    Vector3 parentPos;
    Ball3DController ballController;
    BallHandling ballHandler;
    Movement playerMove;
    GameObject previousParent;


    Renderer renderer;

    int amountOfBounces;

    [SerializeField] float boostedTimeScale;

    // Start is called before the first frame update
    void Start()
    {
        previousParent = transform.parent.parent.gameObject;
        playerMove = GetComponentInParent<Movement>();
        target = GetComponentInParent<Transform>();
        if (ballOfPlayer)
        {
            playerBall = transform;
            ballController = GetComponent<Ball3DController>();
            ballController.getThrown += GetThrown;
        }
        else
        {
            enemyBall = this;
        }
        ballHandler = GetComponentInParent<BallHandling>();
        rb = GetComponent<Rigidbody>();
        ballParent = transform.root;
        parentPos = ballHandler.spawnPosition;
        renderer = GetComponentInChildren<Renderer>();
        renderer.enabled = false;

        for (int i = 0; i < GetComponents<Collider>().Length; i++)
        {
            GetComponents<Collider>()[i].enabled = false;
        }
    }

    public void GetThrown()
    {
        PlayerInput input = GetComponentInParent<PlayerInput>();
        if (input)
        {
            input.ThrowBall();
        }
        transform.parent = null;
        renderer.enabled = true;
        Trail.SetActive(true);
    }

    public void GetCaught(Transform byWhom)
    {
        if (!rb) { return; }
        transform.parent = byWhom;
        transform.position = byWhom.position;
        //StartCoroutine(MakeInvisible.instance.deactivatePlayer(/*MakeInvisible.instance.PlayersActivated*/));

            rb.isKinematic = true;

        for (int i = 0; i < GetComponents<Collider>().Length; i++)
        {
            GetComponents<Collider>()[i].enabled = false;
        }
        playerMove = byWhom.transform.parent.GetComponent<Movement>();
        PlayerInput input = GetComponentInParent<PlayerInput>();
        if (input)
        {
            input.CatchBall();
        }
        if (byWhom.transform.parent.CompareTag("Enemy"))
        {
            byWhom.transform.parent.GetComponent<Movement>().StartMovement();
            byWhom.transform.parent.GetComponent<Collider>().enabled = true;
        }
        if (ballOfPlayer)
        {
            ballController.GetCaught();
        }
        if (renderer)
        {
            renderer.enabled = false;
        }
        ballParent = byWhom.root;
        parentPos = byWhom.root.position;
        Time.timeScale = 1f;
        amountOfBounces = 0;
        Trail.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("RespawnZone"))
        {
            if(playerMove.AtFinish){
                StartCoroutine(ResetBall(playerMove.FinishPos));
            }
            else{
                StartCoroutine(ResetBall(parentPos));
            }
        }
    }

    private IEnumerator ResetBall(Vector3 resetPos)
    {
        ParticleSystem explodeParticles = Instantiate(ballExplode, transform.position, Quaternion.identity);
        renderer.enabled = false;
        for (int i = 0; i < GetComponents<Collider>().Length; i++)
        {
            GetComponents<Collider>()[i].enabled = false;
        }
        rb.velocity = new Vector3(0, 0, 0);
        if (ballOfPlayer)
        {
            enemyPassDelay.Value += addedEnemyPassDelay;
        }
        yield return new WaitForSeconds(0.5f);
        renderer.enabled = true;
        ballParent.root.position = resetPos;
        transform.position = resetPos;
        GetCaught(ballParent.transform.Find("BallHolder"));
        ballParent.GetComponent<BallHandling>().CatchBall();
        ballParent.GetComponent<Movement>().StopMovement();
        yield return new WaitForSeconds(0.5f);
        ParticleSystem respawnParticles = Instantiate(respawnTrail, transform.position + Vector3.back, Quaternion.Inverse(ballParent.rotation));
        respawnParticles.transform.parent = ballParent;
        if(!playerMove.AtFinish){
            ballParent.GetComponent<Movement>().StartMovement();
        }
        yield return new WaitForSeconds(3f);
        Destroy(respawnParticles);
        Destroy(explodeParticles);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            amountOfBounces++;
            if (amountOfBounces > 1 && boostedTimeScale > .1f)
            {
                Time.timeScale = boostedTimeScale;
                Debug.Log("Boost");
            }
        }
    }
}
