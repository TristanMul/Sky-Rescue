using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// ArcCalculator is used to calculate the arc between two transforms.
///
/// Example: 
///     public Rigidbody throwableObject;
///     public Transform target;
///     public float height;
/// 
///     private ArcCalculator arcCalculator;
/// 
///     private void Start() {
///         arcCalculator = new ArcCalculator();
///     }
/// 
///     void Launch{
///         throwableObject.velocity = arcCalculator.CalculateArc(throwableObject.transform, target, height);
///     }
/// 
/// </summary>
public class ArcCalculator
{
    private float gravity;

    public ArcCalculator(){
        gravity = Physics.gravity.y;
    }

    /// <summary>
    /// CalculateArc is used to calculate the arc between two transforms.
    /// The height parameter should always be higher than the height of the throwableObejct and target.
    /// </summary>
    /// <param name="throwableObject"> The starting position of the arc </param>
    /// <param name="target"> The end position of the arc </param>
    /// <param name="height"> The maximum height of the arc </param>
    /// <returns></returns>
    public Vector3 CalculateArc(Transform throwableObject, Transform target, float height){
        float displacementY = target.position.y - throwableObject.position.y;
        Vector3 displacementXZ  = new Vector3(target.position.x - throwableObject.position.x, 0f, target.position.z - throwableObject.position.z);
        float time = Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (displacementY - height) / gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = displacementXZ / time;

        return velocityXZ + velocityY * -Mathf.Sign(gravity);
    }

}
