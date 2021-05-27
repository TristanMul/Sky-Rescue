using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public static CameraMovement instance;

    [SerializeField] Transform toFollowTest;

    Transform toFollow;
    Vector3 offset;
    [SerializeField] float lerpSpeed;
    public bool started = false;

    //Test with changing Fov
    Camera cam;
    Rigidbody playerRb;
    float normalFov;
    [SerializeField] float fovMult;

    // Start is called before the first frame update
    void Start()
    {
        if(instance == null)
        {
            instance = this;
        }
        toFollow = toFollowTest;
        offset = transform.position - new Vector3(toFollow.transform.position.x - 1.3f, toFollow.transform.position.y, toFollow.transform.position.z - 1.3f);
        playerRb = toFollow.GetComponent<Rigidbody>();
        cam = GetComponent<Camera>();
        normalFov = cam.fieldOfView;
    }

    private void FixedUpdate()
    {
        if (started)
        {
            transform.position = Vector3.Lerp(transform.position, toFollow.position + offset, lerpSpeed * Time.deltaTime);
            float addedFov = playerRb.velocity.magnitude * fovMult;
            addedFov = Mathf.Clamp(addedFov, 0, 100);
            cam.fieldOfView = normalFov + addedFov;
        }
    }

    public void SwitchTarget(Transform target)
    {
        toFollow = target;
        playerRb = toFollow.GetComponent<Rigidbody>();
    }
}
