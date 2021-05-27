using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] private GameObject passHeight;
    [SerializeField] private GameObject scoreTarget;
    [SerializeField] private GameObject missTarget;

    public Transform PassHeightTransform { get{return passHeight.transform;} }
    public Transform ScoreTargetTransform { get{return scoreTarget.transform;} }
    public Transform MissTargetTransform { get{return missTarget.transform; } }
}
