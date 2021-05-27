using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationController : MonoBehaviour
{
    [SerializeField] GameObject rotateBlock;
    [SerializeField] float sensitivity;
    [SerializeField] float rotationLimit;
    [SerializeField] float maxRotationSpeed;
    [HideInInspector] public Rigidbody rotateRb;
    float lastMouseX;
    Rigidbody rb;

    bool hasFinished;

    // Start is called before the first frame update
    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rotateRb = rotateBlock.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasFinished)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                lastMouseX = Input.mousePosition.x;
                rotateRb.isKinematic = false;
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                float rotationSpeed = -(Input.mousePosition.x - lastMouseX) * Time.deltaTime * sensitivity;
                if (rotationSpeed > maxRotationSpeed) { rotationSpeed = maxRotationSpeed; }//clamps the rotation speed so it's not too high
                if (rotationSpeed < -maxRotationSpeed) { rotationSpeed = -maxRotationSpeed; }
                rotateRb.angularVelocity = new Vector3(0f, -rotationSpeed, 0f);

                // to make sure the transform doesn't magically rotate
                rotateRb.transform.rotation = new Quaternion(0f, rotateBlock.transform.rotation.y, 0f, rotateRb.transform.rotation.w);

                lastMouseX = Input.mousePosition.x;
            }
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                rotateRb.isKinematic = true;

            }

            //Clamps the rotation of the block
            rotateBlock.transform.rotation =
                new Quaternion(rotateBlock.transform.rotation.x, Mathf.Clamp(rotateBlock.transform.rotation.y, -rotationLimit, rotationLimit),
                rotateBlock.transform.rotation.z,
                rotateBlock.transform.rotation.w);
            rotateBlock.transform.position = transform.position;
        }
    }


}
