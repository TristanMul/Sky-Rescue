using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private GameObject passHeight;
    [SerializeField] private GameObject passTarget;

    public Transform PassHeightTransform { get{return passHeight.transform;} }
    public Transform PassTargetTransform { get{return passTarget.transform;} }
}
