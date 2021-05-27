using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeVisible : MonoBehaviour
{
    private void OnBecameVisible()
    {
/*        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).gameObject.transform.parent = null;
        Destroy(this);*/
    }
    private void OnBecameInvisible()
    {
        //Ball.enemyBall.isVisible = false;
    }
}
