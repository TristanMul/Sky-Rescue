

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ball3DController : MonoBehaviour
{
    public float power;
    //public float sensitivity;

    public int dotsValut;

    //private float interval = 1 / 30.0f;

    private bool aiming = false;
    private bool isShot = false;

    private Vector3 startPos;
    private List<GameObject> dotsObjects = new List<GameObject>();

    public GameObject dotPrefab;
    [SerializeField] GameObject collisionDotPrefab;
    GameObject collisionDot;
    public Transform dotsTransform;

    float maxForce = 600f;
    [SerializeField] float torqueForce = 10;

    public event Action getThrown;

    //Animator anim;
    Vector3 lastForce;
    float lastHorizontal;

    [Header("ThrowValues")]
    [SerializeField] float minHorizontal;
    [SerializeField] float maxHorizontal;
    [SerializeField] float minVertical;
    [SerializeField] float maxVertical;
    [SerializeField] float sensitivity;
    [SerializeField] float initialHorizontal;

    // Use this for initialization
    void Start()
    {
        //anim = GetComponent<Animator>();
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.GetComponent<Collider>().enabled = false;
        startPos = transform.position;

        float tempValue = 1f;

        for (int i = 0; i < dotsValut; i++)
        {
            GameObject dot = Instantiate(dotPrefab);
            dot.transform.parent = dotsTransform;
            if (i > dotsValut)
            {
                dot.transform.localScale = new Vector3(tempValue, tempValue, tempValue);
            }
            dotsObjects.Add(dot);
        }
        collisionDot = Instantiate(collisionDotPrefab);



        GameManager.isStart = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShot)
        {
            Aim();
        }
    }

    void Aim()
    {
        if (Input.GetMouseButton(0))
        {
            if (!aiming)
            {
                startPos = Input.mousePosition;
                CalculatePath();
                ShowPath();
                aiming = true;
            }
            else
            {
                CalculatePath();
            }
        }

        else if (aiming)
        {
            getThrown();
            //anim.enabled = false;
            transform.GetComponent<Rigidbody>().isKinematic = false;
            GetComponents<Collider>()[0].enabled = true;
            GetComponents<Collider>()[1].enabled = true;
            transform.GetComponent<Rigidbody>().AddForce(GetForce(Input.mousePosition));
            transform.GetComponent<Rigidbody>().AddTorque(Vector3.right * torqueForce);
            isShot = true;
            aiming = false;
            HidePath();
        }
    }

    Vector3 GetForce(Vector3 mouse)
    {

        if (!aiming)
        {
            lastHorizontal = mouse.x - initialHorizontal;
        }
        float horizontal = mouse.x;
        Vector3 force = lastForce + new Vector3(0f, 0f, (horizontal - lastHorizontal) * sensitivity);
        if (force.z > maxHorizontal) { force.z = maxHorizontal; }
        if (force.z < minHorizontal) { force.z = minHorizontal; }
        lastForce = force;
        lastHorizontal = horizontal;
        float horizontalPercentage = (force.z - minHorizontal) / maxHorizontal;
        force.y = minVertical + maxVertical - (maxVertical * horizontalPercentage);
        return force;
    }

    bool inReleaseZone(Vector3 mouse)
    {
        if (mouse.x <= 100)
            return true;

        return false;
    }

    //Calculate path
    void CalculatePath()
    {
        dotsTransform.gameObject.SetActive(true);

        CalculateDots();
    }


    /// <summary>
    /// Calculates and places the dots anticipating the arc of the ball
    /// </summary>
    void CalculateDots()
    {
        bool hasCollided = false;
        Vector3 vel = GetForce(Input.mousePosition) * Time.fixedDeltaTime / GetComponent<Rigidbody>().mass;
        for (int i = 0; i < dotsValut; i++)
        {
            if (!hasCollided)
            {
                dotsObjects[i].SetActive(true);
                float t = i / 30f;
                Vector3 point = PathPoint(transform.position, vel, t);
                //point.z = -1.0f;
                dotsObjects[i].transform.position = point;
                if (i > 5)
                {
                    RaycastHit hit;
                    Vector3 nextPoint = PathPoint(transform.position, vel, (i + 1) / 30f);
                    Physics.Raycast(dotsObjects[i].transform.position, nextPoint - point, out hit, .5f);
                    Debug.DrawRay(dotsObjects[i].transform.position, nextPoint - point, Color.green);

                    if (hit.collider != null && (hit.collider.tag == "Player" || hit.collider.tag == "Ground" || hit.collider.tag == "RespawnZone"))//Does the item collide with anything
                    {
                        hasCollided = true;//Stop drawing the next dots
                        collisionDot.SetActive(true);
                        collisionDot.transform.position = hit.point; 
                    }
                }
            }
            else { dotsObjects[i].SetActive(false);
            }
        }
        if (!hasCollided)
        {
            collisionDot.SetActive(false);
        }
    }

    //Get point position
    Vector3 PathPoint(Vector3 startP, Vector3 startVel, float t)
    {
        return startP + startVel * t + 0.5f * Physics.gravity * t * t;
    }

    //Hide all used dots
    void HidePath()
    {
        dotsTransform.gameObject.SetActive(false);
        collisionDot.SetActive(false);
    }

    //Show all used dots
    void ShowPath()
    {
        dotsTransform.gameObject.SetActive(true);
    }

    public void GetCaught()
    {
        isShot = false;
        //anim.enabled = true;
    }
}
