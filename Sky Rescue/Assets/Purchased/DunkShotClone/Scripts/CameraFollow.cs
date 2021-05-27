using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject target;
    public GameObject destroyColl;

    public static bool cameraIsMove = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && GameManager.isStart)
        {
            destroyColl.SetActive(true);
            Invoke("DisableDestroyCollider", 0.2f);
        }

        if (cameraIsMove)
        {
            transform.position = new Vector3(transform.position.x, 
                                             Mathf.Lerp(transform.position.y, target.transform.position.y + 4f, 0.02f), 
                                             transform.position.z);


            if (transform.position.y >= target.transform.position.y + 3.5f)
            {
                cameraIsMove = false;
            }
        }
    }

    private void DisableDestroyCollider()
    {
        destroyColl.SetActive(false);
    }
}
