using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//旋转组件

//[RequireComponent(typeof(Rigidbody2D))]
public class Rotate : MonoBehaviour
{
    public float angularSpeed = 1f;
    public bool clockWise = true;

#if UNITY_EDITOR
#else
    private Vector3 rorateVector;
#endif

    //private Rigidbody2D m_rigidbody2D;

    // Use this for initialization
    void Start() {
        //m_rigidbody2D = GetComponent<Rigidbody2D>();

#if UNITY_EDITOR
#else
        rorateVector = new Vector3(0, 0, (clockWise ? -1f : 1f) * angularSpeed);
#endif
    }

    // Update is called once per frame
    void FixedUpdate() {


#if UNITY_EDITOR
        transform.Rotate(new Vector3(0, 0, (clockWise ? -1f : 1f) * angularSpeed));
#else
        transform.Rotate(rorateVector);
#endif

    }
}
