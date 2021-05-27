using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BasketController : MonoBehaviour {

    public Collider2D coll;
    public GameObject Star;

    private void OnEnable()
    {
        if (Star != null)
        {
            Star.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DestroyPool")
        {
            coll.enabled = true;
            gameObject.SetActive(false);
        }
    }
}
