using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallController : MonoBehaviour
{
    public float power;
    public float sensitivity;

    public int dotsValut;

    private float interval = 1 / 30.0f;

    private bool aiming = false;
    private bool isShot = false;

    private Vector3 startPos;
    private List<GameObject> dotsObjects = new List<GameObject>();

    public GameObject dotPrefab;
    public Transform dotsTransform;
    public GameManager gameManager;

    private int basketCounter = 0;

    // Use this for initialization
    void Start()
    {
        transform.GetComponent<Rigidbody2D>().isKinematic = true;
        transform.GetComponent<Collider2D>().enabled = false;
        startPos = transform.position;

        float tempValue = 1f;

        for (int i = 0; i < dotsValut; i++)
        {
            GameObject dot = Instantiate(dotPrefab);
            dot.transform.parent = dotsTransform;
            if (i > 10)
            {
                dot.transform.localScale = new Vector2(tempValue, tempValue);
                tempValue -= 0.05f;
            }

            dotsObjects.Add(dot);
        }

        dotsTransform.gameObject.SetActive(false);
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
                aiming = true;
                startPos = Input.mousePosition;
                CalculatePath();
                ShowPath();
            }
            else
            {
                CalculatePath();
            }
        }
        else if (aiming)
        {
            AudioController.Instance.PlayAudio(AudioController.Instance.shotSound);

            transform.GetComponent<Rigidbody2D>().isKinematic = false;
            transform.GetComponent<Collider2D>().enabled = true;
            transform.GetComponent<Rigidbody2D>().AddForce(GetForce(Input.mousePosition));
            isShot = true;
            aiming = false;
            HidePath();
        }

    }

    Vector2 GetForce(Vector3 mouse)
    {
        return (new Vector2(startPos.x, startPos.y) - new Vector2(mouse.x, mouse.y)) * power;
    }

    bool inReleaseZone(Vector2 mouse)
    {
        if (mouse.x <= 70)
            return true;

        return false;
    }

    //Calculate path
    void CalculatePath()
    {
        dotsTransform.gameObject.SetActive(true);

        Vector2 vel = GetForce(Input.mousePosition) * Time.fixedDeltaTime / GetComponent<Rigidbody2D>().mass;
        for (int i = 0; i < dotsValut; i++)
        {
            float t = i / 30f;
            Vector3 point = PathPoint(transform.position, vel, t);
            point.z = -1.0f;
            dotsObjects[i].transform.position = point;
        }

    }

    //Get point position
    Vector2 PathPoint(Vector2 startP, Vector2 startVel, float t)
    {
        return startP + startVel * t + 0.5f * Physics2D.gravity * t * t;
    }

    //Hide all used dots
    void HidePath()
    {
        dotsTransform.gameObject.SetActive(false);
    }

    //Show all used dots
    void ShowPath()
    {
        dotsTransform.gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Floor")
        {
            AudioController.Instance.PlayAudio(AudioController.Instance.bounseSound);
            gameManager.GameOver();
        }
        if (collision.gameObject.tag == "Rim")
        {
            AudioController.Instance.PlayAudio(AudioController.Instance.rimSound);
        }
        if (collision.gameObject.tag == "Wall")
        {
            AudioController.Instance.PlayAudio(AudioController.Instance.bounseSound);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Net")
        {
            isShot = false;
            collision.gameObject.GetComponent<Animator>().SetTrigger("IsGoal");
            collision.gameObject.GetComponent<Collider2D>().enabled = false;

            ScoreManager.Instance.AddScore(1);
            CameraFollow.cameraIsMove = true;
            AudioController.Instance.PlayAudio(AudioController.Instance.netSound);

            // Create new basket
            GameObject basket = BasketPoolManager.Instace.GetPooledObject();
            if (basket != null)
            {
                switch (basketCounter)
                {
                    case 0:
                        basket.transform.position = new Vector2(-2.5f, transform.position.y + Random.Range(4f, 6f));
                        basket.transform.rotation = Quaternion.Euler(0, 0, -30f);
                        basket.SetActive(true);
                        basketCounter++;
                        break;
                    case 1:
                        basket.transform.position = new Vector2(2.5f, transform.position.y + Random.Range(4f, 6f));
                        basket.transform.rotation = Quaternion.Euler(0, 0, 30f);
                        basket.SetActive(true);
                        basketCounter = 0;

                        break;
                }
            }
        }
        // Add star
        if (collision.gameObject.tag == "Star")
        {
            collision.gameObject.SetActive(false);
            MoneyManager.Instance.AddMoney(10);
            AudioController.Instance.PlayAudio(AudioController.Instance.coinSound);
        }
    }
}
