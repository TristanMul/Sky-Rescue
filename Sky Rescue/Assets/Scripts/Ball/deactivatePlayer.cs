using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deactivatePlayer : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
 //           other.transform.gameObject.SetActive(false);
        }
    }
}
